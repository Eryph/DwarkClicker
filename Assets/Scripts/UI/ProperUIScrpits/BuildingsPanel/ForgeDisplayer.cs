namespace Preprod
{
	using DwarfClicker.Core;
	using DwarfClicker.Core.Data;
	using DwarfClicker.UI.TradingPost;
	using Engine.Manager;
	using Engine.UI.Utils;
	using Engine.Utils;
    using System;
    using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class ForgeDisplayer : MonoBehaviour
	{
        #region Events
        private Action _onSwitchGoldMithril = null;

        public event Action OnSwitchGoldMithril
        {
            add
            {
                _onSwitchGoldMithril -= value;
                _onSwitchGoldMithril += value;
            }
            remove
            {
                _onSwitchGoldMithril -= value;
            }
        }
        #endregion Events

        [SerializeField] private Image _goldMithrilSwitchButtonImage = null;
        [SerializeField] private Image[] _goldMithrilSwitchUpgrades = null;

        [SerializeField] private Converter _converter = null;
		[SerializeField] private ForgeController _forgeController = null;
		[SerializeField] private ProgressionBarHandler _progressionBar = null;

		[SerializeField] private TextMeshProUGUI _timerText = null;

		[SerializeField] private UpgradeButtonHandler _workerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _wByWorkerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _cycleDurationUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _instantSellingChanceUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _instantSellingGoldBonusUpgrade = null;
		[SerializeField] private Image _consumedImage = null;
		[SerializeField] private Image _producedImage = null;
        [SerializeField] private Image _backgroundImage = null;
        [SerializeField] private Sprite[] _backgrounds = null;

		private PlayerProfile _playerProfile = null;
        private bool _isGoldTrans = true;

		public void Init()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnForgeUpgradeChange += OnForgeUpgrade;
			_playerProfile.OnFortressChange += UpdateFortress;
            _onSwitchGoldMithril += Display;
			GameLoopManager.Instance.GameLoop += UpdateDisplay;
			OnForgeUpgrade();
		}

		private void OnEnable()
		{
			if (_playerProfile == null)
			{
				_playerProfile = JSonManager.Instance.PlayerProfile;
			}
			Display();
			_forgeController.OnWeaponChange += Display;
			SoundManager.Instance.PlaySound("FORGE_AMBIENCE");
		}

		private void OnDisable()
		{
			_forgeController.OnWeaponChange -= Display;
		}

        public void SwitchGoldMithril()
        {
            if (_onSwitchGoldMithril != null)
            {
                _isGoldTrans = !_isGoldTrans;
                _forgeController.IsGoldTrans = _isGoldTrans;
                _onSwitchGoldMithril();
            }
        }

        private void Display()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			ForgeUpgradesData uData = DatabaseManager.Instance.ForgeUpgrades;

			_consumedImage.sprite = currentFortress.ResourceProduced.ResourceSprite;
			_producedImage.sprite = _forgeController.CurrentForgingWeapon.WeaponSprite;

            if (_isGoldTrans)
            {
                bool isMax;
                int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WorkerAmount, currentFortress.UForgeWorkerNbIndex);
			    _workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UForgeWorkerNbIndex, price, _playerProfile.Gold);

			    price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WByWorker, currentFortress.UForgeWByWorkerIndex);
			    _wByWorkerUpgrade.Init(uData.WByWorker.name, uData.WByWorker.desc, currentFortress.UForgeWByWorkerIndex, price, _playerProfile.Gold);

			    price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.CycleDuration, currentFortress.UForgeCycleDurationIndex);
                isMax = currentFortress.UForgeCycleDurationIndex >= uData.CycleDuration.max;
                _cycleDurationUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UForgeCycleDurationIndex, price, _playerProfile.Gold);

			    price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingChance, currentFortress.UForgeInstantSellingChanceIndex);
			    _instantSellingChanceUpgrade.Init(uData.InstantSellingChance.name, uData.InstantSellingChance.desc, currentFortress.UForgeInstantSellingChanceIndex, price, _playerProfile.Gold);

			    price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingGoldBonus, currentFortress.UForgeInstantSellingGoldBonusIndex);
                isMax = currentFortress.UForgeInstantSellingChanceIndex >= uData.InstantSellingChance.max;
                _instantSellingGoldBonusUpgrade.Init(uData.InstantSellingGoldBonus.name, uData.InstantSellingGoldBonus.desc, currentFortress.UForgeInstantSellingGoldBonusIndex, price, _playerProfile.Gold, isMax);

                _goldMithrilSwitchButtonImage.sprite = DatabaseManager.Instance.MithrilButtonIcon;
            }
            else
            {
                int price = DatabaseManager.Instance.UpgradeMithrilPrice;
                bool isDecremental = currentFortress.UForgeCycleDurationIndex >= uData.CycleDuration.max;
                _workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UForgeWorkerNbIndex, price, _playerProfile.Mithril);
                _wByWorkerUpgrade.Init(uData.WByWorker.name, uData.WByWorker.desc, currentFortress.UForgeWByWorkerIndex, price, _playerProfile.Mithril);
                _cycleDurationUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UForgeCycleDurationIndex, price, _playerProfile.Mithril, isDecremental);
                isDecremental = currentFortress.UForgeInstantSellingChanceIndex >= uData.InstantSellingChance.max;
                _instantSellingChanceUpgrade.Init(uData.InstantSellingChance.name, uData.InstantSellingChance.desc, currentFortress.UForgeInstantSellingChanceIndex, price, _playerProfile.Mithril, isDecremental);
                _instantSellingGoldBonusUpgrade.Init(uData.InstantSellingGoldBonus.name, uData.InstantSellingGoldBonus.desc, currentFortress.UForgeInstantSellingGoldBonusIndex, price, _playerProfile.Mithril);
                _goldMithrilSwitchButtonImage.sprite = DatabaseManager.Instance.GoldButtonIcon;
            }

            for (int i = 0; i < _goldMithrilSwitchUpgrades.Length; i++)
            {
                if (_isGoldTrans)
                {
                    _goldMithrilSwitchUpgrades[i].sprite = DatabaseManager.Instance.GoldButtonIcon;
                }
                else
                {
                    _goldMithrilSwitchUpgrades[i].sprite = DatabaseManager.Instance.MithrilButtonIcon;
                }
            }
        }

		private void OnForgeUpgrade()
		{
			Display();
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
            _backgroundImage.sprite = _backgrounds[_playerProfile.CurrentFortressIndex];
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