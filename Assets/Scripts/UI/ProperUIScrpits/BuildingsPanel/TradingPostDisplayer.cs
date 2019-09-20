﻿namespace DwarfClicker.UI.TradingPost
{
	using DwarfClicker.Core;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
	using Engine.UI.Utils;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class TradingPostDisplayer : MonoBehaviour
	{
		[SerializeField] private Converter _converter = null;
		[SerializeField] private TradingPostController _tradingPostController = null;
		[SerializeField] private ProgressionBarHandler _progressionBar = null;

		[SerializeField] private TextMeshProUGUI _timerText = null;

		[SerializeField] private UpgradeButtonHandler _workerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _sellbyWorkerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _cycleDurationUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _winBeerAmountUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _winBeerChanceUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _goldMultUpgrade = null;
		[SerializeField] private Image _consumedIcon = null;

		private PlayerProfile _playerProfile = null;
		private void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnTPUpgradeChange += OnTradingPostUpgrade;
			_playerProfile.OnFortressChange += UpdateFortress;
			GameLoopManager.Instance.GameLoop += UpdateDisplay;
			OnTradingPostUpgrade();
		}

		private void OnEnable()
		{
			if (_playerProfile == null)
			{
				_playerProfile = JSonManager.Instance.PlayerProfile;
			}
			SoundManager.Instance.PlaySound("TRADINGPOST_AMBIENCE");
			Display();
		}

		private void OnTradingPostUpgrade()
		{
			Display();
		}

		private void Display()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			TradingPostUpgradesData uData = DatabaseManager.Instance.TradingPostUpgrades;

			_consumedIcon.sprite = _tradingPostController.WeaponToSell.WeaponSprite;

			int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WorkerAmount, currentFortress.UTPWorkerNbIndex);
			_workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UTPWorkerNbIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.SellByWorker, currentFortress.UTPSellByWorkerIndex);
			_sellbyWorkerUpgrade.Init(uData.SellByWorker.name, uData.SellByWorker.desc, currentFortress.UTPSellByWorkerIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.CycleDuration, currentFortress.UTPCycleDurationIndex);
			_cycleDurationUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UTPCycleDurationIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerAmount, currentFortress.UTPWinBeerAmountIndex);
			_winBeerAmountUpgrade.Init(uData.WinBeerAmount.name, uData.WinBeerAmount.desc, currentFortress.UTPWinBeerAmountIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerChance, currentFortress.UTPWinBeerChanceIndex);
			_winBeerChanceUpgrade.Init(uData.WinBeerChance.name, uData.WinBeerChance.desc, currentFortress.UTPWinBeerChanceIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.GoldMult, currentFortress.UTPGoldMultIndex);
			_goldMultUpgrade.Init(uData.GoldMult.name, uData.GoldMult.desc, currentFortress.UTPGoldMultIndex, price);
		}

		private void OnDestroy()
		{
			_playerProfile.CurrentFortress.OnTPUpgradeChange -= OnTradingPostUpgrade;
			_playerProfile.OnFortressChange -= OnTradingPostUpgrade;
			if (GameLoopManager.Instance)
			{
				GameLoopManager.Instance.GameLoop -= UpdateDisplay;
			}
		}

		private void UpdateFortress()
		{
			_playerProfile.CurrentFortress.OnTPUpgradeChange += OnTradingPostUpgrade;
			OnTradingPostUpgrade();
		}

		private void UpdateDisplay()
		{
			_progressionBar.UpdateTexts(_tradingPostController.WeaponToSellAmount, (int)_playerProfile.Weapons[_tradingPostController.WeaponToSell.Name].Count, _tradingPostController.GoldToProduce );
			float timeLeft = _tradingPostController.TimeLeft;
			if (timeLeft > 0)
				_timerText.text = timeLeft.ToString("0.0");
			else
				_timerText.text = "";

			if (_progressionBar != null)
			{
				_progressionBar.UpdateBar((_tradingPostController.CycleDuration - _tradingPostController.TimeLeft) / _tradingPostController.CycleDuration);
			}
		}
	}
}