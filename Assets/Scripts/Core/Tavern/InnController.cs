namespace DwarfClicker.Core
{
	using DwarfClicker.Misc;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class InnController : MonoBehaviour
	{
		#region Fields
		// Data
		private int _beerByTap = 1;
		private int _storage = 1;

		// Utils
		[SerializeField] private FXController _FXController = null;
		[SerializeField] private Converter _converter = null;
		private PlayerProfile _playerProfile = null;
		private DatabaseManager _db = null;
        private bool _isGoldTrans = true;
        #endregion Fields

        #region Properties
        public bool IsGoldTrans { get { return _isGoldTrans; } set { _isGoldTrans = value; } }
        #endregion Properties

        #region Methods
        #region Monobehaviour
        private void Start()
		{
			_db = DatabaseManager.Instance;
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnInnUpgradeChange += OnUpgrade;
			_playerProfile.OnFortressChange += LoadData;

			LoadData();
		}

		private void LoadData()
		{
			LoadBeerByTap();
			LoadStorage();
		}
		#endregion Monobehaviour

		#region Beer Brewing
		public void BrewBeer()
		{
			
			_converter.UpdateBeer(_beerByTap);
			if (_playerProfile.CurrentFortress.Beer <= _storage)
			{
				_FXController.CreateBeerTapParticle();
			}

			_playerProfile.CurrentFortress.Beer = Mathf.Clamp(_playerProfile.CurrentFortress.Beer, 0f, _storage);

			
		}
		#endregion Beer Brewing

		#region Upgrades
		public void UpgradeBeerByTap()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.BeerByTap, _playerProfile.CurrentFortress.InnBeerByTapIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.InnBeerByTapIndex++;
                    LoadBeerByTap();
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
            else
            {
                if (_playerProfile.Mithril >= DatabaseManager.Instance.UpgradeMithrilPrice)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Mithril -= DatabaseManager.Instance.UpgradeMithrilPrice;
                    _playerProfile.CurrentFortress.InnBeerByTapIndex++;
                    LoadBeerByTap();
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }

		public void UpgradeStorage()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.InnUpgrades.Storage, _playerProfile.CurrentFortress.InnStorageIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.InnStorageIndex++;
                    LoadStorage();
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
            else
            {
                if (_playerProfile.Mithril >= DatabaseManager.Instance.UpgradeMithrilPrice)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Mithril -= DatabaseManager.Instance.UpgradeMithrilPrice;
                    _playerProfile.CurrentFortress.InnStorageIndex++;
                    LoadStorage();
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }
		#endregion

		#region Callbacks
		private void OnUpgrade()
		{
			LoadData();
		}
		#endregion Callbacks

		#region Utils
		private void LoadBeerByTap()
		{
			_beerByTap = _db.InnStats.BeerByTap + _db.InnUpgrades.BeerByTap.value * _playerProfile.CurrentFortress.InnUpgradesIndex._beerByTapIndex;
		}

		private void LoadStorage()
		{
			_storage = _db.InnStats.Storage + _db.InnUpgrades.Storage.value * _playerProfile.CurrentFortress.InnUpgradesIndex._storageIndex;
			_playerProfile.CurrentFortress.BeerStorage = _storage;
		}
		#endregion Utils
		#endregion Methods
	}
}