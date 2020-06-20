namespace Preprod
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
    using System;

    public class MineDisplayer : MonoBehaviour {

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
		[SerializeField] private MineController _mineController = null;
		[SerializeField] private ProgressionBarHandler _progressionBar = null;

		[SerializeField] private TextMeshProUGUI _timerText = null;


		[SerializeField] private UpgradeButtonHandler _workerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _resByWorkerUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _cycleUpUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _mithrilChanceUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _luckUpgrade = null;
		[SerializeField] private UpgradeButtonHandler _richVeinUpgrade = null;

		[SerializeField] private Image _producedImage = null;

        [SerializeField] private Image _backgroundImage = null;
        [SerializeField] private Sprite[] _backgrounds = null;

        private PlayerProfile _playerProfile = null;
        private bool _isGoldTrans = true;

        private void OnEnable()
		{
			SoundManager.Instance.PlaySound("MINE_AMBIENCE");
		}

		private void OnMineUpgrade()
		{
			FortressProfile currentFortress = _playerProfile.CurrentFortress;
			MineUpgradesData uData = DatabaseManager.Instance.MineUpgrades;

			_producedImage.sprite = currentFortress.ResourceProduced.ResourceSprite;


            if (_isGoldTrans)
            {
                int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.WorkerAmount, currentFortress.UMineWorkerNbIndex);
                _workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UMineWorkerNbIndex, price, _playerProfile.Gold);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.ResByWorker, currentFortress.UMineResByWorkerIndex);
                _resByWorkerUpgrade.Init(uData.ResByWorker.name, uData.ResByWorker.desc, currentFortress.UMineResByWorkerIndex, price, _playerProfile.Gold);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.CycleDuration, currentFortress.UMineCycleDurationIndex);
                bool isMax = currentFortress.UForgeCycleDurationIndex >= uData.CycleDuration.max;
                _cycleUpUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UMineCycleDurationIndex, price, _playerProfile.Gold);

                //price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.BeerConsumption, currentFortress.UMineBeerConsoIndex);
                //_beerConsoUpgrade.Init(uData.BeerConsumption.name, uData.BeerConsumption.desc, currentFortress.UMineBeerConsoIndex, price, _playerProfile.Gold);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Mithril, currentFortress.UMineMithrilChanceIndex);
                isMax = currentFortress.UForgeInstantSellingChanceIndex >= uData.Mithril.max;
                _mithrilChanceUpgrade.Init(uData.Mithril.name, uData.Mithril.desc, currentFortress.UMineMithrilChanceIndex, price, _playerProfile.Gold, isMax);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Luck, currentFortress.UMineLuckIndex);
                isMax = currentFortress.UForgeInstantSellingChanceIndex >= uData.Luck.max;
                _luckUpgrade.Init(uData.Luck.name, uData.Luck.desc, currentFortress.UMineLuckIndex, price, _playerProfile.Gold, isMax);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.RichVein, currentFortress.UMineRichVeinIndex);
                isMax = currentFortress.UForgeInstantSellingChanceIndex >= uData.RichVein.max;
                _richVeinUpgrade.Init(uData.RichVein.name, uData.RichVein.desc, currentFortress.UMineRichVeinIndex, price, _playerProfile.Gold, isMax);

                _goldMithrilSwitchButtonImage.sprite = DatabaseManager.Instance.MithrilButtonIcon;
            }
            else
            {
                int price = DatabaseManager.Instance.UpgradeMithrilPrice;
                _workerUpgrade.Init(uData.WorkerAmount.name, uData.WorkerAmount.desc, currentFortress.UMineWorkerNbIndex, price, _playerProfile.Mithril);
                _resByWorkerUpgrade.Init(uData.ResByWorker.name, uData.ResByWorker.desc, currentFortress.UMineResByWorkerIndex, price, _playerProfile.Mithril);
                _cycleUpUpgrade.Init(uData.CycleDuration.name, uData.CycleDuration.desc, currentFortress.UMineCycleDurationIndex, price, _playerProfile.Mithril);
                _mithrilChanceUpgrade.Init(uData.Mithril.name, uData.Mithril.desc, currentFortress.UMineMithrilChanceIndex, price, _playerProfile.Mithril);
                _luckUpgrade.Init(uData.Luck.name, uData.Luck.desc, currentFortress.UMineLuckIndex, price, _playerProfile.Mithril);
                _richVeinUpgrade.Init(uData.RichVein.name, uData.RichVein.desc, currentFortress.UMineRichVeinIndex, price, _playerProfile.Mithril);
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


        public void Init()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnMineUpgradeChange += OnMineUpgrade;
			_playerProfile.OnFortressChange += UpdateFortress;
            _onSwitchGoldMithril += OnMineUpgrade;
			GameLoopManager.Instance.GameLoop += UpdateDisplay;
			OnMineUpgrade();
            _backgroundImage.sprite = _backgrounds[_playerProfile.CurrentFortressIndex];
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

        public void SwitchGoldMithril()
        {
            if (_onSwitchGoldMithril != null)
            {
                _isGoldTrans = !_isGoldTrans;
                _mineController.IsGoldTrans = _isGoldTrans;
                _onSwitchGoldMithril();
            }
        }

        private void UpdateFortress()
		{
			_playerProfile.CurrentFortress.OnMineUpgradeChange += OnMineUpgrade;
			OnMineUpgrade();

            _backgroundImage.sprite = _backgrounds[_playerProfile.CurrentFortressIndex];
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