namespace Preprod
{
	using Core;
	using Engine.Manager;
	using Engine.UI.Utils;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

	public class TradingPostDisplayerDEBUG : MonoBehaviour
	{
		[SerializeField] private Converter _converter = null;
		[SerializeField] private TradingPostController _tradingPostController = null;

		[SerializeField] private TextMeshProUGUI _timerText = null;

		[SerializeField] private TextMeshProUGUI _WorkUpText = null;
		[SerializeField] private TextMeshProUGUI _CycleUpText = null;
		[SerializeField] private TextMeshProUGUI _sellByWorkerUpText = null;
		[SerializeField] private TextMeshProUGUI _goldMultUpText = null;
		[SerializeField] private TextMeshProUGUI _winBeerChanceUpText = null;
		[SerializeField] private TextMeshProUGUI _winBeerAmountUpText = null;

		private PlayerProfile _playerProfile = null;
		private void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnTPUpgradeChange += OnTradingPostUpgrade;
			_playerProfile.OnFortressChange += UpdateFortress;
			GameLoopManager.Instance.GameLoop += UpdateDisplay;
			OnTradingPostUpgrade();
		}

		private void OnTradingPostUpgrade()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			_WorkUpText.text = string.Format("R{0} - {1}g", currentFortress.UTPWorkerNbIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WorkerNb, currentFortress.UTPWorkerNbIndex)); ;
			_CycleUpText.text = string.Format("R{0} - {1}g", currentFortress.UTPCycleDurationIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.SellByWorker, currentFortress.UTPSellByWorkerIndex));
			_sellByWorkerUpText.text = string.Format("R{0} - {1}g", currentFortress.UTPSellByWorkerIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.SellByWorker, currentFortress.UTPSellByWorkerIndex));
			_goldMultUpText.text = string.Format("R{0} - {1}g", currentFortress.UTPGoldMultIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.GoldMult, currentFortress.UTPGoldMultIndex));
			_winBeerChanceUpText.text = string.Format("R{0} - {1}g", currentFortress.UTPWinBeerChanceIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerChance, currentFortress.UTPWinBeerChanceIndex));
			_winBeerAmountUpText.text = string.Format("R{0} - {1}g", currentFortress.UTPWinBeerAmountIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerAmount, currentFortress.UTPWinBeerAmountIndex));
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
		}
	}
}