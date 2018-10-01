using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CanvasManager : MonoBehaviour
{

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Text screenResolution;

    [Header("Player")]
    [SerializeField] private Slider playerSpeed;
    [SerializeField] private Slider playerTurn;
    [SerializeField] private Slider playerSlowDamp;

    [Header("Collectables")]
    [SerializeField] private Button collectableToggle;
    
    [Header("Enemies")]
    [SerializeField] private Button kamakaziEnemyToggle;
    [SerializeField] private Slider kamakaziEnemySpeed;
    [SerializeField] private Slider kamakaziEnemyInterval;
    
    
    private bool collectablesActive = false;
    

    private void Start()
    {
        screenResolution.text = "Screen Resolution: " + Screen.currentResolution;
        
        collectableToggle.onClick.AddListener(ToggleCollectables);
    }
    
    private void ToggleCollectables()
    {
        collectablesActive = !collectablesActive;
    }
}
