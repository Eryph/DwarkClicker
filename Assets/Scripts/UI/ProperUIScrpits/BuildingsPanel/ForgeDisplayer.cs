﻿namespace Preprod
{
	using DwarfClicker.Core;
	using DwarfClicker.Core.Data;
	using DwarfClicker.UI.TradingPost;
	using Engine.Manager;
	using Engine.UI.Utils;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

	public class ForgeDisplayer : MonoBehaviour
	{
		[SerializeField] private Converter _converter = null;
		[SerializeField] private ForgeController _forgeController = null;
		[SerializeField] private ProgressionBarHandler _progressionBar = null;

		[SerializeField] private TextMeshProUGUI _timerText = null;

		[SerializeField] private UpgradeButtonHandler _workerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _wByWorkerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _cycleDurationUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _instantSellingChanceUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _instantSellingGoldBonusUpgrade = null;

		private PlayerProfile _playerProfile = null;
		private void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnForgeUpgradeChange += OnForgeUpgrade;
			_playerProfile.OnFortressChange += UpdateFortress;
			GameLoopManager.Instance.GameLoop += UpdateDisplay;
			OnForgeUpgrade();
		}

		private void OnForgeUpgrade()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			ForgeUpgradesData uData = DatabaseManager.Instance.ForgeUpgrades;

			int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WorkerAmount, currentFortress.UForgeWorkerNbIndex);
			_workerUpgrade.Init(uData.WorkerAmount.name, currentFortress.UForgeWorkerNbIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WByWorker, currentFortress.UForgeWByWorkerIndex);
			_wByWorkerUpgrade.Init(uData.WByWorker.name, currentFortress.UForgeWByWorkerIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.CycleDuration, currentFortress.UForgeCycleDurationIndex);
			_cycleDurationUpgrade.Init(uData.CycleDuration.name, currentFortress.UForgeCycleDurationIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingChance, currentFortress.UForgeInstantSellingChanceIndex);
			_instantSellingChanceUpgrade.Init(uData.InstantSellingChance.name, currentFortress.UForgeInstantSellingChanceIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingGoldBonus, currentFortress.UForgeInstantSellingGoldBonusIndex);
			_instantSellingGoldBonusUpgrade.Init(uData.InstantSellingGoldBonus.name, currentFortress.UForgeInstantSellingGoldBonusIndex, price);
		}

		private void OnDestroy()
		{
			_playerProfile.CurrentFortress.OnForgeUpgradeChange -= OnForgeUpgrade;
			_playerProfile.OnFortressChange -= OnForgeUpgrade;
			if (GameLoopManager.Instance)
			{
				GameLoopManager.Instance.GameLoop -= UpdateDisplay;
			}
		}

		private void UpdateFortress()
		{
			_playerProfile.CurrentFortress.OnForgeUpgradeChange += OnForgeUpgrade;
			OnForgeUpgrade();
		}

		private void UpdateDisplay()
		{
			_progressionBar.UpdateTexts(_forgeController.ResourceConsumed, _playerProfile.Resources[_forgeController.CurrentForgingWeapon.Recipe[0].Key].Count, _forgeController.ForgeCount);

			float timeLeft = _forgeController.TimeLeft;
			
			if (timeLeft > 0)
			{
				_timerText.text = timeLeft.ToString("0.0");
			}
			else
				_timerText.text = "";

			if (_progressionBar != null)
			{
				_progressionBar.UpdateBar((_forgeController.CycleDuration - _forgeController.TimeLeft) / _forgeController.CycleDuration);
			}
		}

	}
}