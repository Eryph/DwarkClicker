namespace Preprod.UI
{
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using System;
	using TMPro;

	public class DebugPanelDEBUG : MonoBehaviour
	{
		#region Fields
		[SerializeField] private InputField _goldField = null;
		[SerializeField] private InputField _beerField = null;
		[SerializeField] private TextMeshProUGUI _timeElapsedText = null;

		private PlayerProfile _playerProfile = null;
		#endregion Fields

		#region Methods
		public void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			GameLoopManager.Instance.GameLoop += TotalTimeDisplay;
		}

		private void OnDestroy()
		{
			if (GameLoopManager.Instance)
				GameLoopManager.Instance.GameLoop -= TotalTimeDisplay;
		}

		public void ResetPlayerProfile()
		{
			_playerProfile.Reset();
		}

		public void SetTimeScale(int t)
		{
			Time.timeScale = t;
		}

		public void TotalTimeDisplay()
		{
			TimeSpan elapsedTime = DateTime.Now.Subtract(_playerProfile.StartingDate);
			_timeElapsedText.text = string.Format("{0}J - {1}H - {2}m", (int)elapsedTime.Days, (int)elapsedTime.Hours, (int)elapsedTime.Minutes);
		}

		public void AddBeer()
		{
			_playerProfile.CurrentFortress.Beer += Convert.ToInt32(_beerField.text);
		}

		public void AddGold()
		{
			_playerProfile.Gold += Convert.ToInt32(_goldField.text);
		}
		#endregion Methods
	}
}