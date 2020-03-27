namespace DwarkClicker.Helper
{
	using DwarfClicker.Core.Achievement;
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using Engine.Manager;
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class IdleComputeHelper
	{
		public static void ComputeMineProgression(DatabaseManager db, PlayerProfile playerProfile, FortressProfile fortress, TimeSpan timeElapsed, bool isBonused = false)
		{
            float cycleMDiv = 1;
            if (isBonused)
            {
                cycleMDiv = DatabaseManager.Instance.ConsumableBonusData.ProductionSpeeMult;
            }

            float miningTime = db.MineStats.CycleDuration;
			for (int i = 0; i < fortress.MineUpgradesIndex._cycleDurationIndex; i++)
			{
				miningTime -= miningTime * db.MineUpgrades.CycleDuration.value;
			}
            miningTime /= cycleMDiv;

            float beerCostbyWorker = db.MineStats.BeerConsumption;
			for (int i = 0; i < fortress.UMineBeerConsoIndex; i++)
			{
				beerCostbyWorker -= beerCostbyWorker * db.MineUpgrades.BeerConsumption.value;
			}

			int workerNb = db.MineStats.WorkerAmount + db.MineUpgrades.WorkerAmount.value * fortress.UMineBeerConsoIndex;
			float beerCostbyCycle = workerNb * beerCostbyWorker;
			int cycleNb = (int)(timeElapsed.TotalSeconds / miningTime);
			int realCycleNb = (int)(fortress.Beer / beerCostbyCycle);
			if (cycleNb < realCycleNb)
				realCycleNb = cycleNb;

			fortress.Beer -= (int)(realCycleNb * beerCostbyCycle);

			int resByWorker = db.MineStats.ResByWorker + db.MineUpgrades.ResByWorker.value * fortress.MineUpgradesIndex._resByWorkerIndex;
			int resGain = realCycleNb * resByWorker * workerNb;

			string resourceKey = fortress.ResourceProduced.Name;
            float trueResGain = resGain * playerProfile._resourcesMultiplierBonus;
            playerProfile.Resources[resourceKey].UpdateCount((int)trueResGain);
			AchievementManager.Instance.UpdateAchievement("MINED_AMOUNT", (int)trueResGain);
			GameManager.Instance.ProgressionInventory.SetProducedResource(resourceKey, resGain);

			// Rich Vein

			int luckCounter = 0;
			int richVein = db.MineStats.RichVein + db.MineUpgrades.RichVein.value * fortress.MineUpgradesIndex._richVeinIndex;
			int luck = db.MineStats.Luck - db.MineUpgrades.Luck.value * fortress.MineUpgradesIndex._luckIndex;
			luckCounter = (int)(realCycleNb / luck);
            int gain = (int)(richVein * luckCounter * playerProfile._resourcesMultiplierBonus);
            playerProfile.Resources[resourceKey].UpdateCount(gain);
			AchievementManager.Instance.UpdateAchievement("MINED_AMOUNT", gain);
			GameManager.Instance.ProgressionInventory.SetProducedResource(resourceKey, gain);

			// Rich Vein

			// Mithril

			int mithrilChance = db.MineStats.Mithril - db.MineUpgrades.Mithril.value * fortress.MineUpgradesIndex._mithrilChanceIndex;

			for (int i = 0; i < realCycleNb; i++)
			{
				if (UnityEngine.Random.Range(0, mithrilChance) == 0)
				{
					playerProfile.Mithril++;
					GameManager.Instance.ProgressionInventory.SetMithril(1);
				}
			}

			// Mithril
		}

		public static void ComputeForgeProgression(DatabaseManager db, PlayerProfile playerProfile, FortressProfile fortress, TimeSpan timeElapsed, bool isBonused = false)
		{
            float cycleDiv = 1;
            if (isBonused)
            {
                cycleDiv = DatabaseManager.Instance.ConsumableBonusData.ProductionSpeeMult;
            }

            float forgingTime = db.ForgeStats.CycleDuration;
			for (int i = 0; i <= fortress.ForgeUpgradesIndex._cycleDurationIndex; i++)
			{
				forgingTime -= forgingTime * db.ForgeUpgrades.CycleDuration.value;
			}
            forgingTime /= cycleDiv;

			int forgeWorkerNb = db.ForgeStats.WorkerAmount + db.ForgeUpgrades.WorkerAmount.value * fortress.UForgeWorkerNbIndex;
			int wByWorker = db.ForgeStats.WByWorker + db.ForgeUpgrades.WByWorker.value * fortress.UForgeWByWorkerIndex;
			int resCostByCycle = forgeWorkerNb * wByWorker * playerProfile.Weapons[fortress.CurrentCraft.Name].Recipie[0].Count;

			int forgeCycleNb = (int)(timeElapsed.TotalSeconds / forgingTime);
			int forgeRealCycleNb = (int)(playerProfile.Resources[fortress.ResourceProduced.Name].Count / resCostByCycle);
			if (forgeCycleNb < forgeRealCycleNb)
				forgeRealCycleNb = forgeCycleNb;

			int weaponProduced = forgeRealCycleNb * wByWorker * forgeWorkerNb;
			int resourceConsumed = -weaponProduced * playerProfile.Weapons[fortress.CurrentCraft.Name].Recipie[0].Count;
			playerProfile.Resources[fortress.ResourceProduced.Name].UpdateCount(resourceConsumed);
			
			GameManager.Instance.ProgressionInventory.SetConsumedResource(fortress.ResourceProduced.Name, resourceConsumed);

			// Instant Selling

			int instantSellingCounter = 0;
			int instantSellingChance = db.ForgeStats.InstantSellingChance - db.ForgeUpgrades.InstantSellingChance.value * fortress.UForgeInstantSellingChanceIndex;
			float instantSellingGoldBonus = db.ForgeStats.InstantSellingGoldBonus + db.ForgeUpgrades.InstantSellingGoldBonus.value * fortress.UForgeInstantSellingGoldBonusIndex;
			instantSellingCounter = (int)(forgeRealCycleNb / instantSellingChance);
			int goldGain = (int)(instantSellingCounter * playerProfile.Weapons[fortress.CurrentCraft.Name].SellPrice * wByWorker * forgeWorkerNb * instantSellingGoldBonus);
			playerProfile.Gold += (int)(goldGain * playerProfile._goldMultiplierBonus);
			GameManager.Instance.ProgressionInventory.SetGold(goldGain);

			// Instant Selling

			string weaponKey = fortress.CurrentCraft.Name;
			int weaponGain = weaponProduced - instantSellingCounter;
            int trueWeaponGain = (int)(weaponGain + playerProfile._toolsMultiplierBonus);
			playerProfile.Weapons[weaponKey].UpdateCount(trueWeaponGain);
			AchievementManager.Instance.UpdateAchievement("FORGED_AMOUNT", trueWeaponGain);
			GameManager.Instance.ProgressionInventory.SetProducedWeapon(weaponKey, trueWeaponGain);
		}

		public static void ComputeTradingPostProgression(DatabaseManager db, PlayerProfile playerProfile, FortressProfile fortress, TimeSpan timeElapsed, bool isBonused = false)
		{
            float cycleDiv = 1;
            if (isBonused)
            {
                cycleDiv = DatabaseManager.Instance.ConsumableBonusData.ProductionSpeeMult;
            }

            float tradingTime = db.TradingPostStats.CycleDuration;
			for (int i = 0; i <= fortress.TradingPostUpgradesIndex._cycleDurationIndex; i++)
			{
				tradingTime -= tradingTime * db.TradingPostUpgrades.CycleDuration.value;
			}
            tradingTime /= cycleDiv;

            int tradingWorkerNb = db.TradingPostStats.WorkerAmount + db.TradingPostUpgrades.WorkerAmount.value * fortress.UTPWorkerNbIndex;
			int sellByWorker = db.TradingPostStats.SellByWorker + db.TradingPostUpgrades.SellByWorker.value * fortress.UTPSellByWorkerIndex;
			int wCostByCycle = tradingWorkerNb * sellByWorker;

			int tradingCycleNb = (int)(timeElapsed.TotalSeconds / tradingTime);

			int weaponTotalAmount = 0;
			for (int i = 0; i < fortress._weapons.Length; i++)
			{
				weaponTotalAmount += playerProfile.Weapons[fortress._weapons[i].Name].Count;
			}

			int tradingRealCycleNb = (int)(weaponTotalAmount / wCostByCycle);
			if (tradingCycleNb < tradingRealCycleNb)
				tradingRealCycleNb = tradingCycleNb;

			int weaponConsumed = tradingRealCycleNb * tradingWorkerNb * sellByWorker;

			int goldProduced = 0;
			int weaponIndex = 0;
			int weaponConsumedIt = 0;

			WeaponData[] weapons = fortress._weapons;

			while (weaponConsumedIt < weaponConsumed)
			{
				if (playerProfile.Weapons[weapons[weaponIndex].Name].Count > 0)
				{
					goldProduced += playerProfile.Weapons[weapons[weaponIndex].Name].SellPrice;
					playerProfile.Weapons[weapons[weaponIndex].Name].UpdateCount(-1);
					AchievementManager.Instance.UpdateAchievement("SELL_AMOUNT", 1);
					GameManager.Instance.ProgressionInventory.SetConsumedWeapon(weapons[weaponIndex].Name, -1);
					weaponConsumedIt++;
					for (int i = 0; i < weapons.Length; i++)
					{
						playerProfile.Weapons[weapons[i].Name].ModTimer++;
					}
				}
				else
				{
					weaponIndex++;
				}
			}

			playerProfile.Gold += (int)(goldProduced * playerProfile._goldMultiplierBonus);
			GameManager.Instance.ProgressionInventory.SetGold(goldProduced);
			
			// Update ModTimer
			for (int i = 0; i < weapons.Length; i++)
			{
				while (playerProfile.Weapons[weapons[i].Name].ModTimer >= weapons[i].ModifierTimer)
				{
					if (weapons[i].Name == playerProfile.CurrentFortress.CurrentCraft.Name)
					{
						playerProfile.Weapons[weapons[i].Name].SellPrice -= weapons[i].PriceModifierDown;
						playerProfile.Weapons[weapons[i].Name].SellPrice = Mathf.Clamp(playerProfile.Weapons[weapons[i].Name].SellPrice, weapons[i].PriceMin, weapons[i].PriceMax);
					}
					else
					{
						playerProfile.Weapons[weapons[i].Name].SellPrice += weapons[i].PriceModifierUp;
						playerProfile.Weapons[weapons[i].Name].SellPrice = Mathf.Clamp(playerProfile.Weapons[weapons[i].Name].SellPrice, weapons[i].PriceMin, weapons[i].PriceMax);
					}
					playerProfile.Weapons[weapons[i].Name].ModTimer -= weapons[i].ModifierTimer;
				}
			}

			// Win Beer

			int winBeerCounter = 0;
			int winBeerChance = db.TradingPostStats.WinBeerChance - db.TradingPostUpgrades.WinBeerChance.value * fortress.UTPWinBeerChanceIndex;
			float winBeerAmount = db.TradingPostStats.WinBeerAmount + db.TradingPostUpgrades.WinBeerAmount.value * fortress.UTPWinBeerAmountIndex;
			winBeerCounter = (int)(tradingRealCycleNb / winBeerChance);
			fortress.Beer += (winBeerCounter * winBeerAmount);
		}
	}
}