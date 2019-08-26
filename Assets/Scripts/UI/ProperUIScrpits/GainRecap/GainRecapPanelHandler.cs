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
		[SerializeField] private Button _globalQuitButton = null;
		[SerializeField] private GameObject _container = null;
		[SerializeField] private Transform _timeElapsedContainer = null;
		[SerializeField] private Transform _producedContainer = null;
		[SerializeField] private Transform _consumedContainer = null;
		[SerializeField] private Transform _goldContainer = null;
		[SerializeField] private Transform _mithrilContainer = null;
		[SerializeField] private int _timePassedActivationMinutes = 2;

		private bool _shouldDisplay = false;
		private bool _shouldNowDisplay = false; 

		private void OnEnable()
		{
			_globalQuitButton.gameObject.SetActive(true);
			if (JSonManager.Instance.PlayerProfile.LaunchAmount > 0)
				Display();
			else
			{
				gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			_globalQuitButton.gameObject.SetActive(false);
		}

		private void Display()
		{
			
			ProgressionLoadInventory inv = GameManager.Instance.ProgressionInventory;
			TextMeshProUGUI timeElapsedText = _timeElapsedContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI producedText = _producedContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI consumedText = _consumedContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI goldText = _goldContainer.GetComponentInChildren<TextMeshProUGUI>();
			TextMeshProUGUI mithrilText = _mithrilContainer.GetComponentInChildren<TextMeshProUGUI>();


			if (inv.HasChanges == false || inv.TimePassed < new TimeSpan(0, _timePassedActivationMinutes, 0))
			{
				QuitPanel();
				return;
			}

			
			timeElapsedText.text = UIHelper.FormatTimeSpanToString(inv.TimePassed);

			int i = 0;
			if (inv.ConsumedResource.Count == 0 && inv.ConsumedWeapons.Count == 0)
			{
				consumedText.text = "Nothing.";
			}
			else
			{
				foreach (KeyValuePair<string, int> pair in inv.ConsumedResource)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = consumedText;
					}
					else
					{
						Transform tmp = Instantiate(_consumedContainer, _consumedContainer.transform.parent);
						tmpText = tmp.GetComponentInChildren<TextMeshProUGUI>();
					}
					tmpText.text = pair.Value.ToString() + " " + pair.Key;
					i++;
				}

				foreach (KeyValuePair<string, int> pair in inv.ConsumedWeapons)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = consumedText;
					}
					else
					{
						Transform tmp = Instantiate(_consumedContainer, _consumedContainer.transform.parent);
						tmpText = tmp.GetComponentInChildren<TextMeshProUGUI>();
					}
					tmpText.text = pair.Value.ToString() + " " + pair.Key;
					i++;
				}
			}

			if (inv.ProducedResource.Count == 0 && inv.ProducedWeapons.Count == 0)
			{
				producedText.text = "Nothing.";
			}
			else
			{
				i = 0;
				foreach (KeyValuePair<string, int> pair in inv.ProducedWeapons)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = producedText;
					}
					else
					{
						Transform tmp = Instantiate(_producedContainer, _producedContainer.transform.parent);
						tmpText = tmp.GetComponentInChildren<TextMeshProUGUI>();
					}
					tmpText.text = "+" + pair.Value.ToString() + " " + pair.Key;
					i++;
				}

				foreach (KeyValuePair<string, int> pair in inv.ProducedResource)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = producedText;
					}
					else
					{
						Transform tmp = Instantiate(_producedContainer, _producedContainer.transform.parent);
						tmpText = tmp.GetComponentInChildren<TextMeshProUGUI>();
					}
					tmpText.text = "+" + pair.Value.ToString() + " " + pair.Key;
					i++;
				}
			}

			goldText.text = "+" + inv.Gold;
			mithrilText.text = "+" + inv.Mithril;
			_container.gameObject.SetActive(false);
			_shouldDisplay = true;
		}

		private void LateUpdate()
		{
			if (_shouldNowDisplay == true)
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
			}
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