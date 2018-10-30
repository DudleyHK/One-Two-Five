using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public bool move = false;
    public bool slow = false;
    public float speed = 15f;
    public float damping = 5f;
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
    [SerializeField] private float deadzone = 5f;

    private Vector3 ppTargetPos;
    private Vector3 wpTargetPos;
    
    private Vector3 currentAngle = Vector3.zero;

    private ushort touchEndCount = 0;
    


    private void Start()
    {
        currentAngle = transform.eulerAngles;
    }


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
        if (move)
        {
            wpTargetPos = Camera.main.ScreenToWorldPoint(ppTargetPos);

            if (Vector3.Distance(transform.position, wpTargetPos) < deadzone)
            {
               // StartCoroutine(DelayCoroutine(StopBike(), 2f));
            }
            else
            {
                Rotate();
                Movement();
            }
        }

        if (slow)
        {
            StopCoroutine(StopBike());
            StartCoroutine(StopBike());

            slow = false;
        }
    }

    private void Movement()
    {
        Debug.Log("CAR MOVING");
        
        transform.position += transform.right * speed * Time.fixedDeltaTime;
    }


    private void Rotate()
    {
        var offset = new Vector2(wpTargetPos.x - transform.position.x, wpTargetPos.z - transform.position.z);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var targetAngle = new Vector3(90f, 0f, angle);

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, turn * Time.fixedDeltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, turn * Time.fixedDeltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, turn * Time.fixedDeltaTime));

        transform.eulerAngles = currentAngle;
    }

    
    private IEnumerator DelayCoroutine(IEnumerator _func, float _t)
    {
        Debug.Log("Wating to Delay Func");
        yield return new WaitForSeconds(_t);

        Debug.Log("Func Run");
        StartCoroutine(_func);

        yield return true;
    }


    private IEnumerator StopBike()
    {
        var temp = damping;
        var rate = temp / 2f;
        
        while (temp >= 0f)
        {
            temp -= rate * Time.fixedDeltaTime;
            transform.position += transform.right * temp * Time.fixedDeltaTime;

            yield return false;
        }
        yield return true;
    }


    private void HandleMouseInput(Vector3 _mouse, bool _move, bool _slow)
    {
        ppTargetPos = _mouse;

        move = _move;
        slow = _slow;
    }


    private void HandleInput(Touch _touch)
    {
        ppTargetPos = _touch.position;

        debugText.text = "touch input pos - " + ppTargetPos;
        Debug.Log("touch input pos - " + ppTargetPos);


        if (_touch.phase == TouchPhase.Began)
        {
            move = true;
            slow = false;
        }
        else if (_touch.phase == TouchPhase.Ended)
        {
            move = false;
            slow = true;
        }
        else
        {
            // Do nothing.
        }
    }
}