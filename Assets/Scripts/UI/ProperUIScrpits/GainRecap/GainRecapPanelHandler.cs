namespace DwarfClicker.UI.GainRecap
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using Engine.Manager;
	using System;
	using UnityEngine.UI;
	using DwarkClicker.Helper;

	public class GainRecapPanelHandler : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _timeElapsedText = null;
		[SerializeField] private TextMeshProUGUI _goldGainText = null;
		[SerializeField] private TextMeshProUGUI _mithrilGainText = null;
		[SerializeField] private Transform _producedContainer = null;
		[SerializeField] private Transform _consumedContainer = null;
		[SerializeField] private ScrollIconHandler _scrollIconHandler = null;
		[SerializeField] private int _timePassedActivationMinutes = 2;

		/*[SerializeField] private Button _globalQuitButton = null;
		[SerializeField] private GameObject _container = null;
		[SerializeField] private Transform _timeElapsedContainer = null;
		[SerializeField] private Transform _goldContainer = null;
		[SerializeField] private Transform _mithrilContainer = null;
		
		*/
		private bool _shouldDisplay = false;
		private bool _shouldNowDisplay = false;

        private void Start()
        {
            GameManager.Instance.OnProgressionLoad += Display;
        }

        private void OnEnable()
		{
			if (JSonManager.Instance.PlayerProfile.LaunchAmount > 0 && FTUEManager.Instance.IsActivated == false)
				Display();
			else
			{
				gameObject.SetActive(false);
            }
		}

		private void Display()
		{
            gameObject.SetActive(true);
			ProgressionLoadInventory inv = GameManager.Instance.ProgressionInventory;
			/*TextMeshProUGUI timeElapsedText = _timeElapsedContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI producedText = _producedContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI consumedText = _consumedContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI goldText = _goldContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI mithrilText = _mithrilContainer.GetComponentInChildren<TextMeshProUGUI>();
			*/

			

			if (inv.HasChanges == false || inv.TimePassed < new TimeSpan(0, _timePassedActivationMinutes, 0))
			{
				QuitPanel();
				return;
			}

			_timeElapsedText.text = UIHelper.FormatTimeSpanToString(inv.TimePassed);
			_goldGainText.text = UIHelper.FormatIntegerString(inv.Gold);
			_mithrilGainText.text = UIHelper.FormatIntegerString(inv.Mithril);

			foreach (KeyValuePair<string, int> pair in inv.ConsumedResource)
			{
				ScrollIconHandler tmp = Instantiate(_scrollIconHandler, _consumedContainer) as ScrollIconHandler;
				tmp.Init(DatabaseManager.Instance.ExtractResource(pair.Key).ResourceSprite, pair.Value);
			}

			foreach (KeyValuePair<string, int> pair in inv.ConsumedWeapons)
			{
				ScrollIconHandler tmp = Instantiate(_scrollIconHandler, _consumedContainer) as ScrollIconHandler;
				tmp.Init(DatabaseManager.Instance.ExtractWeapon(pair.Key).WeaponSprite, pair.Value);
			}

			foreach (KeyValuePair<string, int> pair in inv.ProducedResource)
			{
				ScrollIconHandler tmp = Instantiate(_scrollIconHandler, _producedContainer) as ScrollIconHandler;
				tmp.Init(DatabaseManager.Instance.ExtractResource(pair.Key).ResourceSprite, pair.Value);
			}

			foreach (KeyValuePair<string, int> pair in inv.ProducedWeapons)
			{
				ScrollIconHandler tmp = Instantiate(_scrollIconHandler, _producedContainer) as ScrollIconHandler;
				tmp.Init(DatabaseManager.Instance.ExtractWeapon(pair.Key).WeaponSprite, pair.Value);
			}

			_shouldDisplay = true;
		}

		private void LateUpdate()
		{
			/*if (_shouldNowDisplay == true)
			{
				_container.SetActive(true);
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_container.transform);
				_shouldNowDisplay = false;
			}
			if (_container.activeSelf == false && _shouldDisplay == true)
			{
				_container.SetActive(false);
				_shouldNowDisplay = true;
			}*/
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}

		public void ShowRewardedAd()
		{

			if (JSonManager.Instance.PlayerProfile._noMoreAdsBonus)
			{
				MonetizationManager.Instance.AdFinished += DoubleProduction;
				MonetizationManager.Instance.ShowAd();
			}
			else
			{
				DoubleProduction();
			}
		}

		private void DoubleProduction()
		{
			MonetizationManager.Instance.AdFinished -= DoubleProduction;
			JSonManager.Instance.PlayerProfile.Gold += GameManager.Instance.ProgressionInventory.Gold;
		}
	}
}