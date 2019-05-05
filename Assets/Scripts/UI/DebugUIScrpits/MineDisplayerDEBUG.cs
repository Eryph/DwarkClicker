namespace Preprod
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Core;
	using Engine.Manager;
	using TMPro;
	using Engine.Utils;
	using Engine.UI.Utils;

	public class MineDisplayerDEBUG : MonoBehaviour {

		[SerializeField] private Converter _converter = null;
		[SerializeField] private MineController _mineController = null;

		[SerializeField] private TextMeshProUGUI _timerText = null;

		[SerializeField] private TextMeshProUGUI _WorkUpText = null;
		[SerializeField] private TextMeshProUGUI _CycleUpText = null;
		[SerializeField] private TextMeshProUGUI _ResByText = null;
		[SerializeField] private TextMeshProUGUI _beerConsoText = null;
		[SerializeField] private TextMeshProUGUI _mithrilChanceText = null;
		[SerializeField] private TextMeshProUGUI _luckText = null;
		[SerializeField] private TextMeshProUGUI _richVeinText = null;

		private PlayerProfile _playerProfile = null;


		private void OnMineUpgrade()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			_WorkUpText.text = string.Format("R{0} - {1}g", currentFortress.UMineWorkerNbIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.WorkerNb, currentFortress.UMineWorkerNbIndex));
			_CycleUpText.text = string.Format("R{0} - {1}g", currentFortress.UMineCycleDurationIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.CycleDuration, currentFortress.UMineCycleDurationIndex));
			_ResByText.text = string.Format("R{0} - {1}g", currentFortress.UMineResByWorkerIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.ResByWorker, currentFortress.UMineResByWorkerIndex));
			_beerConsoText.text = string.Format("R{0} - {1:0.#}g", currentFortress.UMineBeerConsoIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.BeerConsumption, currentFortress.UMineBeerConsoIndex));
			_mithrilChanceText.text = string.Format("R{0} - {1}g", currentFortress.UMineMithrilChanceIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Mithril, currentFortress.UMineMithrilChanceIndex));
			_luckText.text = string.Format("R{0} - {1}g", currentFortress.UMineLuckIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Luck, currentFortress.UMineLuckIndex));
			_richVeinText.text = string.Format("R{0} - {1}g", currentFortress.UMineRichVeinIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.RichVein, currentFortress.UMineRichVeinIndex));
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
			if (timeLeft > 0)
				_timerText.text = timeLeft.ToString("0.0");
			else
				_timerText.text = "0";
		}
	}
}