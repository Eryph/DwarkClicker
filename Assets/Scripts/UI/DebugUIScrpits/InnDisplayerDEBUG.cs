namespace Preprod
{
	using DwarfClicker.Core;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

	public class InnDisplayerDEBUG : MonoBehaviour
	{
		[SerializeField] private Converter _converter = null;
		[SerializeField] private TextMeshProUGUI _beerByTapText = null;
		[SerializeField] private TextMeshProUGUI _storageText = null;

		private PlayerProfile _playerProfile = null;
		private void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnInnUpgradeChange += OnInnUpgrade;
			_playerProfile.OnFortressChange += UpdateFortress;
			OnInnUpgrade();
		}

		private void OnInnUpgrade()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			_beerByTapText.text = string.Format("R{0} - {1}g", currentFortress.InnBeerByTapIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.BeerByTap, currentFortress.InnBeerByTapIndex));
			_storageText.text = string.Format("R{0} - {1}g", currentFortress.InnStorageIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.Storage, currentFortress.InnStorageIndex));
		}

		private void UpdateFortress()
		{
			_playerProfile.CurrentFortress.OnTPUpgradeChange += OnInnUpgrade;
			OnInnUpgrade();
		}

		private void OnDestroy()
		{
			_playerProfile.CurrentFortress.OnInnUpgradeChange -= OnInnUpgrade;
			_playerProfile.OnFortressChange -= OnInnUpgrade;
			if (GameLoopManager.Instance)
			{
				_playerProfile.CurrentFortress.OnInnUpgradeChange -= OnInnUpgrade;
			}
		}
	}
}