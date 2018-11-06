using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Monetization;


public class UnityAdsManager : MonoBehaviour
{

#if UNITY_EDITOR
    private bool testMode = true;
    private string gameId = "1111117";
#else 
    private bool testMode = true;
    
    #if UNITY_ANDROID
        private string gameId = "2893538";
    #elif UNITY_IOS
        private string gameId = "2893539";
    #endif
#endif

    
    
    private void Start()
    {
        if (Monetization.isSupported)
            Monetization.Initialize(gameId, testMode);
    }
}