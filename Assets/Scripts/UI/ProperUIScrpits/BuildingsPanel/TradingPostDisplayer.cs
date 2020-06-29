namespace DwarfClicker.UI.TradingPost
{
	using DwarfClicker.Core;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
	using Engine.UI.Utils;
	using Engine.Utils;
    using System;
    using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class TradingPostDisplayer : MonoBehaviour
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

        [SerializeField] private Image _backgroundImage = null;
        [SerializeField] private Sprite[] _backgrounds = null;

        private PlayerProfile _playerProfile = null;
        private bool _isGoldTrans = true;

        public void Init()
        {
            _playerProfile = JSonManager.Instance.PlayerProfile;
            _playerProfile.CurrentFortress.OnTPUpgradeChange += OnTradingPostUpgrade;
            _playerProfile.OnFortressChange += UpdateFortress;
            GameLoopManager.Instance.GameLoop += UpdateDisplay;
            _onSwitchGoldMithril += Display;
            OnTradingPostUpgrade();
            _backgroundImage.sprite = _backgrounds[_playerProfile.CurrentFortressIndex];
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

        public void SwitchGoldMithril()
        {
            if (_onSwitchGoldMithril != null)
            {
                _isGoldTrans = !_isGoldTrans;
                _tradingPostController.IsGoldTrans = _isGoldTrans;
                _onSwitchGoldMithril();
            }
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

            if (_isGoldTrans)
            {
                bool isMax = currentFortress.UForgeCycleDurationIndex >= uData.CycleDuration.max;
                ulong price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WorkerAmount, currentFortress.UTPWorkerNbIndex);
                _workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UTPWorkerNbIndex, price, _playerProfile.Gold);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.GoldBySell, currentFortress.UTPSellByWorkerIndex);
                _sellbyWorkerUpgrade.Init(uData.GoldBySell.name, uData.GoldBySell.desc, currentFortress.UTPSellByWorkerIndex, price, _playerProfile.Gold);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.CycleDuration, currentFortress.UTPCycleDurationIndex);
                _cycleDurationUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UTPCycleDurationIndex, price, _playerProfile.Gold, isMax);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerAmount, currentFortress.UTPWinBeerAmountIndex);
                _winBeerAmountUpgrade.Init(uData.WinBeerAmount.name, uData.WinBeerAmount.desc, currentFortress.UTPWinBeerAmountIndex, price, _playerProfile.Gold);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerChance, currentFortress.UTPWinBeerChanceIndex);
                isMax = currentFortress.UForgeInstantSellingChanceIndex >= uData.WinBeerChance.max;
                _winBeerChanceUpgrade.Init(uData.WinBeerChance.name, uData.WinBeerChance.desc, currentFortress.UTPWinBeerChanceIndex, price, _playerProfile.Gold, isMax);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.GoldMult, currentFortress.UTPGoldMultIndex);
                _goldMultUpgrade.Init(uData.GoldMult.name, uData.GoldMult.desc, currentFortress.UTPGoldMultIndex, price, _playerProfile.Gold);

                _goldMithrilSwitchButtonImage.sprite = DatabaseManager.Instance.MithrilButtonIcon;
            }
            else
            {
                bool isDecremental = currentFortress.UForgeInstantSellingChanceIndex >= uData.WinBeerChance.max;
                int price = DatabaseManager.Instance.UpgradeMithrilPrice;
                _workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UTPWorkerNbIndex, price, _playerProfile.Mithril);
                _sellbyWorkerUpgrade.Init(uData.GoldBySell.name, uData.GoldBySell.desc, currentFortress.UTPSellByWorkerIndex, price, _playerProfile.Mithril);
                _cycleDurationUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UTPCycleDurationIndex, price, _playerProfile.Mithril);
                _winBeerAmountUpgrade.Init(uData.WinBeerAmount.name, uData.WinBeerAmount.desc, currentFortress.UTPWinBeerAmountIndex, price, _playerProfile.Mithril);
                _winBeerChanceUpgrade.Init(uData.WinBeerChance.name, uData.WinBeerChance.desc, currentFortress.UTPWinBeerChanceIndex, price, _playerProfile.Mithril, isDecremental);
                _goldMultUpgrade.Init(uData.GoldMult.name, uData.GoldMult.desc, currentFortress.UTPGoldMultIndex, price, _playerProfile.Mithril);

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

            _backgroundImage.sprite = _backgrounds[_playerProfile.CurrentFortressIndex];
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