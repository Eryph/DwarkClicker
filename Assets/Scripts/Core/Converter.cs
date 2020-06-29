namespace DwarfClicker.Core
{
	using DwarfClicker.Core.Achievement;
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
    using DwarkClicker.Helper;
    using Engine.Manager;
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Converter : MonoBehaviour
	{
		#region Fields
		private PlayerProfile _profile = null;
		#endregion Fields

		#region Events
		
		#endregion Events

		#region Methods
		#region Monobehaviour
		private void Start()
		{
			_profile = JSonManager.Instance.PlayerProfile;
		}
		#endregion Monobehaviour

		#region Data Updates
		public void UpdateMithril(int incr)
		{
			_profile.Mithril += incr;
		}

		public void UpdateBeer()
		{
			_profile.CurrentFortress.Beer++;
		}

		public void UpdateBeer(int incr)
		{
			_profile.CurrentFortress.Beer += incr;
		}

		public void UpdateGold()
		{
			_profile.Gold++;
		}

		public void UpdateGold(ulong incr)
		{
			_profile.Gold += incr;
		}
		#endregion Data Updates

		#region Gain
		public void GainBeer(int amount)
		{
			_profile.CurrentFortress.Beer += amount;
		}

		public void GainMithril(int amount)
		{
			_profile.Mithril += amount;
		}

		public void GainResource(string resourceName, int amount)
		{
			_profile.Resources[resourceName].UpdateCount(amount);
		}

		public void GainGold(ulong amount)
		{
			_profile.Gold += amount;
		}
		#endregion Gain

		#region Convert
		public void MineConverter(ResourceData resource, float beerCost, int resGain)
		{
            float trueResGain = resGain * _profile._resourcesMultiplierBonus;
			_profile.CurrentFortress.Beer -= beerCost;
			_profile.Resources[resource.Name].UpdateCount((int)trueResGain);
			AchievementManager.Instance.UpdateAchievement("MINED_AMOUNT", (ulong)trueResGain);
			_profile.TriggerInventoryChangeEvent();

		}

		public void ForgeConverter(int resourceToConsumed, WeaponData _toCraft, int nbToCraft)
		{
            float trueToolGain = nbToCraft * _profile._toolsMultiplierBonus;
            _profile.Resources[_toCraft.Recipe[0].Key].UpdateCount(-resourceToConsumed);
			_profile.Weapons[_toCraft.Name].UpdateCount((int)trueToolGain);
			AchievementManager.Instance.UpdateAchievement("FORGED_AMOUNT", (ulong)trueToolGain);
			_profile.TriggerInventoryChangeEvent();
		}

		public void ForgeConverterInstantSelling(WeaponData _toCraft, int nbToCraft, float goldBonus)
		{
            float trueToolGain = nbToCraft * _profile._toolsMultiplierBonus;
            for (int i = 0; i < _toCraft.Recipe.Length; i++)
			{
				_profile.Resources[_toCraft.Recipe[i].Key].UpdateCount(-_toCraft.Recipe[i].Count * nbToCraft);
			}
			AchievementManager.Instance.UpdateAchievement("FORGED_AMOUNT", (ulong)trueToolGain);
			UpdateGold((ulong)(_toCraft.GoldValue * goldBonus * _profile._goldMultiplierBonus));
			
			_profile.TriggerInventoryChangeEvent();
		}

		public void TradingPostConverter(WeaponData _toSell, int nbToSell, int goldToProduce)
		{
			nbToSell = Mathf.Clamp(nbToSell, 0, int.MaxValue);
			_profile.Weapons[_toSell.Name].UpdateCount(-nbToSell);


			_profile.Gold += (ulong)(goldToProduce * _profile._goldMultiplierBonus);
			_profile.TriggerInventoryChangeEvent();

			//Remanent Market Computing
			WeaponData[] weapons = _profile.CurrentFortress._weapons;
			for (int i = 0; i < weapons.Length; i++)
			{
				_profile.Weapons[weapons[i].Name].ModTimer++;
				if (_profile.Weapons[weapons[i].Name].ModTimer >= weapons[i].ModifierTimer)
				{
					if (weapons[i].Name == _profile.CurrentFortress.CurrentCraft.Name)
					{
						_profile.Weapons[weapons[i].Name].SellPrice -= weapons[i].PriceModifierDown;
						_profile.Weapons[weapons[i].Name].SellPrice = UIHelper.LongClamp(_profile.Weapons[weapons[i].Name].SellPrice, weapons[i].PriceMin, weapons[i].PriceMax);
					}
					else
					{
						_profile.Weapons[weapons[i].Name].SellPrice += weapons[i].PriceModifierUp;
						_profile.Weapons[weapons[i].Name].SellPrice = UIHelper.LongClamp(_profile.Weapons[weapons[i].Name].SellPrice, weapons[i].PriceMin, weapons[i].PriceMax);
					}
					_profile.Weapons[weapons[i].Name].ModTimer = 0;
					
				}
			}
			AchievementManager.Instance.UpdateAchievement("SELL_AMOUNT", (ulong)nbToSell);
		}
		#endregion Convert

		#region Compute Upgrades
		public ulong ComputeUpgradeCost(IntUpgrade up, int rank)
		{
			ulong cost = up.cost;
			for (int i= 0; i < rank; i++)
			{
				cost = (ulong)(cost * up.coef);
			}
			return cost;
		}

		public ulong ComputeUpgradeCost(FloatUpgrade up, int rank)
		{
			ulong cost = up.cost;
			for (int i = 0; i < rank; i++)
			{
				cost = (ulong)(cost * up.coef);
			}
			return cost;
		}
		#endregion Compute Upgrades
		#endregion Methods
	}
}