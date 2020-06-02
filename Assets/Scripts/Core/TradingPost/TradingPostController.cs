namespace DwarfClicker.Core
{
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections.Generic;
	using UnityEngine;

	public class TradingPostController : ABuildingBase
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
		private int _realToSellAmount = 0;
		private int _goldToProduce = 0;

		private WeaponData _weaponToSell = null;

		// Utils
		[SerializeField] private Converter _converter = null;
		private Timer _timer = null;
		private PlayerProfile _playerProfile = null;
		private DatabaseManager _db = null;
        private bool _isGoldTrans = true;
		#endregion Fields

		#region Properties
		public int WeaponToSellAmount { get { return _weaponToSellAmount; } }
		public int RealToSellAmount { get { return _realToSellAmount; } }
		public int GoldToProduce { get { return _goldToProduce; } }
		public float TimeLeft { get { return _timer.TimeLeft; } }
		public float CycleDuration { get { return _cycleDuration; } }
		public WeaponData WeaponToSell { get { return _weaponToSell; } }
        public bool IsGoldTrans { get { return _isGoldTrans; } set { _isGoldTrans = value; } }
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
			OnPause += SetPause;
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
			_workerNb = _db.TradingPostStats.WorkerAmount + _db.TradingPostUpgrades.WorkerAmount.value * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._workerNbIndex;
			_cycleDuration = _db.TradingPostStats.CycleDuration;
			for (int i = 0; i < _playerProfile.CurrentFortress.TradingPostUpgradesIndex._cycleDurationIndex; i++)
			{
				_cycleDuration -= _cycleDuration * _db.TradingPostUpgrades.CycleDuration.value;
			}

			_goldMultiplier = _db.TradingPostStats.GoldMult + _db.TradingPostUpgrades.GoldMult.value * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._goldMultIndex;
			_winBeerChance = _db.TradingPostStats.WinBeerChance - _db.TradingPostStats.WinBeerChance * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._winBeerChanceIndex;
			_winBeerAmount = _db.TradingPostStats.WinBeerAmount + _db.TradingPostStats.WinBeerAmount * _playerProfile.CurrentFortress.TradingPostUpgradesIndex._winBeerAmountIndex;

			_weaponToSellAmount = _workerNb * _sellByWorker;

			_isPaused = _playerProfile.CurrentFortress.TradingPostIsPaused;
			ComputeToSellData(false);
			SetPause(_isPaused);
		}
		#endregion Monobehaviour

		#region Upgrades
		public void UpgradeSellByWorker()
		{
            if (!FTUEManager.Instance.IsActivated)
            {
                if (_isGoldTrans)
                {
                    int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.SellByWorker, _playerProfile.CurrentFortress.UTPSellByWorkerIndex);
                    if (_playerProfile.Gold >= cost)
                    {
                        SoundManager.Instance.PlaySound("BUY_CLICK");
                        _playerProfile.Gold -= cost;
                        _playerProfile.CurrentFortress.UTPSellByWorkerIndex++;
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
                        _playerProfile.CurrentFortress.UTPSellByWorkerIndex++;
                    }
                    else
                    {
                        SoundManager.Instance.PlaySound("ERROR_CLICK");
                    }
                }
            }
		}

		public void UpgradeCycleDuration()
		{
            if (!FTUEManager.Instance.IsActivated)
            {
                if (_isGoldTrans)
                {
                    int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.CycleDuration, _playerProfile.CurrentFortress.UTPCycleDurationIndex);
                    if (_playerProfile.Gold >= cost)
                    {
                        SoundManager.Instance.PlaySound("BUY_CLICK");
                        _playerProfile.Gold -= cost;
                        _playerProfile.CurrentFortress.UTPCycleDurationIndex++;
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
                        _playerProfile.CurrentFortress.UTPCycleDurationIndex++;
                    }
                    else
                    {
                        SoundManager.Instance.PlaySound("ERROR_CLICK");
                    }
                }
            }
		}

		public void UpgradeWorkerNb()
		{
            if (!FTUEManager.Instance.IsActivated)
            {
                if (_isGoldTrans)
                {
                    int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WorkerAmount, _playerProfile.CurrentFortress.UTPWorkerNbIndex);
                    if (_playerProfile.Gold >= cost)
                    {
                        SoundManager.Instance.PlaySound("BUY_CLICK");
                        _playerProfile.Gold -= cost;
                        _playerProfile.CurrentFortress.UTPWorkerNbIndex++;
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
                        _playerProfile.CurrentFortress.UTPWorkerNbIndex++;
                    }
                    else
                    {
                        SoundManager.Instance.PlaySound("ERROR_CLICK");
                    }
                }
            }
        }

		public void UpgradeGoldMult()
		{
            if (!FTUEManager.Instance.IsActivated)
            {
                if (_isGoldTrans)
                {
                    int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.GoldMult, _playerProfile.CurrentFortress.UTPGoldMultIndex);
                    if (_playerProfile.Gold >= cost)
                    {
                        SoundManager.Instance.PlaySound("BUY_CLICK");
                        _playerProfile.Gold -= cost;
                        _playerProfile.CurrentFortress.UTPGoldMultIndex++;
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
                        _playerProfile.CurrentFortress.UTPGoldMultIndex++;
                    }
                    else
                    {
                        SoundManager.Instance.PlaySound("ERROR_CLICK");
                    }
                }
            }
        }

		public void UpgradeWinBeerChance()
		{
            if (!FTUEManager.Instance.IsActivated)
            {
                if (_playerProfile.CurrentFortress.UTPWinBeerChanceIndex < DatabaseManager.Instance.TradingPostUpgrades.WinBeerChance.max)
                {
                    if (_isGoldTrans)
                    {
                        int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerChance, _playerProfile.CurrentFortress.UTPWinBeerChanceIndex);
                        if (_playerProfile.Gold >= cost)
                        {
                            SoundManager.Instance.PlaySound("BUY_CLICK");
                            _playerProfile.Gold -= cost;
                            _playerProfile.CurrentFortress.UTPWinBeerChanceIndex++;
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
                            _playerProfile.CurrentFortress.UTPGoldMultIndex++;
                        }
                        else
                        {
                            SoundManager.Instance.PlaySound("ERROR_CLICK");
                        }
                    }
                }
            }
        }

		public void UpgradeWinBeerAmount()
		{
            if (!FTUEManager.Instance.IsActivated)
            {
                if (_isGoldTrans)
                {
                    int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.TradingPostUpgrades.WinBeerAmount, _playerProfile.CurrentFortress.UTPWinBeerAmountIndex);
                    if (_playerProfile.Gold >= cost)
                    {
                        SoundManager.Instance.PlaySound("BUY_CLICK");
                        _playerProfile.Gold -= cost;
                        _playerProfile.CurrentFortress.UTPWinBeerAmountIndex++;
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
                        _playerProfile.CurrentFortress.UTPWinBeerAmountIndex++;
                    }
                    else
                    {
                        SoundManager.Instance.PlaySound("ERROR_CLICK");
                    }
                }
            }
        }
		#endregion Upgrades

		#region Loop
		private void Loop()
		{
			if (_isPaused)
			{
				if (!_timer.IsStopped)
				{
					_timer.Stop();
				}
			}
			else
			{
				if (ComputeWeaponToSell())
				{
					ComputeToSellData();
					if (_timer.IsStopped == true)
					{
						ResetTimer();
					}
					else if (_timer.IsTimerEnd())
					{
						_timer.Stop();

						//Normal Trading

						_converter.TradingPostConverter(_weaponToSell, _weaponToSellAmount, _goldToProduce);

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
		}
		#endregion Loop

		#region Timer Management
		private void ResetTimer()
		{
            if (_playerProfile._bonusTimeRemaining > 0)
            {
                float cycleDuration = _cycleDuration / DatabaseManager.Instance.ConsumableBonusData.ProductionSpeeMult;
                _timer.ResetTimer(cycleDuration);
            }
            else
            {
                _timer.ResetTimer(_cycleDuration);
            }
			_timer.IsStopped = false;
		}

		public override void Poltering()
		{
			if (!_timer.IsStopped)
			{
				_timer.ReduceRemainingTime(DatabaseManager.Instance.PolteringValue);
				_FXController.CreatePolteringParticle();
                SoundManager.Instance.PlayRandomSound("POLTERING_TRADINGPOST");
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

        public void TriggerHighlight()
        {
            FTUEManager.Instance.SetNewHighlight();
        }

		private void HandleFortressChange()
		{
			LoadData();
			_timer.Stop();
			_playerProfile.CurrentFortress.OnTPUpgradeChange += OnUpgrade;
		}

		private void SetPause(bool isPaused)
		{
			_playerProfile.CurrentFortress.TradingPostIsPaused = isPaused;
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

		private void ComputeToSellData(bool computeReal = true)
		{
			int _realToSellAmount = _weaponToSellAmount;
			if (computeReal)
			{
				if (_weaponToSellAmount > _playerProfile.Weapons[_weaponToSell.Name].Count)
				{
					_realToSellAmount = _weaponToSellAmount - (_weaponToSellAmount - _playerProfile.Weapons[_weaponToSell.Name].Count);
				}
			}
			_goldToProduce = (int)(_playerProfile.Weapons[_weaponToSell.Name].SellPrice * _realToSellAmount * _goldMultiplier);
		}
		#endregion Utils
		#endregion Methods
	}
}