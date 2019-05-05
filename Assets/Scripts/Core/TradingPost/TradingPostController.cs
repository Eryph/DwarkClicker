namespace Core
{
	using Core.Containers;
	using Core.Data;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections.Generic;
	using UnityEngine;

	public class TradingPostController : MonoBehaviour
	{
		#region Fields
		// Data
		private int _workerNb = 1;
		private int _sellByWorker = 1;
		private float _cycleDuration = 4f;
		private float _goldMultiplier = 0;
		private int _winBeerChance = 0;
		private int _winBeerAmount = 0;
		private int _winBeerChanceIncr = 0;
		private int _weaponToSellAmount = 0;

		private WeaponData _weaponToSell = null;

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
			_playerProfile.CurrentFortress.OnTPUpgradeChange += OnUpgrade;
			_playerProfile.OnFortressChange += HandleFortressChange;
			GameLoopManager.Instance.GameLoop += Loop;
			LoadData();

			if (ComputeWeaponToSell())
			{
				ResetTimer();
			}
			else
			{
				_timer.IsStopped = true;
			}
		}

		private void OnDestroy()
		{
			_playerProfile.CurrentFortress.OnTPUpgradeChange -= OnUpgrade;
			_playerProfile.OnFortressChange -= HandleFortressChange;
			GameLoopManager.Instance.GameLoop -= Loop;
			if (GameLoopManager.Instance)
			{
				GameLoopManager.Instance.GameLoop -= Loop;
				_playerProfile.CurrentFortress.OnTPUpgradeChange -= OnUpgrade;
			}
		}

		private void LoadData()
		{
			_weaponToSell = _playerProfile.CurrentFortress.CurrentCraft;
			_sellByWorker = _db.TradingPostStats.SellByWorker + _db.TradingPostUpgrades.SellByWorker.value * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._sellByWorkerIndex;
			_workerNb = _db.TradingPostStats.WorkerNb + _db.TradingPostUpgrades.WorkerNb.value * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._workerNbIndex;
			_cycleDuration = _db.TradingPostStats.CycleDuration;
			for (int i = 0; i <= _playerProfile.CurrentFortress.TradingPostUpgradesIndex._cycleDurationIndex; i++)
			{
				_cycleDuration -= _cycleDuration * _db.TradingPostUpgrades.CycleDuration.value;
			}

			_goldMultiplier = _db.TradingPostStats.GoldMult + _db.TradingPostUpgrades.GoldMult.value * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._goldMultIndex;
			_winBeerChance = _db.TradingPostStats.WinBeerChance - _db.TradingPostStats.WinBeerChance * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._winBeerChanceIndex;
			_winBeerAmount = _db.TradingPostStats.WinBeerAmount + _db.TradingPostStats.WinBeerAmount * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._winBeerAmountIndex;

			_weaponToSellAmount = _workerNb * _sellByWorker;
		}
		#endregion Monobehaviour

		#region Upgrades
		public void UpgradeSellByWorker()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.SellByWorker, _playerProfile.CurrentFortress.UTPSellByWorkerIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UTPSellByWorkerIndex++;
			}
		}

		public void UpgradeCycleDuration()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.CycleDuration, _playerProfile.CurrentFortress.UTPCycleDurationIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UTPCycleDurationIndex++;
			}
		}

		public void UpgradeWorkerNb()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WorkerNb, _playerProfile.CurrentFortress.UTPWorkerNbIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UTPWorkerNbIndex++;
			}
		}

		public void UpgradeGoldMult()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.GoldMult, _playerProfile.CurrentFortress.UTPGoldMultIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UTPGoldMultIndex++;
			}
		}

		public void UpgradeWinBeerChance()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerChance, _playerProfile.CurrentFortress.UTPWinBeerChanceIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UTPWinBeerChanceIndex++;
			}
		}

		public void UpgradeWinBeerAmount()
		{
			int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerAmount, _playerProfile.CurrentFortress.UTPWinBeerAmountIndex);
			if (_playerProfile.Gold >= cost)
			{
				_playerProfile.Gold -= cost;
				_playerProfile.CurrentFortress.UTPWinBeerAmountIndex++;
			}
		}
		#endregion Upgrades

		#region Loop
		private void Loop()
		{
			if (ComputeWeaponToSell())
			{
				if (_timer.IsStopped == true)
				{
					ResetTimer();
				}
				else if (_timer.IsTimerEnd())
				{
					_timer.Stop();

					//Normal Trading
					_converter.TradingPostConverter(_weaponToSell, _sellByWorker * _workerNb, _goldMultiplier);

					// Win Beer
					_winBeerChanceIncr++;
					if (_winBeerChanceIncr >= _winBeerChance)
					{
						_winBeerChanceIncr = 0;
						_converter.UpdateBeer(_winBeerAmount);
					}
				}
			}
			else
			{
				_timer.Stop();
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

		private void HandleFortressChange()
		{
			LoadData();
			_timer.Stop();
			_playerProfile.CurrentFortress.OnTPUpgradeChange += OnUpgrade;
		}
		#endregion Callbacks

		#region Utils
		private bool ComputeWeaponToSell()
		{
			WeaponData[] weapons = JSonManager.Instance.PlayerProfile.CurrentFortress._weapons;
			for (int i = 0; i < weapons.Length; i++)
			{
				if (_playerProfile.Weapons[weapons[i].Name].Count >= _weaponToSellAmount)
				{
					_weaponToSell = weapons[i];
					return true;
				}
			}
			return false;
		}
		#endregion Utils
		#endregion Methods
	}
}