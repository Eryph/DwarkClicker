namespace Preprod
{
	using DwarfClicker.Core;
	using DwarfClicker.Core.Data;
	using DwarfClicker.Misc;
	using DwarfClicker.UI.TradingPost;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
    using UnityEngine.UI;

    public class InnDisplayer : MonoBehaviour
    {
        [SerializeField] private Converter _converter = null;
        [SerializeField] private SoundController _soundController = null;

        [SerializeField] private UpgradeButtonHandler _beerByTapUpgrade = null;
        [SerializeField] private UpgradeButtonHandler _storageUpgrade = null;
        [SerializeField] private Transform _bar = null;

        [SerializeField] private Image _sceneAnim = null;
        [SerializeField] private Sprite[] _sceneSprites = null;

        private int _sceneIndex = 1;

        private PlayerProfile _playerProfile = null;

        private Vector3 _emptyPos = Vector3.zero;

        private Vector3 _fullPos = Vector3.zero;

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
            UpdateBar();
            OnInnUpgrade();
        }

        private void UpdateBar()
        {
            float t = _playerProfile.CurrentFortress.Beer / _playerProfile.CurrentFortress.BeerStorage;
            _bar.localPosition = Vector3.Lerp(_emptyPos, _fullPos, t);
        }

        private void OnInnUpgrade()
        {
            FortressProfile currentFortress = _playerProfile.CurrentFortress;
            InnUpgradesData uData = DatabaseManager.Instance.InnUpgrades;

            int price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.BeerByTap, currentFortress.InnBeerByTapIndex);
            _beerByTapUpgrade.Init(uData.BeerByTap.name, uData.BeerByTap.desc, currentFortress.InnBeerByTapIndex, price);

            price = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.Storage, currentFortress.InnStorageIndex);
            _storageUpgrade.Init(uData.Storage.name, uData.Storage.desc, currentFortress.InnStorageIndex, price);
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