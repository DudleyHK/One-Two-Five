using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.UI;

public class RewardedAdButton : MonoBehaviour 
{
	public delegate void ApplyReward(bool _applied);
	public static event ApplyReward applyReward;

	[HideInInspector] public bool DisplayAd;
	
	[SerializeField] private Button button;
	
	private string placementId = "rewardedVideo";
	private bool used;


	
	private void Start()
	{
		used = false;
	}


	private void Update()
	{
		if (button)
		{
			button.interactable = Monetization.IsReady(placementId) && !used;
		}
	}
	
	
	public void ShowAd()
	{
		if (DisplayAd)
		{
			ShowAdCallbacks options = new ShowAdCallbacks();
			options.finishCallback = HandleShowResult;

			ShowAdPlacementContent ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;
			ad.Show(options);
		}
		else
		{
			if (applyReward != null)
				applyReward(false);
		}
	}


	private void HandleShowResult(ShowResult _result)
	{
		if (_result == ShowResult.Finished)
		{
			if (applyReward != null)
				applyReward(true);
			
			used = true;
		}
		else if (_result == ShowResult.Skipped)
		{
			Debug.LogWarning("Warning: The player skipped the video - DO NOT REWARD!");
		}
		else if (_result == ShowResult.Failed)
		{
			Debug.LogError("Error: Video failed to show");
		}
		else
		{
			// Do nothing
		}
	}
}
