namespace Preprod
{
	using DwarfClicker.Core;
	using DwarfClicker.Core.Data;
	using DwarfClicker.UI.TradingPost;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

	public class InnDisplayer : MonoBehaviour
	{
		[SerializeField] private Converter _converter = null;

		[SerializeField] private UpgradeButtonHandler _beerByTapUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _storageUpgrade = null;

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
			InnUpgradesData uData = DatabaseManager.Instance.InnUpgrades;

			int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.BeerByTap, currentFortress.InnBeerByTapIndex);
			_beerByTapUpgrade.Init(uData.BeerByTap.name, currentFortress.InnBeerByTapIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.Storage, currentFortress.InnStorageIndex);
			_storageUpgrade.Init(uData.Storage.name, currentFortress.InnStorageIndex, price);
		}

		private void UpdateFortress()
		{
			_playerProfile.CurrentFortress.OnInnUpgradeChange += OnInnUpgrade;
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