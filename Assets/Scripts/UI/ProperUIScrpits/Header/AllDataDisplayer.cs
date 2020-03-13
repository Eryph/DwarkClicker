namespace Preprod.UI
{
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;
	using DwarkClicker.Helper;
	using System;

	public class AllDataDisplayer : MonoBehaviour
	{
		#region Fields
		[SerializeField] private TextMeshProUGUI _goldDisplay = null;
		[SerializeField] private TextMeshProUGUI _beerDisplay = null;
		[SerializeField] private TextMeshProUGUI _mithrilDisplay = null;
		[SerializeField] private GameObject _dailyRewardDisabledButton = null;
		[SerializeField] private GameObject _dailyRewardEnabledButton = null;
		[SerializeField] private GameObject _dailyRewardPanel = null;
		[SerializeField] private GameObject _fortressPanel = null;
		[SerializeField] private GameObject _kingPanel = null;
		[SerializeField] private GameObject _shopPanel = null;
		[SerializeField] private GameObject _settingsPanel = null;
		[SerializeField] private TextMeshProUGUI _fortressCount = null;

		private PlayerProfile _playerProfile = null;
		#endregion Fields

		#region Methods
		private void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;

			_playerProfile.OnGoldChange += UpdateGoldDisplay;
			_playerProfile.CurrentFortress.OnBeerChange += UpdateBeerDisplay;
			_playerProfile.OnMithrilChange += UpdateMithrilDisplay;
			_playerProfile.OnFortressChange += UpdateFortress;

			UpdateGoldDisplay();
			UpdateBeerDisplay();
			UpdateMithrilDisplay();
			UpdateFortress();

			_fortressCount.text = _playerProfile.FortressCount.ToString();

			if (JSonManager.Instance.PlayerProfile.IsDailyRewardAvailable)
			{
				_dailyRewardDisabledButton.SetActive(false);
				_dailyRewardEnabledButton.SetActive(true);
			}
			else
			{
				_dailyRewardDisabledButton.SetActive(true);
				_dailyRewardEnabledButton.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			_playerProfile.OnGoldChange -= UpdateGoldDisplay;
			_playerProfile.OnBeerChange -= UpdateBeerDisplay;
			_playerProfile.OnMithrilChange -= UpdateMithrilDisplay;
		}

		private void UpdateGoldDisplay()
		{
			string displayText = UIHelper.FormatIntegerString(_playerProfile.Gold);
			_goldDisplay.text = displayText; 
		}

		private void UpdateBeerDisplay()
		{
			string displayText = UIHelper.FormatIntegerString((int)_playerProfile.CurrentFortress.Beer);
			_beerDisplay.text = displayText;
		}

		private void UpdateMithrilDisplay()
		{
			string displayText = UIHelper.FormatIntegerString(_playerProfile.Mithril);
			_mithrilDisplay.text = displayText;
		}

		private void UpdateFortress()
		{
			_playerProfile.CurrentFortress.OnBeerChange += UpdateBeerDisplay;
			UpdateBeerDisplay();
		}

		public void OpenFortressPanel()
		{
			_fortressPanel.SetActive(true);
		}

		public void OpenDailyRewardPanel()
		{
			_dailyRewardPanel.SetActive(true);
		}

		public void OpenKingPanel()
		{
			_kingPanel.SetActive(true);
		}

		public void OpenShopPanel()
		{
            if (JSonManager.Instance.PlayerProfile.FTUEStep != 2 &&
               JSonManager.Instance.PlayerProfile.FTUEStep != 5 &&
               JSonManager.Instance.PlayerProfile.FTUEStep != 8 &&
               JSonManager.Instance.PlayerProfile.FTUEStep != 14)
            {
                _shopPanel.SetActive(true);
            }
		}

		public void OpenSetings()
		{
			_settingsPanel.SetActive(true);
		}

		public void SetDailyRewardButtonDisabled()
		{
			_dailyRewardDisabledButton.SetActive(true);
			_dailyRewardEnabledButton.SetActive(false);
		}
		#endregion Methods
	}
}