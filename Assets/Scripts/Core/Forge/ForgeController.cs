namespace DwarfClicker.Core
{
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ForgeController : ABuildingBase
	{
		#region Fields
		// Data
		private int _workerNb = 1;
		private int _wByWorker = 1;
		private float _cycleDuration = 4f;
		private int _instantSellingChance = 100;
		private float _instantSellingGoldBonus = 1;

		private int _instantSellingIncr = 0;
		private int _forgeCount;
		private int _resourceConsumed;

		private WeaponData _currentForgingWeapon = null;

		// Utils
		[SerializeField] private Converter _converter = null;
		private Timer _timer = null;
		private PlayerProfile _playerProfile = null;
		private DatabaseManager _db = null;
		#endregion Fields

		#region Properties
		public float TimeLeft { get { return _timer.TimeLeft; } }
		public float CycleDuration { get { return _cycleDuration; } }
		public int ForgeCount { get { return _forgeCount; } }
		public int ResourceConsumed { get { return _resourceConsumed; } }
		public WeaponData CurrentForgingWeapon { get { return _currentForgingWeapon; } }
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
				_onWeaponChange += value;
			}
		}
		#endregion Events

		#region Methods
		#region Monobehaviour
		private void Start()
		{
			_timer = new Timer();
			_db = DatabaseManager.Instance;
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnForgeUpgradeChange += OnUpgrade;
			_playerProfile.OnFortressChange += HandleFortressChange;
			_playerProfile.CurrentFortress.OnWeaponChange += ChangeWeapon;
			GameLoopManager.Instance.GameLoop += Loop;

			LoadData();
			if (CanCraft(_workerNb * _wByWorker))
			{
				ResetTimer();
			}
		}

		private void OnDestroy()
		{
			_playerProfile.OnFortressChange -= HandleFortressChange;
			_playerProfile.CurrentFortress.OnForgeUpgradeChange -= OnUpgrade;
			_playerProfile.CurrentFortress.OnWeaponChange -= ChangeWeapon;
			GameLoopManager.Instance.GameLoop -= Loop;
		}

		private void LoadData()
		{
			ChangeWeapon();
			LoadWByWorker();
			LoadWorkerNb();
			LoadCycleDuration();
			LoadInstantSellingGoldBonus();
			LoadInstantSellingChance();
			_isPaused = _playerProfile.CurrentFortress.ForgeIsPaused;
			SetPause(_isPaused);
		}
		#endregion Monobehaviour

		#region Upgrades
		public void UpgradeCycleDuration()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.CycleDuration, _playerProfile.CurrentFortress.UForgeCycleDurationIndex);
			if (_playerProfile.Gold >= cost)
			{
				SoundManager.Instance.PlaySound("BUY_CLICK");
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeCycleDurationIndex++;
				LoadCycleDuration();
			}
			{
				SoundManager.Instance.PlaySound("ERROR_CLICK");
			}
		}

		public void UpgradeWByWorker()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WByWorker, _playerProfile.CurrentFortress.UForgeWByWorkerIndex);
			if (_playerProfile.Gold >= cost)
			{
				SoundManager.Instance.PlaySound("BUY_CLICK");
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeWByWorkerIndex++;
				LoadWByWorker();
			}
			else
			{
				SoundManager.Instance.PlaySound("ERROR_CLICK");
			}
		}

		public void UpgradeWorkerNb()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WorkerAmount, _playerProfile.CurrentFortress.UForgeWorkerNbIndex);
			if (_playerProfile.Gold >= cost)
			{
				SoundManager.Instance.PlaySound("BUY_CLICK");
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeWorkerNbIndex++;
				LoadWorkerNb();
			}
			else
			{
				SoundManager.Instance.PlaySound("ERROR_CLICK");
			}
		}

		public void UpgradeInstantSellingChance()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingChance, _playerProfile.CurrentFortress.UForgeInstantSellingChanceIndex);
			if (_playerProfile.Gold >= cost)
			{
				SoundManager.Instance.PlaySound("BUY_CLICK");
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeInstantSellingChanceIndex++;
				LoadInstantSellingChance();
			}
			else
			{
				SoundManager.Instance.PlaySound("ERROR_CLICK");
			}
		}

		public void UpgradeInstantSellingGoldBonus()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.InstantSellingGoldBonus, _playerProfile.CurrentFortress.UForgeInstantSellingGoldBonusIndex);
			if (_playerProfile.Gold >= cost)
			{
				SoundManager.Instance.PlaySound("BUY_CLICK");
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeInstantSellingGoldBonusIndex++;
				LoadInstantSellingGoldBonus();
			}
			else
			{
				SoundManager.Instance.PlaySound("ERROR_CLICK");
			}
		}
		#endregion Upgrades

		#region Loop
		private void Loop()
		{
			// Debug
			if (_isPaused)
			{
				if (!_timer.IsStopped)
				{
					_timer.Stop();
				}
			}
			else
			{
				if (_currentForgingWeapon == null)
				{
					ChangeWeapon();
				}

				if (CanCraft(_forgeCount) == true)
				{
					if (_timer.IsStopped == true)
					{
						ResetTimer();
					}
					else if (_timer.IsTimerEnd())
					{
						_instantSellingIncr++;
						_timer.Stop();
						if (_instantSellingIncr >= _instantSellingChance)
						{
							//Instant Selling
							_instantSellingIncr = 0;
							_converter.ForgeConverterInstantSelling(_currentForgingWeapon, _forgeCount, _instantSellingGoldBonus);
						}
						else
						{
							//Normal Forging
							_converter.ForgeConverter(_resourceConsumed, _currentForgingWeapon, _forgeCount);
						}

					}
				}
				else
				{
					_timer.Stop();
				}
			}
		}
		#endregion Loop

		#region Timer Management
		private void ResetTimer()
		{
			_timer.ResetTimer(_cycleDuration);
			_timer.IsStopped = false;
		}

		public override void Poltering()
		{
			if (!_timer.IsStopped)
			{
				_timer.ReduceRemainingTime(DatabaseManager.Instance.PolteringValue);
				_FXController.CreatePolteringParticle();
				SoundManager.Instance.PlayRandomSound("POLTERING_FORGE");
			}
			else
			{
				SoundManager.Instance.PlaySound("ERROR_CLICK");
			}
		}
		#endregion Timer Management

		#region Callbacks
		private void OnUpgrade()
		{
			LoadData();
		}

		private void ChangeWeapon()
		{
			_currentForgingWeapon = _playerProfile.CurrentFortress.CurrentCraft;
			if (_onWeaponChange != null)
				_onWeaponChange();
		}

		private void HandleFortressChange()
		{
			LoadData();
			ChangeWeapon();
			_playerProfile.CurrentFortress.OnForgeUpgradeChange += OnUpgrade;
			_playerProfile.CurrentFortress.OnWeaponChange += ChangeWeapon;
			_timer.Stop();
		}

		private void SetPause(bool isPaused)
		{
			_playerProfile.CurrentFortress.ForgeIsPaused = isPaused;
		}
		#endregion Callbacks

		#region Utils
		private bool CanCraft(int mult)
		{
			for (int i = 0; i < _currentForgingWeapon.Recipe.Length; i++)
			{
				if (_playerProfile.Resources.ContainsKey(_currentForgingWeapon.Recipe[i].Key) == false)
				{
					return false;
				}
				else if (_playerProfile.Resources[_currentForgingWeapon.Recipe[i].Key].Count < _currentForgingWeapon.Recipe[i].Count * mult)
				{
					return false;
				}
			}
			return (true);
		}

		private void LoadWorkerNb()
		{
			_workerNb = _db.ForgeStats.WorkerAmount + _db.ForgeUpgrades.WorkerAmount.value * _playerProfile.CurrentFortress.ForgeUpgradesIndex._workerNbIndex;
			_forgeCount = _workerNb * _wByWorker;
			_resourceConsumed = _currentForgingWeapon.Recipe[0].Count * _forgeCount;
		}

		private void LoadWByWorker()
		{
			_wByWorker = _db.ForgeStats.WByWorker + _db.ForgeUpgrades.WByWorker.value * _playerProfile.CurrentFortress.ForgeUpgradesIndex._wByWorkerIndex;
			_forgeCount = _workerNb * _wByWorker;
			_resourceConsumed = _currentForgingWeapon.Recipe[0].Count * _forgeCount;
		}

		private void LoadCycleDuration()
		{
			_cycleDuration = _db.ForgeStats.CycleDuration;
			for (int i = 0; i < _playerProfile.CurrentFortress.ForgeUpgradesIndex._cycleDurationIndex; i++)
			{
				_cycleDuration -= _cycleDuration * _db.ForgeUpgrades.CycleDuration.value;
			}
		}

		private void LoadInstantSellingChance()
		{
			_instantSellingChance = _db.ForgeStats.InstantSellingChance - _db.ForgeUpgrades.InstantSellingChance.value * _playerProfile.CurrentFortress.ForgeUpgradesIndex._instantSellingChanceIndex;
		}

		private void LoadInstantSellingGoldBonus()
		{
			_instantSellingGoldBonus = _db.ForgeStats.InstantSellingGoldBonus + _db.ForgeUpgrades.InstantSellingGoldBonus.value * _playerProfile.CurrentFortress.ForgeUpgradesIndex._instantSellingGoldBonusIndex;
		}
		#endregion Utils
		#endregion Methods
	}
}