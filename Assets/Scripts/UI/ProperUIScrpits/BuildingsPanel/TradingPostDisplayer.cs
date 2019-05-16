namespace DwarfClicker.UI.TradingPost
{
	using DwarfClicker.Core;
	using Engine.Manager;
	using Engine.UI.Utils;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

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
			Display();
		}

		private void OnTradingPostUpgrade()
		{
			Display();
		}

		private void Display()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;

			int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WorkerAmount, currentFortress.UTPWorkerNbIndex);
			_workerUpgrade.Init("Worker +", currentFortress.UTPWorkerNbIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.SellByWorker, currentFortress.UTPSellByWorkerIndex);
			_sellbyWorkerUpgrade.Init("Sell by Worker", currentFortress.UTPSellByWorkerIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.CycleDuration, currentFortress.UTPCycleDurationIndex);
			_cycleDurationUpgrade.Init("Cycle Duration", currentFortress.UTPCycleDurationIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerAmount, currentFortress.UTPWinBeerAmountIndex);
			_winBeerAmountUpgrade.Init("Win Beer Amount", currentFortress.UTPWinBeerAmountIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerChance, currentFortress.UTPWinBeerChanceIndex);
			_winBeerChanceUpgrade.Init("Win Beer Chance", currentFortress.UTPWinBeerChanceIndex, price);

			price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.GoldMult, currentFortress.UTPGoldMultIndex);
			_goldMultUpgrade.Init("Gold Mult", currentFortress.UTPGoldMultIndex, price);
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
			float timeLeft = _tradingPostController.TimeLeft;
			if (timeLeft > 0)
				_timerText.text = timeLeft.ToString("0.0");
			else
				_timerText.text = "0";

			if (_progressionBar != null)
			{
				_progressionBar.UpdateBar((_tradingPostController.CycleDuration - _tradingPostController.TimeLeft) / _tradingPostController.CycleDuration);
			}
		}
	}
}