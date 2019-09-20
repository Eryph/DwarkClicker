﻿namespace Preprod
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using DwarfClicker.Core;
	using Engine.Manager;
	using TMPro;
	using Engine.Utils;
	using Engine.UI.Utils;
	using DwarfClicker.UI.TradingPost;
	using DwarfClicker.Core.Data;
	using UnityEngine.UI;

	public class MineDisplayer : MonoBehaviour {

		[SerializeField] private Converter _converter = null;
		[SerializeField] private MineController _mineController = null;
		[SerializeField] private ProgressionBarHandler _progressionBar = null;

		[SerializeField] private TextMeshProUGUI _timerText = null;


		[SerializeField] private UpgradeButtonHandler _workerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _resByWorkerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _cycleUpUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _beerConsoUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _mithrilChanceUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _luckUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _richVeinUpgrade = null;

		[SerializeField] private Image _producedImage = null;

		private PlayerProfile _playerProfile = null;

		private void OnEnable()
		{
			SoundManager.Instance.PlaySound("MINE_AMBIENCE");
		}

		private void OnMineUpgrade()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			MineUpgradesData uData = DatabaseManager.Instance.MineUpgrades;

			_producedImage.sprite = currentFortress.ResourceProduced.ResourceSprite;

			int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.WorkerAmount, currentFortress.UMineWorkerNbIndex);
			_workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UMineWorkerNbIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.ResByWorker, currentFortress.UMineResByWorkerIndex);
			_resByWorkerUpgrade.Init(uData.ResByWorker.name, uData.ResByWorker.desc, currentFortress.UMineResByWorkerIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.CycleDuration, currentFortress.UMineCycleDurationIndex);
			_cycleUpUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UMineCycleDurationIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.BeerConsumption, currentFortress.UMineBeerConsoIndex);
			_beerConsoUpgrade.Init(uData.BeerConsumption.name, uData.BeerConsumption.desc, currentFortress.UMineBeerConsoIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Mithril, currentFortress.UMineMithrilChanceIndex);
			_mithrilChanceUpgrade.Init(uData.Mithril.name, uData.Mithril.desc, currentFortress.UMineMithrilChanceIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Luck, currentFortress.UMineLuckIndex);
			_luckUpgrade.Init(uData.Luck.name, uData.Luck.desc, currentFortress.UMineLuckIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.RichVein, currentFortress.UMineRichVeinIndex);
			_richVeinUpgrade.Init(uData.RichVein.name, uData.RichVein.desc, currentFortress.UMineRichVeinIndex, price);
		}
		
		private void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnMineUpgradeChange += OnMineUpgrade;
			_playerProfile.OnFortressChange += UpdateFortress;
			GameLoopManager.Instance.GameLoop += UpdateDisplay;
			OnMineUpgrade();
		}

		private void OnDestroy()
		{
			_playerProfile.OnFortressChange -= OnMineUpgrade;
			_playerProfile.CurrentFortress.OnMineUpgradeChange -= OnMineUpgrade;
			if (GameLoopManager.Instance)
			{
				GameLoopManager.Instance.GameLoop -= UpdateDisplay;
			}
		}

		private void UpdateFortress()
		{
			_playerProfile.CurrentFortress.OnMineUpgradeChange += OnMineUpgrade;
			OnMineUpgrade();
		}

		private void UpdateDisplay()
		{
			float timeLeft = _mineController.TimeLeft;

			_progressionBar.UpdateTexts((int)_mineController.BeerCost, (int)_playerProfile.CurrentFortress.Beer, _mineController.MiningCount);

			if (timeLeft > 0)
			{
				_timerText.text = timeLeft.ToString("0.0");
			}
			else
				_timerText.text = "";

			if (_progressionBar != null)
			{
				if (_mineController.TimeLeft <= 0)
				{
					_progressionBar.SetEmpty();
				}
				else
				{
					_progressionBar.UpdateBar((_mineController.CycleDuration - _mineController.TimeLeft) / _mineController.CycleDuration);
				}
			}
		}
	}
}