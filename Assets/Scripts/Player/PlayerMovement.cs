using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public bool move = false;
    public float speed = 150f;
    public float reverse = 100f;
    public float turn = 5f;

    [SerializeField] enum Direction {None, Forward, Reverse }
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


    private void FixedUpdate()
    {
        var direction = Direction.None;
        
        if (move)
        {
            var targetWorldPos = Camera.main.ScreenToWorldPoint(targetPos);

            // check where the click is. 
            var signedAngle = Vector3.SignedAngle(transform.localPosition, targetWorldPos, transform.forward);
            var pos = Mathf.Cos(signedAngle); // zAxis
            
            
            Debug.Log("Angle of click from player is " + signedAngle);
            
            
            
            // Position
            //if (transform.right.z > (transform.right - transform.position).normalized.z)
            //{
                direction = Direction.Forward;
            //}
            //else
            //{
            //    direction = Direction.Reverse;
            //    Debug.DrawRay(transform.position, transform.right * -10f, Color.red, 1f);
            //}

            

            if (direction != Direction.None)
            {
                var adjustSpeed = direction == Direction.Forward ? speed : reverse;
                
                transform.position = TweenLibrary.EaseInOutLinear(transform.position,
                    new Vector3(targetWorldPos.x, transform.position.y, targetWorldPos.z), adjustSpeed * Time.fixedDeltaTime);
            }
            else
            {
                Debug.LogWarning("Warning: Direction " + direction + " invalid!");
            }
            
            // Rotation
            var pixelPos = Camera.main.WorldToScreenPoint(transform.localPosition);
            var offset = new Vector2(targetPos.x - pixelPos.x, targetPos.y - pixelPos.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(90f, 0f, angle);
        }
    }

    private void HandleMouseInput(Vector3 _mouse, bool _move)
    {
        targetPos = _mouse;
        move = _move;
        
       //if(move)
       //    Debug.Log("mouse pos " + _mouse);
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