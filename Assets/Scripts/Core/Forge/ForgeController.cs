namespace DwarfClicker.Core
{
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ForgeController : BuildingBase
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
		#endregion Properties

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
			LoadWByWorker();
			LoadWorkerNb();
			LoadCycleDuration();
			LoadInstantSellingGoldBonus();
			LoadInstantSellingChance();
			ChangeWeapon();
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
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeCycleDurationIndex++;
				LoadCycleDuration();
			}
		}

		public void UpgradeWByWorker()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WByWorker, _playerProfile.CurrentFortress.UForgeWByWorkerIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeWByWorkerIndex++;
				LoadWByWorker();
			}
		}

		public void UpgradeWorkerNb()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.ForgeUpgrades.WorkerAmount, _playerProfile.CurrentFortress.UForgeWorkerNbIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeWorkerNbIndex++;
				LoadWorkerNb();
			}
		}

		public void UpgradeInstantSellingChance()
		{
			int cost = (_playerProfile.CurrentFortress.UForgeInstantSellingChanceIndex + 1) * DatabaseManager.Instance.ForgeUpgrades.InstantSellingChance.cost;
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeInstantSellingChanceIndex++;
				LoadInstantSellingChance();
			}
		}

		public void UpgradeInstantSellingGoldBonus()
		{
			int cost = (_playerProfile.CurrentFortress.UForgeInstantSellingGoldBonusIndex + 1) * DatabaseManager.Instance.ForgeUpgrades.InstantSellingGoldBonus.cost;
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UForgeInstantSellingGoldBonusIndex++;
				LoadInstantSellingGoldBonus();
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
							_converter.ForgeConverter(_currentForgingWeapon, _forgeCount);
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
		#endregion Timer Management

		#region Callbacks
		private void OnUpgrade()
		{
			LoadData();
		}

		private void ChangeWeapon()
		{
			_currentForgingWeapon = _playerProfile.CurrentFortress.CurrentCraft;
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
		}

		private void LoadWByWorker()
		{
			_wByWorker = _db.ForgeStats.WByWorker + _db.ForgeUpgrades.WByWorker.value * _playerProfile.CurrentFortress.ForgeUpgradesIndex._wByWorkerIndex;
			_forgeCount = _workerNb * _wByWorker;
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