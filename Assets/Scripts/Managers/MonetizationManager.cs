namespace Engine.Manager
{
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Advertisements;

	public class MonetizationManager : Singleton<MonetizationManager>, IUnityAdsListener
	{
		#region Fields
		[SerializeField] private string _projectId = "123456789";
		[SerializeField] private string _androidGameId = "1234567";
		[SerializeField] private string _IOSgameId = "1234567";
		[SerializeField] private string _placementId = "rewardedVideo";
		[SerializeField] private bool _testMode = true;
		#endregion Fields

		#region Events
		private event Action _adFinished = null;
		
		public event Action AdFinished
		{
			add
			{
				_adFinished -= value;
				_adFinished += value;
			}
			remove
			{
				_adFinished -= value;
			}
		}

		private event Action _onSDKReady = null;

		public event Action OnSDKReady
		{
			add
			{
				_onSDKReady -= value;
				_onSDKReady += value;
			}
			remove
			{
				_onSDKReady -= value;
			}
		}
		#endregion Events

		#region Methods
		protected override void Start()
		{
			base.Start();
			Advertisement.AddListener(this);

			if (Advertisement.isSupported)
			{
				Advertisement.Initialize(_androidGameId, _testMode);
			}
			else
			{
				Debug.LogError("AD IS NOT SUPPORTED");
			}
		}

    #region Ads
		// Implement IUnityAdsListener interface methods:
		public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
		{
			// Define conditional logic for each ad completion status:
			if (showResult == ShowResult.Finished)
			{
				if (_adFinished != null)
				{
					_adFinished();
				}
			}
			else if (showResult == ShowResult.Skipped)
			{
				Debug.LogWarning("The ad was skipped, no reward.");
			}
			else if (showResult == ShowResult.Failed)
			{
				Debug.LogWarning("The ad did not finish due to an error.");
			}
		}

		public void OnUnityAdsReady(string placementId)
		{
			Debug.Log(placementId + " placement is ready !");
			//if (_onSDKReady != null)
			//	_onSDKReady();
		}

		public void OnUnityAdsDidError(string message)
		{
			// Log the error.
		}

		public void OnUnityAdsDidStart(string placementId)
		{
			// Optional actions to take when the end-users triggers an ad.
		}

		public void ShowAd()
		{
			if (Advertisement.isInitialized)
			{
                if (Advertisement.IsReady(_placementId))
                {
                    Advertisement.Show(_placementId);
                    AchievementManager.Instance.UpdateAchievement("ADS", 1);
                }
                else
                {
                    Debug.LogError("AD NOT READY YET");
                }
			}
			else
			{
				Debug.LogError("AD IS NOT INITIALIZED");
			}
		}
		#endregion Ads

		#region IAP
		#endregion IAP
		#endregion Methods
	}
}