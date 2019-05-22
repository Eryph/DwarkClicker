namespace Engine.Utils
{
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class FortressProfile
	{
		#region Fields
		public bool _isBought = false;
		public string _name = "FortressName";
		public int _fortressIndex = 0;
		#region Production
		public int _weaponIndex = 0;
		public WeaponData[] _weapons = null;
		public ResourceData _resourceProduced = null;
		public float _beer = 0;
		public float _beerStorage = 0;
		public int _modifierTimer = 0;
		#endregion Production

		public bool _tradingPostIsPaused = false;
		public bool _mineIsPaused = false;
		public bool _forgeIsPaused = false;

		#region Time
		private DateTime _date;
		#endregion Time
		 
		#region Upgrades
		public MineUpgradesIndex _mineUpgradesIndex = null;
		public ForgeUpgradesIndex _forgeUpgradesIndex = null;
		public TradingPostUpgradesIndex _tradingPostUpgradesIndex = null;
		public InnUpgradesIndex _innUpgradesIndex = null;
		#endregion Upgrades
		#endregion Fields

		#region Properties
		public bool TradingPostIsPaused { get { return _tradingPostIsPaused; } set { _tradingPostIsPaused = value; } }
		public bool MineIsPaused { get { return _mineIsPaused; } set { _mineIsPaused = value; } }
		public bool ForgeIsPaused { get { return _forgeIsPaused; } set { _forgeIsPaused = value; } }


		public int ModifierTimer { get { return _modifierTimer; } set { _modifierTimer = value; } }

		public int WeaponIndex { get { return _weaponIndex; }
			set {
				_weaponIndex = value;
				if (_onWeaponChange != null)
					_onWeaponChange();
			}
		}

		public WeaponData CurrentCraft { get { return _weapons[_weaponIndex]; } }
		public WeaponData[] WeaponToCraft { get { return _weapons; } }// set { _weapons = value; } }
		public ResourceData ResourceProduced { get { return _resourceProduced; } } // set { _resourceProduced = value; } }


		public int FortressIndex { get { return _fortressIndex; } }

		public string Name { get { return _name; } set { _name = value; } }

		public float Beer
		{
			get
			{
				return _beer;
			}
			set
			{
				_beer = value;
				_beer = Mathf.Clamp(_beer, 0f, _beerStorage);
				if (_onBeerChange != null)
					_onBeerChange();
			}
		}

		public float BeerStorage { get { return _beerStorage; } set { _beerStorage = value; } }

		#region Mine Upgrades
		public MineUpgradesIndex MineUpgradesIndex { get { return _mineUpgradesIndex; } }

		public int UMineWorkerNbIndex
		{
			get
			{
				return _mineUpgradesIndex._workerNbIndex;
			}
			set
			{
				_mineUpgradesIndex._workerNbIndex = value;
				if (_onMineUpgradeChange != null)
					_onMineUpgradeChange();
			}
		}

		public int UMineResByWorkerIndex
		{
			get
			{
				return _mineUpgradesIndex._resByWorkerIndex;
			}
			set
			{
				_mineUpgradesIndex._resByWorkerIndex = value;
				if (_onMineUpgradeChange != null)
					_onMineUpgradeChange();
			}
		}

		public int UMineBeerConsoIndex
		{
			get
			{
				return _mineUpgradesIndex._beerConsoIndex;
			}
			set
			{
				_mineUpgradesIndex._beerConsoIndex = value;
				if (_onMineUpgradeChange != null)
					_onMineUpgradeChange();
			}
		}

		public int UMineLuckIndex
		{
			get
			{
				return _mineUpgradesIndex._luckIndex;
			}
			set
			{
				_mineUpgradesIndex._luckIndex = value;
				if (_onMineUpgradeChange != null)
					_onMineUpgradeChange();
			}
		}

		public int UMineCycleDurationIndex
		{
			get
			{
				return _mineUpgradesIndex._cycleDurationIndex;
			}
			set
			{
				_mineUpgradesIndex._cycleDurationIndex = value;
				if (_onMineUpgradeChange != null)
					_onMineUpgradeChange();
			}
		}

		public int UMineRichVeinIndex
		{
			get
			{
				return _mineUpgradesIndex._richVeinIndex;
			}
			set
			{

				_mineUpgradesIndex._richVeinIndex = value;
				if (_onMineUpgradeChange != null)
					_onMineUpgradeChange();
			}
		}

		public int UMineMithrilChanceIndex
		{
			get
			{
				return _mineUpgradesIndex._mithrilChanceIndex;
			}
			set
			{
				_mineUpgradesIndex._mithrilChanceIndex = value;
				if (_onMineUpgradeChange != null)
					_onMineUpgradeChange();
			}
		}
		#endregion Mine Upgrades

		#region Forge Upgrades
		public ForgeUpgradesIndex ForgeUpgradesIndex { get { return _forgeUpgradesIndex; } }

		public int UForgeWorkerNbIndex
		{
			get
			{
				return _forgeUpgradesIndex._workerNbIndex;
			}
			set
			{
				_forgeUpgradesIndex._workerNbIndex = value;
				if (_onForgeUpgradeChange != null)
					_onForgeUpgradeChange();
			}
		}

		public int UForgeWByWorkerIndex
		{
			get
			{
				return _forgeUpgradesIndex._wByWorkerIndex;
			}
			set
			{
				_forgeUpgradesIndex._wByWorkerIndex = value;
				if (_onForgeUpgradeChange != null)
					_onForgeUpgradeChange();
			}
		}

		public int UForgeCycleDurationIndex
		{
			get
			{
				return _forgeUpgradesIndex._cycleDurationIndex;
			}
			set
			{
				_forgeUpgradesIndex._cycleDurationIndex = value;
				if (_onForgeUpgradeChange != null)
					_onForgeUpgradeChange();
			}
		}

		public int UForgeInstantSellingChanceIndex
		{
			get
			{
				return _forgeUpgradesIndex._instantSellingChanceIndex;
			}
			set
			{
				_forgeUpgradesIndex._instantSellingChanceIndex = value;
				if (_onForgeUpgradeChange != null)
					_onForgeUpgradeChange();
			}
		}

		public int UForgeInstantSellingGoldBonusIndex
		{
			get
			{
				return _forgeUpgradesIndex._instantSellingGoldBonusIndex;
			}
			set
			{
				_forgeUpgradesIndex._instantSellingGoldBonusIndex = value;
				if (_onForgeUpgradeChange != null)
					_onForgeUpgradeChange();
			}
		}
		#endregion Forge Upgrades

		#region Trading Post Upgrades
		public TradingPostUpgradesIndex TradingPostUpgradesIndex { get { return _tradingPostUpgradesIndex; } }

		public int UTPWorkerNbIndex
		{
			get
			{
				return _tradingPostUpgradesIndex._workerNbIndex;
			}
			set
			{
				_tradingPostUpgradesIndex._workerNbIndex = value;
				if (_onTPUpgradeChange != null)
					_onTPUpgradeChange();
			}
		}

		public int UTPSellByWorkerIndex
		{
			get
			{
				return _tradingPostUpgradesIndex._sellByWorkerIndex;
			}
			set
			{
				_tradingPostUpgradesIndex._sellByWorkerIndex = value;
				if (_onTPUpgradeChange != null)
					_onTPUpgradeChange();
			}
		}

		public int UTPCycleDurationIndex
		{
			get
			{
				return _tradingPostUpgradesIndex._cycleDurationIndex;
			}
			set
			{
				_tradingPostUpgradesIndex._cycleDurationIndex = value;
				if (_onTPUpgradeChange != null)
					_onTPUpgradeChange();
			}
		}

		public int UTPWinBeerChanceIndex
		{
			get
			{
				return _tradingPostUpgradesIndex._winBeerChanceIndex;
			}
			set
			{
				_tradingPostUpgradesIndex._winBeerChanceIndex = value;
				if (_onTPUpgradeChange != null)
					_onTPUpgradeChange();
			}
		}

		public int UTPWinBeerAmountIndex
		{
			get
			{
				return _tradingPostUpgradesIndex._winBeerAmountIndex;
			}
			set
			{
				_tradingPostUpgradesIndex._winBeerAmountIndex = value;
				if (_onTPUpgradeChange != null)
					_onTPUpgradeChange();
			}
		}

		public int UTPGoldMultIndex
		{
			get
			{
				return _tradingPostUpgradesIndex._goldMultIndex;
			}
			set
			{
				_tradingPostUpgradesIndex._goldMultIndex = value;
				if (_onTPUpgradeChange != null)
					_onTPUpgradeChange();
			}
		}
		#endregion Trading Post Upgrades

		#region Inn Upgrades
		public InnUpgradesIndex InnUpgradesIndex { get { return _innUpgradesIndex; } }

		public int InnBeerByTapIndex
		{
			get
			{
				return _innUpgradesIndex._beerByTapIndex;
			}
			set
			{
				_innUpgradesIndex._beerByTapIndex = value;
				if (_onInnUpgradeChange != null)
					_onInnUpgradeChange();
			}
		}

		public int InnStorageIndex
		{
			get
			{
				return _innUpgradesIndex._storageIndex;
			}
			set
			{
				_innUpgradesIndex._storageIndex = value;
				if (_onInnUpgradeChange != null)
					_onInnUpgradeChange();
			}
		}
		#endregion Inn Upgrades
		#endregion Properties

		#region Events
		private Action _onWeaponChange = null;

		public event Action OnWeaponChange
		{
			add
			{
				_onWeaponChange -= value;
				_onWeaponChange += value;
			}
			remove
			{
				_onWeaponChange -= value;
			}
		}

		private Action _onBeerChange = null;

		public event Action OnBeerChange
		{
			add
			{
				_onBeerChange -= value;
				_onBeerChange += value;
			}
			remove
			{
				_onBeerChange -= value;
			}
		}

		private Action _onMineUpgradeChange = null;

		public event Action OnMineUpgradeChange
		{
			add
			{
				_onMineUpgradeChange -= value;
				_onMineUpgradeChange += value;
			}
			remove
			{
				_onMineUpgradeChange -= value;
			}
		}

		private Action _onForgeUpgradeChange = null;

		public event Action OnForgeUpgradeChange
		{
			add
			{
				_onForgeUpgradeChange -= value;
				_onForgeUpgradeChange += value;
			}
			remove
			{
				_onForgeUpgradeChange -= value;
			}
		}

		private Action _onTPUpgradeChange = null;

		public event Action OnTPUpgradeChange
		{
			add
			{
				_onTPUpgradeChange -= value;
				_onTPUpgradeChange += value;
			}
			remove
			{
				_onTPUpgradeChange -= value;
			}
		}

		private Action _onInnUpgradeChange = null;

		public event Action OnInnUpgradeChange
		{
			add
			{
				_onInnUpgradeChange -= value;
				_onInnUpgradeChange += value;
			}
			remove
			{
				_onInnUpgradeChange -= value;
			}
		}
		#endregion Events

		#region Methods
		public void Init(string name, WeaponData[] weapons, ResourceData resource)
		{
			_beer = 0;

			_forgeUpgradesIndex = new ForgeUpgradesIndex();
			_mineUpgradesIndex = new MineUpgradesIndex();
			_tradingPostUpgradesIndex = new TradingPostUpgradesIndex();
			_innUpgradesIndex = new InnUpgradesIndex();

			_forgeUpgradesIndex.ResetUpgrades();
			_mineUpgradesIndex.ResetUpgrades();
			_tradingPostUpgradesIndex.ResetUpgrades();
			_innUpgradesIndex.ResetUpgrades();

			Name = name;
			InitInstanceData(weapons, resource);
		}

		public void InitInstanceData(WeaponData[] weapons, ResourceData resource)
		{
			_weapons = weapons;
			_resourceProduced = resource;
		}

		public void ResetEvents()
		{
			_onWeaponChange = null;
			_onTPUpgradeChange = null;
			_onBeerChange = null;
			_onForgeUpgradeChange = null;
			_onInnUpgradeChange = null;
			_onMineUpgradeChange = null;
		}
		#endregion Methods
	}
}