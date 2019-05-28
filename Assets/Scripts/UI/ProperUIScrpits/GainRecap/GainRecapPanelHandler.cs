namespace DwarfClicker.UI.GainRecap
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using Engine.Manager;
	using System;

	public class GainRecapPanelHandler : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _timeElapsedText = null;
		[SerializeField] private TextMeshProUGUI _producedText = null;
		[SerializeField] private TextMeshProUGUI _consumedText = null;
		[SerializeField] private TextMeshProUGUI _goldText = null;
		[SerializeField] private TextMeshProUGUI _mithrilText = null;
		[SerializeField] private int _timePassedActivationMinutes = 2;

		private void OnEnable()
		{
			if (JSonManager.Instance.PlayerProfile.LaunchAmount > 0)
				Display();
			else
			{
				gameObject.SetActive(false);
			}
		}

		private void Display()
		{
			ProgressionLoadInventory inv = GameManager.Instance.ProgressionInventory;

			if (inv.HasChanges == false || inv.TimePassed < new TimeSpan(0, _timePassedActivationMinutes, 0))
			{
				QuitPanel();
				return;
			}

			int i = 0;
			if (inv.ConsumedResource.Count == 0 && inv.ConsumedWeapons.Count == 0)
			{
				_consumedText.text = "Nothing.";
			}
			else
			{
				foreach (KeyValuePair<string, int> pair in inv.ConsumedResource)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = _consumedText;
					}
					else
					{
						tmpText = Instantiate(_consumedText, _consumedText.transform.parent);
					}
					tmpText.text = "- " + pair.Key + " : " + pair.Value.ToString();
					i++;
				}

				foreach (KeyValuePair<string, int> pair in inv.ConsumedWeapons)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = _consumedText;
					}
					else
					{
						tmpText = Instantiate(_consumedText, _consumedText.transform.parent);
					}
					tmpText.text = "- " + pair.Key + " : " + pair.Value.ToString();
					i++;
				}
			}

			if (inv.ProducedResource.Count == 0 && inv.ProducedWeapons.Count == 0)
			{
				_producedText.text = "Nothing.";
			}
			else
			{
				i = 0;
				foreach (KeyValuePair<string, int> pair in inv.ProducedWeapons)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = _producedText;
					}
					else
					{
						tmpText = Instantiate(_producedText, _producedText.transform.parent);
					}
					tmpText.text = "- " + pair.Key + " : " + pair.Value.ToString();
					i++;
				}

				foreach (KeyValuePair<string, int> pair in inv.ProducedResource)
				{
					TextMeshProUGUI tmpText = null;
					if (i == 0)
					{
						tmpText = _producedText;
					}
					else
					{
						tmpText = Instantiate(_producedText, _producedText.transform.parent);
					}
					tmpText.text = "- " + pair.Key + " : " + pair.Value.ToString();
					i++;
				}
			}

			_goldText.text = "Gold : " + inv.Gold;
			_mithrilText.text = "Mithril : " + inv.Mithril;
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}