namespace Engine.Manager
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Engine.Utils;
	using UnityEngine.SceneManagement;
	using UnityEngine.Assertions;
	using System.IO;
	using DwarkClicker.Helper;
	using DwarfClicker.UI.GainRecap;
	using DwarfClicker.Misc;
	using DwarfClicker.Core.Containers;

	public class GameManager : Singleton<GameManager>
	{
		#region Fields
		[SerializeField] private string _firstSceneToLoadPath = "Assets/Scenes/MainMenu.unity";

		private ProgressionLoadInventory _progressionInventory;
		private PlayerProfile _playerProfile = null;
		private DatabaseManager _db = null;
		#endregion Fields

		#region Properties
		public ProgressionLoadInventory ProgressionInventory { get { return _progressionInventory; } }
        #endregion Properties

        #region Events
        private event Action _onProgressionLoad = null;
        public event Action OnProgressionLoad
        {
            add
            {
                _onProgressionLoad -= value;
                _onProgressionLoad += value;
            }
            remove
            {
                _onProgressionLoad -= value;
            }
        }
		#endregion Events

		#region Methods
		#endregion Monobehaviour
		protected override void Start()
		{
			base.Start();
			_db = DatabaseManager.Instance;
			JSonManager.Instance.OnProfileLoaded += LoadData;
			//MonetizationManager.Instance.OnSDKReady += LoadGameScene;
			int buildIndex = SceneUtility.GetBuildIndexByScenePath(_firstSceneToLoadPath);
           
            if (buildIndex > 0)
			{
				SceneManager.LoadScene(buildIndex);
			}
			else
			{
				Debug.LogError("First scene to load \"" + _firstSceneToLoadPath + "\" was not found.");
			}
		}

		private void LoadGameScene()
		{
            MonetizationManager.Instance.OnSDKReady -= LoadGameScene;
			int buildIndex = SceneUtility.GetBuildIndexByScenePath(_firstSceneToLoadPath);
			if (buildIndex > 0)
			{
				SceneManager.LoadScene(buildIndex);
			}
			else
			{
				Debug.LogError("First scene to load \"" + _firstSceneToLoadPath + "\" was not found.");
			}
		}
		#region Monobehaviour

		#region Progression Load
		public void LoadProgression(bool computeCurrent = false)
		{
			/*if (_playerProfile._kingTask != null && _playerProfile._kingTask.GoalSprite == null)
				_playerProfile._kingTask = null;*/

			DateTime currentDate = DateTime.Now;
			DateTime prevDate;
			if (computeCurrent == true)
			{
				prevDate = _playerProfile.FortressDate;
			}
			else
			{
				prevDate = _playerProfile.Date;
			}
			_playerProfile.FortressDate = currentDate;
			TimeSpan timeElapsed = currentDate.Subtract(prevDate);

			for (int fIndex = 0; fIndex < _playerProfile.Fortress.Count; fIndex++)
			{
				FortressProfile fortress = _playerProfile.Fortress[fIndex];


				if (fortress._isBought == false)
				{
					continue;
				}
				
				if (computeCurrent == true && fIndex == _playerProfile.CurrentFortressIndex)
				{
					continue;
				}

				_progressionInventory = new ProgressionLoadInventory();
				_progressionInventory.Init();
				_progressionInventory.SetTimePassed(timeElapsed);

                if (timeElapsed.TotalSeconds < _playerProfile._bonusTimeRemaining)
                {
                    _playerProfile._bonusTimeRemaining -= (float)timeElapsed.TotalSeconds;
                    IdleProgression(fortress, timeElapsed, true);
                }
                else
                {
                    if (_playerProfile._bonusTimeRemaining > 0)
                    {
                        int bonusTimeElapsed = (int)(timeElapsed.TotalSeconds - _playerProfile._bonusTimeRemaining);
                        TimeSpan bonusTimeSpan = new TimeSpan(0, 0, bonusTimeElapsed);
                        IdleProgression(fortress, bonusTimeSpan, true);
                        timeElapsed -= bonusTimeSpan;
                    }
                    IdleProgression(fortress, timeElapsed);
                    _playerProfile._bonusTimeRemaining = 0;
                }

            }
            if (_onProgressionLoad != null)
                _onProgressionLoad();
		}

        private void IdleProgression(FortressProfile fortress, TimeSpan timeElapsed, bool isBonused = false)
        {
            if (fortress.MineIsPaused == false)
            {
                IdleComputeHelper.ComputeMineProgression(_db, _playerProfile, fortress, timeElapsed, isBonused);
            }

            if (fortress.ForgeIsPaused == false)
            {
                IdleComputeHelper.ComputeForgeProgression(_db, _playerProfile, fortress, timeElapsed, isBonused);
            }

            if (fortress.TradingPostIsPaused == false)
            {
                IdleComputeHelper.ComputeTradingPostProgression(_db, _playerProfile, fortress, timeElapsed, isBonused);
            }
        }
		#endregion Progression Load

		#region Save/Load Triggers
		private void LoadData()
		{
            
            _playerProfile = JSonManager.Instance.PlayerProfile;
            if (FTUEManager.Instance.CurrentStep < FTUEManager.Instance.StepAmount)
            {
                JSonManager.Instance.PlayerProfile.Reset();
            }
            int i = 0;
			foreach (KeyValuePair<string, Resource> resource in _playerProfile.Resources)
			{
				resource.Value.SetSprite(DatabaseManager.Instance.ResourceList.Resources[i].ResourceSprite);
				i++;
			}
			i = 0;
			foreach (KeyValuePair<string, Weapon> weapon in _playerProfile.Weapons)
			{
				weapon.Value.SetSprite(DatabaseManager.Instance.WeaponList.Weapons[i].WeaponSprite);
				i++;
			}
					
			if (_playerProfile.LaunchAmount > 0)
				LoadProgression();
            
        }

		private void OnApplicationQuit()
		{
            
            if (_playerProfile.HasReset == false)
                _playerProfile.LaunchAmount = _playerProfile.LaunchAmount + 1;
            _playerProfile.SerializeDate(DateTime.Now);
			JSonManager.Instance.SavePlayerProfile();
			JSonManager.Instance.SaveNotifProfile();
            /*if (FTUEManager.Instance.CurrentStep < FTUEManager.Instance.StepAmount)
            {
                JSonManager.Instance.PlayerProfile.Reset();
            }*/
        }

#if ANDROID
		private void OnApplicationPause(bool pauseStatus)
		{
            if (_playerProfile == null)
            {
                _playerProfile = JSonManager.Instance.PlayerProfile;
            }

			if (pauseStatus)
			{
				_playerProfile.LaunchAmount = _playerProfile.LaunchAmount + 1;
				_playerProfile.SerializeDate(DateTime.Now);
				//float timeToNotif = ComputeTimeToNotification();
				//DeviceManager.Instance.PushLocalNotification("The dwarfs thirsty !", "Beer is running low ! Come back and brew some beers.", timeToNotif);
				JSonManager.Instance.SavePlayerProfile();
				JSonManager.Instance.SaveNotifProfile();
			}
			else
			{
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
				_playerProfile.DeserializeDate();
				LoadProgression();
			}
		}

		private void OnApplicationFocus(bool focus)
		{
			if (focus)
			{
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
				_playerProfile.DeserializeDate();
				LoadProgression();
			}
			else
			{
				_playerProfile.LaunchAmount = _playerProfile.LaunchAmount + 1;
				_playerProfile.SerializeDate(DateTime.Now);
				JSonManager.Instance.SavePlayerProfile();
				JSonManager.Instance.SaveNotifProfile();
			}
		}
#endif

		#endregion Save/Load Triggers

		#region Profile
		public void BuyFortress(int index)
		{
			if (DatabaseManager.Instance.Fortress[index].Price <= _playerProfile.Gold)
			{
				_playerProfile.Gold -= DatabaseManager.Instance.Fortress[index].Price;
				_playerProfile.Fortress[index]._isBought = true;
			}
		}
		#endregion Profile

		#region Compute Time
		private float ComputeTimeToNotification()
		{
			float beerCost = 0;
			float beerCostbyWorker = _db.MineStats.BeerConsumption;
			for (int i = 0; i < _playerProfile.CurrentFortress.MineUpgradesIndex._beerConsoIndex; i++)
			{
				beerCostbyWorker -= beerCostbyWorker * _db.MineUpgrades.BeerConsumption.value;
			}
			int workerNb = _db.MineStats.WorkerAmount + _db.MineUpgrades.WorkerAmount.value * _playerProfile.CurrentFortress.MineUpgradesIndex._workerNbIndex;

			beerCost = workerNb * beerCostbyWorker;

			float cycleDuration = _db.MineStats.CycleDuration;
			for (int i = 0; i < _playerProfile.CurrentFortress.MineUpgradesIndex._cycleDurationIndex; i++)
			{
				cycleDuration -= cycleDuration * _db.MineUpgrades.CycleDuration.value;
			}

			float cycleAmount = _playerProfile.CurrentFortress.Beer / beerCost;

			float timeToNotif = cycleAmount * cycleDuration / 360;
			return timeToNotif;
		}
		#endregion
		#endregion Methods
	}
}
