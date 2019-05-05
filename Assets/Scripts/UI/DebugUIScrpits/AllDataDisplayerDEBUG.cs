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

	public class AllDataDisplayerDEBUG : MonoBehaviour
	{
		#region Fields
		[SerializeField] private TextMeshProUGUI _goldDisplay = null;
		[SerializeField] private TextMeshProUGUI _beerDisplay = null;
		[SerializeField] private TextMeshProUGUI _mithrilDisplay = null;
		[SerializeField] private GameObject _fortressPanel = null;
		[SerializeField] private Text _fortressText = null;

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
			string displayText = UIHelper.FormatIntegerString(_playerProfile.CurrentFortress.Beer);
			_beerDisplay.text = displayText;
		}

		private void UpdateMithrilDisplay()
		{
			string displayText = UIHelper.FormatIntegerString(_playerProfile.Mithril);
			_mithrilDisplay.text = displayText;
		}

		private void UpdateFortress()
		{
			_fortressText.text = _playerProfile.CurrentFortress.Name;
			_playerProfile.CurrentFortress.OnBeerChange += UpdateBeerDisplay;
			UpdateBeerDisplay();
		}

		public void OpenFortressPanel()
		{
			_fortressPanel.SetActive(true);
		}
		#endregion Methods
	}
}