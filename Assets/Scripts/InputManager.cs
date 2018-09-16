using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;



public class InputManager : MonoBehaviour
{
    public delegate void HandleInput(Touch _touch);
    public static event HandleInput handleInput;
    
    public delegate void HandleMouseInput(Vector3 _mousePixPos, bool _move);
    public static event HandleMouseInput handleMouseInput;

    [SerializeField] Text debugText;

    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 direction;



    private void Update()
    {
        
#if UNITY_EDITOR
        MouseInput();
#else
        TouchInput();
#endif

    }


    private void MouseInput()
    {
       
        
        if (handleMouseInput != null)
            handleMouseInput(Input.mousePosition, Input.GetMouseButton(0));
    }



    private void TouchInput()
    {
        if (Input.touchCount <= 0) return;

        // TODO: Get the most recent touch and use that as the input.
        var touch = Input.GetTouch(0);

        if (handleInput != null)
            handleInput(touch);

        var output = "";

        switch (touch.phase)
        {
            case TouchPhase.Began: output = "Began"; break;
            case TouchPhase.Moved: output = "Moved"; break;
            case TouchPhase.Ended: output = "Ended"; break;
        }

        debugText.text = output;
        Debug.Log(output);
    }
}