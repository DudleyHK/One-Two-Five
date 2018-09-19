using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CanvasManager : MonoBehaviour
{

    [SerializeField] Canvas mainCanvas;
    [SerializeField] Text screenResolution;
    


    private void Start()
    {
        screenResolution.text = "Screen Resolution: " + Screen.currentResolution;
    }
}
