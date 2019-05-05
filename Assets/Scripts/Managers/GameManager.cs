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

	public class GameManager : Singleton<GameManager>
	{
		#region Fields
		[SerializeField] private string _firstSceneToLoadPath = "Assets/Scenes/MainMenu.unity";

		private PlayerProfile _playerProfile = null;
		private DatabaseManager _db = null;
		#endregion Fields

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
			JSonManager.Instance.OnProfileLoaded -= LoadData;
		}

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

				IdleComputeHelper.ComputeMineProgression(_db, _playerProfile, fortress, timeElapsed);
				IdleComputeHelper.ComputeForgeProgression(_db, _playerProfile, fortress, timeElapsed);
				IdleComputeHelper.ComputeTradingPostProgression(_db, _playerProfile, fortress, timeElapsed);
				/*// Mine Processing

				float miningTime = _db.MineStats.CycleDuration;
				for (int i = 0; i < fortress.MineUpgradesIndex._cycleDurationIndex; i++)
				{
					miningTime -= miningTime * _db.MineUpgrades.CycleDuration.value;
				}

				float beerCostbyWorker = _db.MineStats.BeerConsumption;
				for (int i = 0; i < fortress.UMineBeerConsoIndex; i++)
				{
					beerCostbyWorker -= beerCostbyWorker * _db.MineUpgrades.BeerConsumption.value;
				}

				int workerNb = _db.MineStats.WorkerNb + _db.MineUpgrades.WorkerNb.value * fortress.UMineBeerConsoIndex;
				float beerCostbyCycle = workerNb * beerCostbyWorker;
				int cycleNb = (int)(timeElapsed.TotalSeconds / miningTime);
				int realCycleNb = (int)(fortress.Beer / beerCostbyCycle);
				if (cycleNb < realCycleNb)
					realCycleNb = cycleNb;

				fortress.Beer -= (int)(realCycleNb * beerCostbyCycle);

				int resByWorker = _db.MineStats.ResByWorker + _db.MineUpgrades.ResByWorker.value * fortress.MineUpgradesIndex._resByWorkerIndex;
				int resGain = realCycleNb * resByWorker * workerNb;

				_playerProfile.Resources[fortress.ResourceProduced.Name].UpdateCount(resGain);

				// Rich Vein

				int luckCounter = 0;
				int richVein = _db.MineStats.RichVein + _db.MineUpgrades.RichVein.value * fortress.MineUpgradesIndex._richVeinIndex;
				int luck = _db.MineStats.Luck - _db.MineUpgrades.Luck.value * fortress.MineUpgradesIndex._luckIndex;
				luckCounter = (int)(realCycleNb / luck);
				_playerProfile.Resources[fortress.ResourceProduced.Name].UpdateCount(richVein * luckCounter);

				// Rich Vein

				// Mithril

				int mithrilChance = _db.MineStats.Mithril - _db.MineUpgrades.Mithril.value * fortress.MineUpgradesIndex._mithrilChanceIndex;

				for (int i = 0; i < realCycleNb; i++)
				{
					if (UnityEngine.Random.Range(0, mithrilChance) == 0)
					{
						_playerProfile.Mithril++;
					}
				}

				// Mithril

				// Mine Processing*/


				/*// Forge Processing

				float forgingTime = _db.ForgeStats.CycleDuration;
				for (int i = 0; i <= fortress.ForgeUpgradesIndex._cycleDurationIndex; i++)
				{
					forgingTime -= forgingTime * _db.ForgeUpgrades.CycleDuration.value;
				}

				int forgeWorkerNb = _db.ForgeStats.WorkerNb + _db.ForgeUpgrades.WorkerNb.value * fortress.UForgeWorkerNbIndex;
				int wByWorker = _db.ForgeStats.WByWorker + _db.ForgeUpgrades.WByWorker.value * fortress.UForgeWByWorkerIndex;
				int resCostByCycle = forgeWorkerNb * wByWorker * _playerProfile.Weapons[fortress.CurrentCraft.Name].Recipie[0].Count;

				int forgeCycleNb = (int)(timeElapsed.TotalSeconds / forgingTime);
				int forgeRealCycleNb = (int)(_playerProfile.Resources[fortress.ResourceProduced.Name].Count / resCostByCycle);
				if (forgeCycleNb < forgeRealCycleNb)
					forgeRealCycleNb = forgeCycleNb;

				int weaponProduced = forgeRealCycleNb * wByWorker * forgeWorkerNb;
				_playerProfile.Resources[fortress.ResourceProduced.Name].UpdateCount(-weaponProduced * _playerProfile.Weapons[fortress.CurrentCraft.Name].Recipie[0].Count);

				// Instant Selling

				int instantSellingCounter = 0;
				int instantSellingChance = _db.ForgeStats.InstantSellingChance - _db.ForgeUpgrades.InstantSellingChance.value * fortress.UForgeInstantSellingChanceIndex;
				float instantSellingGoldBonus = _db.ForgeStats.InstantSellingGoldBonus + _db.ForgeUpgrades.InstantSellingGoldBonus.value * fortress.UForgeInstantSellingGoldBonusIndex;
				instantSellingCounter = (int)(forgeRealCycleNb / instantSellingChance);
				_playerProfile.Gold += (int)(instantSellingCounter * _playerProfile.Weapons[fortress.CurrentCraft.Name].SellPrice * wByWorker * forgeWorkerNb * instantSellingGoldBonus);

				// Instant Selling

				_playerProfile.Weapons[fortress.CurrentCraft.Name].UpdateCount(weaponProduced - instantSellingCounter);

				// Forge Processing*/


				/*// Trading Post Processing

				float tradingTime = _db.TradingPostStats.CycleDuration;
				for (int i = 0; i <= fortress.TradingPostUpgradesIndex._cycleDurationIndex; i++)
				{
					tradingTime -= tradingTime * _db.TradingPostUpgrades.CycleDuration.value;
				}

				int tradingWorkerNb = _db.TradingPostStats.WorkerNb + _db.TradingPostUpgrades.WorkerNb.value * fortress.UTPWorkerNbIndex;
				int sellByWorker = _db.TradingPostStats.SellByWorker + _db.TradingPostUpgrades.SellByWorker.value * fortress.UTPSellByWorkerIndex;
				int wCostByCycle = tradingWorkerNb * sellByWorker;

				int tradingCycleNb = (int)(timeElapsed.TotalSeconds / tradingTime);
				int tradingRealCycleNb = (int)(_playerProfile.Weapons[fortress.CurrentCraft.Name].Count / wCostByCycle);
				if (tradingCycleNb < tradingRealCycleNb)
					tradingRealCycleNb = tradingCycleNb;

				int weaponConsumed = tradingRealCycleNb * tradingWorkerNb * sellByWorker;
				int goldProduced = weaponConsumed * _playerProfile.Weapons[fortress.CurrentCraft.Name].SellPrice;
				_playerProfile.Weapons[fortress.CurrentCraft.Name].UpdateCount(-weaponConsumed);
				_playerProfile.Gold += goldProduced;

				// Win Beer

				int winBeerCounter = 0;
				int winBeerChance = _db.TradingPostStats.WinBeerChance - _db.TradingPostUpgrades.WinBeerChance.value * fortress.UTPWinBeerChanceIndex;
				float winBeerAmount = _db.TradingPostStats.WinBeerAmount + _db.TradingPostUpgrades.WinBeerAmount.value * fortress.UTPWinBeerAmountIndex;
				winBeerCounter = (int)(tradingRealCycleNb / winBeerChance);
				fortress.Beer += (winBeerCounter * winBeerAmount);

				// Win Beer

				// Trading Post Processing*/
			}

			JSonManager.Instance.SavePlayerProfile();
		}

		private void OnApplicationQuit()
		{
			_playerProfile.LaunchAmount = _playerProfile.LaunchAmount + 1;
			_playerProfile.SerializeDate(DateTime.Now);
			JSonManager.Instance.SavePlayerProfile();
			JSonManager.Instance.SaveNotifProfile();
		}

#if ANDROID
		private void OnApplicationPause()
		{
			_playerProfile.LaunchAmount = _playerProfile.LaunchAmount + 1;
			_playerProfile.SerializeDate(DateTime.Now);


			DeviceManager.Instance.PushLocalNotification("TestNotification", "Notification triggered, you won", 0.5f);


		JSonManager.Instance.SavePlayerProfile();
			JSonManager.Instance.SaveNotifProfile();
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
