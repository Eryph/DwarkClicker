namespace Preprod
{
	using DwarfClicker.Core;
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

		[SerializeField] private TextMeshProUGUI _WorkUpText = null;
		[SerializeField] private TextMeshProUGUI _CycleUpText = null;
		[SerializeField] private TextMeshProUGUI _wByWorkerUpText = null;
		[SerializeField] private TextMeshProUGUI _instantSellingChanceText = null;
		[SerializeField] private TextMeshProUGUI _instantSellingGoldBonusText = null;

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
			_WorkUpText.text = string.Format("R{0} - {1}g", currentFortress.UForgeWorkerNbIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WorkerAmount, currentFortress.UForgeWorkerNbIndex));
			_CycleUpText.text = string.Format("R{0} - {1}g", currentFortress.UForgeCycleDurationIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.CycleDuration, currentFortress.UForgeCycleDurationIndex));
			_wByWorkerUpText.text = string.Format("R{0} - {1}g", currentFortress.UForgeWByWorkerIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WByWorker, currentFortress.UForgeWByWorkerIndex));
			_instantSellingChanceText.text = string.Format("R{0} - {1}g", currentFortress.UForgeInstantSellingChanceIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingChance, currentFortress.UForgeInstantSellingChanceIndex));
			_instantSellingGoldBonusText.text = string.Format("R{0} - {1}g", currentFortress.UForgeInstantSellingGoldBonusIndex, _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingGoldBonus, currentFortress.UForgeInstantSellingGoldBonusIndex));
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
			float timeLeft = _forgeController.TimeLeft;
			if (timeLeft > 0)
				_timerText.text = timeLeft.ToString("0.0");
			else
				_timerText.text = "0";

			if (_progressionBar != null)
			{
				_progressionBar.UpdateBar((_forgeController.CycleDuration - _forgeController.TimeLeft) / _forgeController.CycleDuration);
			}
		}

	}
}