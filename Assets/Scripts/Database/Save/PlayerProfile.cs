namespace Engine.Utils
{
	using System.Collections.Generic;
	using UnityEngine;
	using System;
	using DwarfClicker.Core.Containers;
	using Engine.Manager;

	[Serializable]
	public class PlayerProfile
	{
		#region Fields
		#region Time
		private DateTime _date;
		public DateTime _startDate;
		private DateTime _fortressDate;
		public DateTime _lastDailyRewardRedeemed;

		public long _serializedDate = 0;
		public long _serializedStartingDate = 0;
		public long _serializedLastDailyRewardRedeemed = 0;
		public int _launchAmount = 0;
		public int _dailyRewardIndex = 0;
		#endregion Time

		#region Fortress
		public List<FortressProfile> _fortressList = null;
		public int _currentFortressIndex = 0;
		#endregion Fortress

		#region Inventory
		public int _gold = 0;
		public int _mithril = 0;

		public DictionaryStringResource _resources = null;
		public DictionaryStringWeapon _weapons = null;
		#endregion Inventory
		#endregion Fields

		#region Properties
		#region Time
		public DateTime Date { get { return _date; } set { _date = value; } }
		public DateTime StartingDate { get { return _startDate; } }
		public DateTime LastDailyRewardRedeemed { get { return _lastDailyRewardRedeemed; } }
		#endregion Time

		#region Fortress
		public DateTime FortressDate { get { return _fortressDate; } set { _fortressDate = value; } }
		public List<FortressProfile> Fortress { get { return _fortressList; } }
		public FortressProfile CurrentFortress { get { return _fortressList[_currentFortressIndex]; } }
		public int LaunchAmount { get { return _launchAmount; } set { _launchAmount = value; } }

		public int CurrentFortressIndex
		{
			get
			{
				return _currentFortressIndex;
			}
			set
			{
				CurrentFortress.ResetEvents();
				GameManager.Instance.LoadProgression(true);
				_currentFortressIndex = value;
				if (_onFortressChange != null)
					_onFortressChange();
			}
		}

		public FortressProfile LastFortress
		{
			get
			{
				int y = 0;
				int i = 0;
				while (i < _fortressList.Count)
				{
					if (_fortressList[i]._isBought == false)
					{
						break;
					}
					y = i;
					i++;
				}
				return (_fortressList[y]);
			}
		}
		#endregion Fortress

		#region Inventory
		public DictionaryStringResource Resources { get { return _resources; } }

		public DictionaryStringWeapon Weapons { get { return _weapons; } }

		public int Gold
		{
			get
			{
				return _gold;
			}
			set
			{
				_gold = value;
				if (_onGoldChange != null)
					_onGoldChange();
			}
		}

		public int Mithril
		{
			get
			{
				return _mithril;
			}
			set
			{
				_mithril = value;
				if (_onMithrilChange != null)
					_onMithrilChange();
			}
		}
		#endregion Inventory
		#endregion Properties

		#region Events
		#region Fortress
		private Action _onFortressChange = null;

		public event Action OnFortressChange
		{
			add
			{
				_onFortressChange -= value;
				_onFortressChange += value;
			}
			remove
			{
				_onFortressChange -= value;
			}
		}
		#endregion Fortress

		#region Inventory
		private Action _onInventoryChange = null;

		public event Action OnInventoryChange
		{
			add
			{
				_onInventoryChange -= value;
				_onInventoryChange += value;
			}
			remove
			{
				_onInventoryChange -= value;
			}
		}

		private Action _onGoldChange = null;

		public event Action OnGoldChange
		{
			add
			{
				_onGoldChange -= value;
				_onGoldChange += value;
			}
			remove
			{
				_onGoldChange -= value;
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

		private Action _onMithrilChange = null;

		public event Action OnMithrilChange
		{
			add
			{
				_onMithrilChange -= value;
				_onMithrilChange += value;
			}
			remove
			{
				_onMithrilChange -= value;
			}
		}
		#endregion Inventory

		/*#region Upgrades
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
		#endregion Upgrades*/
		#endregion Events

		#region Methods
		#region Init / Reset
		public void Init()
		{
			_launchAmount = 0;
			_startDate = DateTime.Now;
			_resources = new DictionaryStringResource();
			_weapons = new DictionaryStringWeapon();
			_fortressList = new List<FortressProfile>();
			DatabaseManager db = DatabaseManager.Instance;

			for (int i = 0; i < db.ResourceList.Resources.Length; i++)
			{
				Resource resource = new Resource();
				resource.Init(db.ResourceList.Resources[i]);
				_resources.Add(db.ResourceList.Resources[i].Name, resource);
			}

			for (int i = 0; i < db.WeaponList.Weapons.Length; i++)
			{
				Weapon weapon = new Weapon();
				weapon.Init(db.WeaponList.Weapons[i]);
				_weapons.Add(db.WeaponList.Weapons[i].Name, weapon);
			}

			for (int i = 0; i < db.Fortress.Length; i++)
			{
				FortressProfile fortress = new FortressProfile();
				
				if (i <= 0)
				{
					fortress._isBought = true;
				}
				else
				{
					fortress._isBought = false;
				}

				fortress.Init(db.Fortress[i].Name, db.Fortress[i].WeaponsToProduce, db.Fortress[i].ResourceToProduce);
				_fortressList.Add(fortress);
			}
		}

		public void InitFortressInstanceData()
		{
			DatabaseManager db = DatabaseManager.Instance;

			for (int i = 0; i < db.Fortress.Length; i++)
			{
				Fortress[i]._resourceProduced = db.Fortress[i].ResourceToProduce;
				Fortress[i]._weapons = db.Fortress[i].WeaponsToProduce;
			}
		}

		public void Reset()
		{
			// Fortress Clear
			
			_fortressList.Clear();

			// Inventory Clear
			Init();
			ResetCollectionCounts();

			Gold = 0;
			Mithril = 0;
			CurrentFortressIndex = 0;
			_dailyRewardIndex = 0;

			JSonManager.Instance.SavePlayerProfile();
		}

		private void ResetCollectionCounts()
		{
			foreach (KeyValuePair<string, Resource> resource in _resources)
			{
				resource.Value.ResetCount();
			}

			foreach (KeyValuePair<string, Weapon> weapon in _weapons)
			{
				weapon.Value.ResetCount();
			}
		}
		#endregion Init / Reset

		#region Event Trigger
		public void TriggerInventoryChangeEvent()
		{
			if (_onInventoryChange != null)
				_onInventoryChange();
		}
		#endregion Event Trigger

		#region Serialization
		public void SerializeDate(DateTime date)
		{
			_serializedDate = date.ToFileTime();
			_serializedStartingDate = _startDate.ToFileTime();
			_serializedLastDailyRewardRedeemed = _lastDailyRewardRedeemed.ToFileTime();
		}

		public void DeserializeDate()
		{
			_date = DateTime.FromFileTime(_serializedDate);
			_startDate = DateTime.FromFileTime(_serializedStartingDate);
			_lastDailyRewardRedeemed = DateTime.FromFileTime(_serializedLastDailyRewardRedeemed);
		}
		#endregion Serialization
		#endregion Methods
	}
}