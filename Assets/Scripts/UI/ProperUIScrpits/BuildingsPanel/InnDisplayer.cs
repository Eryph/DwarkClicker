namespace Preprod
{
	using DwarfClicker.Core;
	using DwarfClicker.Core.Data;
	using DwarfClicker.Misc;
	using DwarfClicker.UI.TradingPost;
	using Engine.Manager;
	using Engine.Utils;
    using System;
    using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
    using UnityEngine.UI;

    public class InnDisplayer : MonoBehaviour
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
        [SerializeField] private SoundController _soundController = null;
        [SerializeField] private InnController _innController = null;

        [SerializeField] private UpgradeButtonHandler _beerByTapUpgrade = null;
        [SerializeField] private UpgradeButtonHandler _storageUpgrade = null;
        [SerializeField] private Transform _bar = null;

        [SerializeField] private Image _sceneAnim = null;
        [SerializeField] private Sprite[] _sceneSprites = null;

        private int _sceneIndex = 1;

        private PlayerProfile _playerProfile = null;

        private Vector3 _emptyPos = Vector3.zero;

        private Vector3 _fullPos = Vector3.zero;

        private bool _isGoldTrans = true;

        private void OnEnable()
        {
            SoundManager.Instance.PlaySound("INN_AMBIENCE");
            if (_playerProfile != null)
                UpdateBar();
        }

        private void Start()
        {
            _emptyPos = _bar.localPosition;
            _playerProfile = JSonManager.Instance.PlayerProfile;
            _playerProfile.CurrentFortress.OnInnUpgradeChange += OnInnUpgrade;
            _playerProfile.OnFortressChange += UpdateFortress;
            _playerProfile.CurrentFortress.OnBeerChange += UpdateBar;
            _onSwitchGoldMithril += OnInnUpgrade;
            UpdateBar();
            OnInnUpgrade();
        }

        private void UpdateBar()
        {
            float t = _playerProfile.CurrentFortress.Beer / _playerProfile.CurrentFortress.BeerStorage;
            _bar.localPosition = Vector3.Lerp(_emptyPos, _fullPos, t);
        }

        public void SwitchGoldMithril()
        {
            if (_onSwitchGoldMithril != null)
            {
                _isGoldTrans = !_isGoldTrans;
                _innController.IsGoldTrans = _isGoldTrans;
                _onSwitchGoldMithril();
            }
        }

        private void OnInnUpgrade()
        {
            FortressProfile currentFortress = _playerProfile.CurrentFortress;
            InnUpgradesData uData = DatabaseManager.Instance.InnUpgrades;

            if (_isGoldTrans)
            {
                int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.BeerByTap, currentFortress.InnBeerByTapIndex);
                _beerByTapUpgrade.Init(uData.BeerByTap.name, uData.BeerByTap.desc, currentFortress.InnBeerByTapIndex, price, _playerProfile.Gold);

                price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.Storage, currentFortress.InnStorageIndex);
                _storageUpgrade.Init(uData.Storage.name, uData.Storage.desc, currentFortress.InnStorageIndex, price, _playerProfile.Gold);

                _goldMithrilSwitchButtonImage.sprite = DatabaseManager.Instance.MithrilButtonIcon;
            }
            else
            {
                int price = DatabaseManager.Instance.UpgradeMithrilPrice;
                _beerByTapUpgrade.Init(uData.BeerByTap.name, uData.BeerByTap.desc, currentFortress.InnBeerByTapIndex, price, _playerProfile.Mithril);
                _storageUpgrade.Init(uData.Storage.name, uData.Storage.desc, currentFortress.InnStorageIndex, price, _playerProfile.Mithril);

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

        private void UpdateFortress()
        {
            _playerProfile.CurrentFortress.OnInnUpgradeChange += OnInnUpgrade;
            _playerProfile.CurrentFortress.OnBeerChange += UpdateBar;
            OnInnUpgrade();
        }

        private void OnDestroy()
        {
            _playerProfile.CurrentFortress.OnInnUpgradeChange -= OnInnUpgrade;
            _playerProfile.OnFortressChange -= OnInnUpgrade;
            if (GameLoopManager.Instance)
            {
                _playerProfile.CurrentFortress.OnInnUpgradeChange -= OnInnUpgrade;
            }
        }

        public void PlayBeerSound()
        {
            if (_playerProfile.CurrentFortress.Beer >= _playerProfile.CurrentFortress.BeerStorage)
            {
                SoundManager.Instance.PlaySound("ERROR_CLICK");
            }
            else
            {
                _soundController.PlayBeerSound();
            }
        }

        public void AnimScene()
        {
            _sceneAnim.sprite = _sceneSprites[_sceneIndex];
            _sceneIndex++;
            if (_sceneIndex >= _sceneSprites.Length)
                _sceneIndex = 0;
        }
    }
}