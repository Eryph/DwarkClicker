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
		#endregion Events

		#region Methods
		protected override void Start()
		{
			base.Start();
			_db = DatabaseManager.Instance;
			JSonManager.Instance.OnProfileLoaded += LoadData;
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

		private void LoadData()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			if (_playerProfile.LaunchAmount > 0)
				LoadProgression();
		}

		#region Progression Load
		public void LoadProgression(bool computeCurrent = false)
		{
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
				

				if (fortress.MineIsPaused == false)
				{
					IdleComputeHelper.ComputeMineProgression(_db, _playerProfile, fortress, timeElapsed);
				}

				if (fortress.ForgeIsPaused == false)
				{
					IdleComputeHelper.ComputeForgeProgression(_db, _playerProfile, fortress, timeElapsed);
				}

				if (fortress.TradingPostIsPaused == false)
				{
					IdleComputeHelper.ComputeTradingPostProgression(_db, _playerProfile, fortress, timeElapsed);
				}
			}
		}
		#endregion Progression Load

		private void OnApplicationQuit()
		{
			_playerProfile.LaunchAmount = _playerProfile.LaunchAmount + 1;
			_playerProfile.SerializeDate(DateTime.Now);
			JSonManager.Instance.SavePlayerProfile();
			JSonManager.Instance.SaveNotifProfile();
		}

#if ANDROID
		private void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				_playerProfile.LaunchAmount = _playerProfile.LaunchAmount + 1;
				_playerProfile.SerializeDate(DateTime.Now);
				DeviceManager.Instance.PushLocalNotification("The dwarfs thirsty !", "Beer is running low ! Come back and brew some beers.", 24f);
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
				DeviceManager.Instance.PushLocalNotification("The dwarfs thirsty !", "Beer is running low ! Come back and brew some beers.", 24f);
				JSonManager.Instance.SavePlayerProfile();
				JSonManager.Instance.SaveNotifProfile();
			}
		}
#endif

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
#endregion Methods
	}
}
