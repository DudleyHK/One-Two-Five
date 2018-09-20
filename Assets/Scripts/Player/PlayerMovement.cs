using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public bool move = false;
    public float speed = 150f;
    public float reverse = 100f;
    public float turn = 5f;

    [SerializeField]
    enum Direction
    {
        None,
        Forward,
        Reverse
    }

    [SerializeField] Camera mainCamera;
    [SerializeField] Text debugText;

    private Vector3 ppTargetPos;
    private Vector3 wpTargetPos;


    private void OnEnable()
    {
        InputManager.handleInput += HandleInput;
        InputManager.handleMouseInput += HandleMouseInput;
    }


    private void OnDisable()
    {
        InputManager.handleInput -= HandleInput;
        InputManager.handleMouseInput -= HandleMouseInput;
    }


    private void FixedUpdate()
    {
        var direction = Direction.None;

        if (move)
        {
            wpTargetPos = Camera.main.ScreenToWorldPoint(ppTargetPos);


             Rotate();
            Movement();
        }
    }

    private void Movement()
    {
        transform.position = TweenLibrary.EaseInOutLinear(transform.position,
            new Vector3(wpTargetPos.x, transform.position.y, wpTargetPos.z),
            speed * Time.fixedDeltaTime);
    }

    
    
    private void Rotate()
    {
        // Current Angle
        var currOffset = new Vector3(transform.position.x - transform.forward.x, 0f,
            transform.position.z - transform.forward.z);
        var currAngle = Mathf.Atan2(currOffset.z, currOffset.x) * Mathf.Rad2Deg;

        // Target Angle
        var offset = new Vector3(wpTargetPos.x - transform.position.x, 0f,
            wpTargetPos.z - transform.position.z);
        var angle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;
        
        var newAngle = Mathf.LerpAngle(currAngle, angle, turn * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(90f, 0f,newAngle );





       
        Debug.DrawRay(transform.position, wpTargetPos, Color.cyan, 3f);
    }

    private void HandleMouseInput(Vector3 _mouse, bool _move)
    {
        ppTargetPos = _mouse;
        move = _move;
    }


    private void HandleInput(Touch _touch)
    {
        ppTargetPos = _touch.position;

        debugText.text = "touch input pos - " + ppTargetPos;
        Debug.Log("touch input pos - " + ppTargetPos);


        if (_touch.phase == TouchPhase.Began)
        {
            move = true;
        }
        else if (_touch.phase == TouchPhase.Ended)
        {
            move = false;
        }
        else
        {
            // Do nothing.
        }
    }
}