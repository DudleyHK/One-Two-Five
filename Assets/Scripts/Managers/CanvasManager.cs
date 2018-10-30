using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject items;
    
    [SerializeField] private Text screenResolution;
    [SerializeField] private Toggle HUDActive;
    [SerializeField] private Toggle kamakaziEnemies;
    [SerializeField] private Toggle chaserEnemies;
    [SerializeField] private Toggle collectables;

    [SerializeField] private Slider chaserSpeed;
    [SerializeField] private Slider chaserInterval;
    [SerializeField] private Slider kamakaziInterval;
    [SerializeField] private Slider collectableInterval;


    [SerializeField] private Text chaserSpeedLabel;
    [SerializeField] private Text chaserIntervalLabel;
    [SerializeField] private Text kamakaziIntervalLabel;
    [SerializeField] private Text collectableIntervalLabel;


    private void OnEnable()
    {
        HUDActive.onValueChanged.AddListener(ToggleHUD);
        
        kamakaziEnemies.onValueChanged.AddListener(ToggleKamakaziEnemies);
        chaserEnemies.onValueChanged.AddListener(ToggleChaserEnemies);
        collectables.onValueChanged.AddListener(ToggleCollectables);

        chaserSpeed.onValueChanged.AddListener(AdjustChaserSpeed);
        chaserInterval.onValueChanged.AddListener(AdjustChaserInterval);
        kamakaziInterval.onValueChanged.AddListener(AdjustKamakaziInterval);
        collectableInterval.onValueChanged.AddListener(AdjustCollectionInterval);
    }

    private void OnDisable()
    {
        HUDActive.onValueChanged.RemoveAllListeners();
        
        kamakaziEnemies.onValueChanged.RemoveAllListeners();
        chaserEnemies.onValueChanged.RemoveAllListeners();
        collectables.onValueChanged.RemoveAllListeners();

        chaserSpeed.onValueChanged.RemoveAllListeners();
        chaserInterval.onValueChanged.RemoveAllListeners();
        kamakaziInterval.onValueChanged.RemoveAllListeners();
        collectableInterval.onValueChanged.RemoveAllListeners();
    }


    private void Start()
    {
        screenResolution.text = "Screen Resolution: " + Screen.currentResolution;

        chaserSpeed.value = EnemyManager.ChaserSpeed;
        chaserInterval.value = EnemyManager.ChaserInterval;
        kamakaziInterval.value = EnemyManager.KamakaziInterval;
        collectableInterval.value = CollectablesManager.CollectableInterval;

        chaserSpeedLabel.text = chaserSpeed.value.ToString();
        chaserIntervalLabel.text = chaserInterval.value.ToString();
        kamakaziIntervalLabel.text = kamakaziInterval.value.ToString();
        collectableIntervalLabel.text = collectableInterval.value.ToString();
        
    }


    private void ToggleHUD(bool _isOn)
    {
        items.SetActive(_isOn);
    }


    private void ToggleKamakaziEnemies(bool _isOn)
    {
        EnemyManager.KamakaziActive = _isOn;
    }


    private void ToggleChaserEnemies(bool _isOn)
    {
        EnemyManager.ChaserActive = _isOn;
    }


    private void ToggleCollectables(bool _isOn)
    {
        CollectablesManager.CollectablesActive = _isOn;
    }

    private void AdjustChaserSpeed(float _val)
    {
        EnemyManager.ChaserSpeed = _val;
        chaserSpeedLabel.text = _val.ToString();
    }

    private void AdjustChaserInterval(float _val)
    {
        EnemyManager.ChaserInterval = _val;
        chaserIntervalLabel.text = _val.ToString();
    }


    private void AdjustKamakaziInterval(float _val)
    {
        EnemyManager.KamakaziInterval = _val;
        kamakaziIntervalLabel.text = _val.ToString();
    }


    private void AdjustCollectionInterval(float _val)
    {
        CollectablesManager.CollectableInterval = _val;
        collectableIntervalLabel.text = _val.ToString();
    }
}