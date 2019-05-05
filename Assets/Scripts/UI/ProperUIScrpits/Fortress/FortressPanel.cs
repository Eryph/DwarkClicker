namespace Preprod
{
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class FortressPanel : MonoBehaviour
	{
		#region Fields
		[SerializeField] private Transform _content = null;
		[SerializeField] private Button[] _switch = null;
		[SerializeField] private Button[] _buy = null;
		[SerializeField] private Button _redeemButton = null;

		private PlayerProfile _profile = null;
		#endregion Fields

		private void Start()
		{
			if (_profile == null)
			{
				_profile = JSonManager.Instance.PlayerProfile;
			}
		}

		private void OnEnable()
		{
			if (_profile == null)
			{
				_profile = JSonManager.Instance.PlayerProfile;
			}
			DisplayPanel();
		}

		public void DisplayPanel()
		{
			int index = 0;
			bool multipleFortress = false;
			for (int i = 0; i < DatabaseManager.Instance.Fortress.Length; i++)
			{
				if (_profile.Fortress[i]._isBought)
				{
					if (i > 1)
						multipleFortress = true;
					_switch[i].gameObject.SetActive(true);
					_switch[i].interactable = true;
					_buy[i].interactable = false;
				}
				else
				{
					_switch[i].interactable = false;
					_buy[i].interactable = true;
				}

				Image _buttonBackground = _switch[i].GetComponent<Image>();
				if (_buttonBackground != null)
				{
					if (i == _profile.CurrentFortressIndex)
					{
						_buttonBackground.color = Color.green;
					}
					else
					{
						_buttonBackground.color = Color.white;
					}
				}

					Text switchText = _switch[i].GetComponentInChildren<Text>();
				if (switchText != null)
				{
					switchText.text = _profile.Fortress[i].Name;
				}

				Text buyText = _buy[i].GetComponentInChildren<Text>();
				if (buyText != null)
				{
					buyText.text = "Buy : " + DatabaseManager.Instance.Fortress[i].Price;
				}
				index = i + 1;
			}

			for (int i = index; i < _content.childCount; i++)
			{
				_content.GetChild(i).gameObject.SetActive(false);
			}

			if (multipleFortress == true)
			{
				_redeemButton.interactable = false;
			}
		}

		public void SwitchFortress(int index)
		{
			_profile.CurrentFortressIndex = index;
			DisplayPanel();
		}

		public void BuyFortress(int index)
		{
			GameManager.Instance.BuyFortress(index);
			DisplayPanel();
		}

		public void RedeemProduction()
		{
			GameManager.Instance.LoadProgression(true);
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}