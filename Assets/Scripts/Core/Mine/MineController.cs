namespace DwarfClicker.Core
{
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
    using Engine.UI.FTUE;
    using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MineController : ABuildingBase {

        #region Fields
        // Data
        private int _workerNb = 1;
        private int _resByWorker = 1;
        private float _beerCostbyWorker = 1;
        private int _luck = 100;
        private int _richVein = 100;
        private int _mithrilChance = 1000;
        private float _cycleDuration = 4f;


		private int _luckCounter = 0;
		private float _beerCost = 1;
        private int _miningCount = 1;

		private ResourceData _resourceProduced = null;

		// Utils
		[SerializeField] private Converter _converter = null;
        [SerializeField] private DialboxController _dialboxController = null;
		private Timer _timer = null;
		private PlayerProfile _playerProfile = null;
		private DatabaseManager _db = null;
        private bool _isGoldTrans = true;
        private int _polteringFTUECount = 0;
        #endregion Fields

        #region Properties
        public float TimeLeft { get { return _timer.TimeLeft; } }
		public float CycleDuration { get { return _cycleDuration; } }
		public float BeerCost { get { return _beerCost; } }
		public int MiningCount { get { return _miningCount; } }
        public bool IsGoldTrans { get { return _isGoldTrans; } set { _isGoldTrans = value; } }
        #endregion Properties

        #region Methods
        #region Monobehaviour
        private void Start()
		{
			_timer = new Timer();
			_db = DatabaseManager.Instance;
			_playerProfile = JSonManager.Instance.PlayerProfile;
			_playerProfile.CurrentFortress.OnMineUpgradeChange += OnUpgrade;
			_playerProfile.OnFortressChange += HandleFortressChange;
			OnPause += SetPause;
			GameLoopManager.Instance.GameLoop += Loop;
			LoadData();

			if (_playerProfile.CurrentFortress.Beer >= _beerCost)
			{
				ResetTimer();
			}
			else
			{
				StopTimer();
			}
		}

		private void OnDestroy()
		{
			_playerProfile.CurrentFortress.OnMineUpgradeChange -= OnUpgrade;
			_playerProfile.OnFortressChange -= HandleFortressChange;
			GameLoopManager.Instance.GameLoop -= Loop;

			if (GameLoopManager.Instance)
			{
				GameLoopManager.Instance.GameLoop -= Loop;
				_playerProfile.CurrentFortress.OnMineUpgradeChange -= OnUpgrade;
			}
		}

        private void LoadData()
        {
			ChangeResource();
            _workerNb = _db.MineStats.WorkerAmount + _db.MineUpgrades.WorkerAmount.value * _playerProfile.CurrentFortress.MineUpgradesIndex._workerNbIndex;
			_resByWorker = (int)(_db.MineStats.ResByWorker + _db.MineUpgrades.ResByWorker.value * _playerProfile.CurrentFortress.MineUpgradesIndex._resByWorkerIndex);
			_luck = _db.MineStats.Luck - _db.MineUpgrades.Luck.value * _playerProfile.CurrentFortress.MineUpgradesIndex._luckIndex;
			_richVein = _db.MineStats.RichVein + _db.MineUpgrades.RichVein.value * _playerProfile.CurrentFortress.MineUpgradesIndex._richVeinIndex;
			_mithrilChance = _db.MineStats.Mithril - _db.MineUpgrades.Mithril.value * _playerProfile.CurrentFortress.MineUpgradesIndex._mithrilChanceIndex;

			_cycleDuration = _db.MineStats.CycleDuration;
			for (int i = 0; i < _playerProfile.CurrentFortress.MineUpgradesIndex._cycleDurationIndex; i++)
			{
				_cycleDuration -= _cycleDuration * _db.MineUpgrades.CycleDuration.value;
			}

			_beerCostbyWorker = _db.MineStats.BeerConsumption;
			for (int i = 0; i < _playerProfile.CurrentFortress.MineUpgradesIndex._beerConsoIndex; i++)
			{
				_beerCostbyWorker -= _beerCostbyWorker * _db.MineUpgrades.BeerConsumption.value;
			}

			_beerCost = _workerNb * _beerCostbyWorker;
			_miningCount = _workerNb * _resByWorker;

			_isPaused = _playerProfile.CurrentFortress.MineIsPaused;
			SetPause(_isPaused);

		}
		#endregion Monobehaviour

		#region Upgrades
		public void UpgradeBeerConso()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.BeerConsumption, _playerProfile.CurrentFortress.UMineBeerConsoIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.UMineBeerConsoIndex++;
                }
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
                    _playerProfile.CurrentFortress.UMineBeerConsoIndex++;
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }

		public void UpgradeCycleDuration()
        {
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.CycleDuration, _playerProfile.CurrentFortress.UMineCycleDurationIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.UMineCycleDurationIndex++;
                }
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
                    _playerProfile.CurrentFortress.UMineCycleDurationIndex++;
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }

		public void UpgradeLuck()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Luck, _playerProfile.CurrentFortress.UMineLuckIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.UMineLuckIndex++;
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
                    _playerProfile.CurrentFortress.UMineLuckIndex++;
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }

		public void UpgradeMithrilChance()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.Mithril, _playerProfile.CurrentFortress.UMineMithrilChanceIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.UMineMithrilChanceIndex++;
                }
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
                    _playerProfile.CurrentFortress.UMineMithrilChanceIndex++;
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }

		public void UpgradeResByWorker()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.ResByWorker, _playerProfile.CurrentFortress.UMineResByWorkerIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.UMineResByWorkerIndex++;
                }
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
                    _playerProfile.CurrentFortress.UMineResByWorkerIndex++;
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }

		public void UpgradeRichVein()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.RichVein, _playerProfile.CurrentFortress.UMineRichVeinIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.UMineRichVeinIndex++;
                }
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
                    _playerProfile.CurrentFortress.UMineRichVeinIndex++;
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
                }
            }
        }

		public void UpgradeWorkerNb()
		{
            if (_isGoldTrans)
            {
                int cost = _converter.ComputeUpgradeCost(DatabaseManager.Instance.MineUpgrades.WorkerAmount, _playerProfile.CurrentFortress.UMineWorkerNbIndex);
                if (_playerProfile.Gold >= cost)
                {
                    SoundManager.Instance.PlaySound("BUY_CLICK");
                    _playerProfile.Gold -= cost;
                    _playerProfile.CurrentFortress.UMineWorkerNbIndex++;
                }
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
                    _playerProfile.CurrentFortress.UMineWorkerNbIndex++;
                }
                else
                {
                    SoundManager.Instance.PlaySound("ERROR_CLICK");
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
				if (_playerProfile.CurrentFortress.Beer >= _beerCost)
				{
					if (_timer.IsStopped == true)
					{
						ResetTimer();
					}
					else if (_timer.IsTimerEnd())
					{
						//Normal Mining
						_timer.Stop();
						_converter.MineConverter(_resourceProduced, _beerCost, _miningCount);

						//Lucky Mining
						_luckCounter++;
						if (_luckCounter >= _luck)
						{
							_luckCounter = 0;
							_converter.MineConverter(_resourceProduced, 0, _richVein);
						}

						//Mithril Mining
						if (Random.Range(0, _mithrilChance) == 0)
						{
							_converter.UpdateMithril(1);
						}
					}
				}
				else
				{
					StopTimer();
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

		private void StopTimer()
		{
			_timer.IsStopped = true;
			_timer.Stop();
		}

		public override void Poltering()
		{
			if (!_timer.IsStopped)
			{
                if (_polteringFTUECount < 5)
                {
                    _polteringFTUECount++;
                    if (_polteringFTUECount >= 5)
                        _dialboxController.TriggerNextHiglight();
                }
				_timer.ReduceRemainingTime(DatabaseManager.Instance.PolteringValue);
				_FXController.CreatePolteringParticle();
				SoundManager.Instance.PlayRandomSound("POLTERING_MINE");
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

		private void ChangeResource()
		{
			FortressProfile fortress = _playerProfile.CurrentFortress;
			_resourceProduced = DatabaseManager.Instance.Fortress[fortress.FortressIndex].ResourceToProduce;
			
		}

		private void HandleFortressChange()
		{
			LoadData();
			ChangeResource();
			StopTimer();
			_playerProfile.CurrentFortress.OnMineUpgradeChange += OnUpgrade;
		}

		private void SetPause(bool isPaused)
		{
			_playerProfile.CurrentFortress.MineIsPaused = isPaused;
		}
		#endregion Callbacks
		#endregion Methods
	}
}