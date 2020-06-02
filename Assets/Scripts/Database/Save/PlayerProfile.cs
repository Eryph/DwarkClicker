namespace Engine.Utils
{
	using System.Collections.Generic;
	using UnityEngine;
	using System;
	using DwarfClicker.Core.Containers;
	using Engine.Manager;
	using DwarfClicker.Core.Achievement;

	[Serializable]
	public class PlayerProfile
	{
		#region Fields
		#region Config
		public bool _isSoundMuted = false;
		public bool _isMusicMuted = false;
		#endregion Config

		#region Consumable Bonus
		public bool _noMoreAdsBonus = false;
        public bool _prodSpeedBonus = false;
        public float _prodSpeedBonusTimeRemaing = 0f;
        #endregion Consumable Bonus

        #region Permanent Bonus
        public float _goldMultiplierBonus = 1;
        public float _resourcesMultiplierBonus = 1;
        public float _toolsMultiplierBonus = 1;
        #endregion Permanent Bonus

        #region Consumable Bonus
        public float _bonusTimeRemaining = 0;
        #endregion Consumable Bonus

        #region Time
        private DateTime _date;
		public DateTime _startDate;
		private DateTime _fortressDate;
		public DateTime _lastDailyRewardRedeemed;
		public DateTime _taskTimeStamp;

		public long _serializedDate = 0;
		public long _serializedStartingDate = 0;
		public long _serializedLastDailyRewardRedeemed = 0;
		public long _serializedTaskTimeStamp = 0;
		public int _launchAmount = 0;
		public int _dailyRewardIndex = 0;
		public bool _isDailyRewardAvailable = true;
		#endregion Time

		#region Fortress
		public List<FortressProfile> _fortressList = null;
		public int _currentFortressIndex = 0;
		#endregion Fortress

		#region FTUE
		public int _currentStep = 0;
		#endregion FTUE

		#region Inventory
		public int _gold = 0;
		public int _mithril = 0;

		public DictionaryStringResource _resources = null;
		public DictionaryStringWeapon _weapons = null;
        private bool _hasReset = false;
		#endregion Inventory

		#region Achievement
		public DictionaryStringAchievement _achievements = null;
		#endregion Achievement

		#region KingTask
		public KingTask _kingTask = null;
		#endregion KingTask
		#endregion Fields

		#region Properties
		#region FTUE
		public int FTUEStep
		{
			get { return _currentStep; }
			set
			{
				_currentStep = value;
				if (_onFTUEStepChange != null)
				{
                    if (value != 0)
					    _onFTUEStepChange();
				}
			}
		}
		#endregion FTUE

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

		public bool IsDailyRewardAvailable
		{
			get
			{
				return _isDailyRewardAvailable;
			}
			set
			{
				_isDailyRewardAvailable = value;
			}
		}

		public bool IsDailyRewardReset
		{
			get
			{
				TimeSpan oneDay = new TimeSpan(23, 0, 0);
				TimeSpan diff = DateTime.Now - _lastDailyRewardRedeemed;
				return oneDay.Add(new TimeSpan(24, 0, 0)) < diff;
			}
		}

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

		public int FortressCount
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
				return (y + 1);
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
        public bool HasReset { get { return _hasReset; } }

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
				if (_gold < value)
					AchievementManager.Instance.UpdateAchievement("GOLD", value - _gold);
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
		#region FTUE
		private Action _onFTUEStepChange = null;

		public event Action OnFTUEStepChange
		{
			add
			{
				_onFTUEStepChange -= value;
				_onFTUEStepChange += value;
			}
			remove
			{
				_onFTUEStepChange -= value;
			}
		}
		#endregion FTUE

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
		#endregion Events

		#region Methods
		#region Init / Reset
		public void Init()
		{
			FTUEStep = 0;
			
			_launchAmount = 0;
			_startDate = DateTime.Now;
			_lastDailyRewardRedeemed = DateTime.Now - new TimeSpan(100, 0, 0);
			_taskTimeStamp = DateTime.Now - new TimeSpan(100, 0, 0);
			_resources = new DictionaryStringResource();
			_weapons = new DictionaryStringWeapon();
			_achievements = AchievementManager.Instance.GenerateAchievementCollection();
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

			_kingTask = AchievementManager.Instance.GenerateTask();
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
            // Bonus Clear
            _noMoreAdsBonus = false;
            _goldMultiplierBonus = 1;
            _resourcesMultiplierBonus = 1;
            _toolsMultiplierBonus = 1;
            _bonusTimeRemaining = 0;

            // Fortress Clear
            CurrentFortressIndex = 0;
			_fortressList.Clear();

			// Inventory Clear
			ResetCollectionCounts();
			Gold = 0;
			Mithril = 0;
            
			_dailyRewardIndex = 0;

			Init();

			JSonManager.Instance.SavePlayerProfile();
            Application.Quit();
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
			_serializedTaskTimeStamp = _taskTimeStamp.ToFileTime();
		}

		public void DeserializeDate()
		{
			_date = DateTime.FromFileTime(_serializedDate);
			_startDate = DateTime.FromFileTime(_serializedStartingDate);
			_lastDailyRewardRedeemed = DateTime.FromFileTime(_serializedLastDailyRewardRedeemed);
			_taskTimeStamp = DateTime.FromFileTime(_serializedTaskTimeStamp);
		}
		#endregion Serialization
		#endregion Methods
	}
}