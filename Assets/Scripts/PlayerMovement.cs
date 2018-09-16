using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public bool move = false;
    public float speed = 150f;
    public float reverse = 100f;
    public float turn = 5f;

    [SerializeField] Camera mainCamera;
    [SerializeField] Text debugText;

    private Vector3 targetPos;


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


    private void Update()
    {
        if (move)
        {
            var pixelPos = Camera.main.WorldToScreenPoint(transform.localPosition);
            var offset = new Vector2(targetPos.x - pixelPos.x, targetPos.y - pixelPos.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;


            transform.rotation = Quaternion.Euler(90f, 0f, angle);
            transform.position += transform.right * speed * Time.deltaTime;
            
        }
    }

    private void HandleMouseInput(Vector3 _mouse, bool _move)
    {
        targetPos = _mouse;
        move = _move;
        
       if(move)
           Debug.Log("mouse pos " + _mouse);
    }


    private void HandleInput(Touch _touch)
    {
        targetPos =  _touch.position;

        debugText.text = "touch input pos - " + targetPos;
        Debug.Log("touch input pos - " + targetPos);
        

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