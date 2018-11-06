using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private RewardedAdButton continueButton;
    [SerializeField] private float waitToPause = 0f;


    private void OnEnable()
    {
        RewardedAdButton.applyReward += UnpauseGame;
    }


    private void OnDisable()
    {
        RewardedAdButton.applyReward -= UnpauseGame;
    }


    private void Update()
    {
        settingsButton.interactable = !optionsPanel.activeInHierarchy;
    }
    

    public void SetActive(bool _active, bool _displayAd)
    {
        optionsPanel.SetActive(_active);
        
        PauseGame(_active);
        
        continueButton.DisplayAd = _displayAd;

        if (!_active) 
            StartCoroutine(PlayerHit.SetInvinsible());
    }

    
    public void SetActiveFromIcon()
    {
        SetActive(true, false);
    }

    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    

    private void UnpauseGame(bool _applyReward)
    {
        if (_applyReward)
        {
            SetActive(false, true);   
        }
        else
        {
            SetActive(false, false);
        }
    }
    
    
    private void PauseGame(bool _pause)
    {
        Time.timeScale = _pause ? 0 : 1;
    }
}
