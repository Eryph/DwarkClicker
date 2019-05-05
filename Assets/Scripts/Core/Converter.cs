namespace Core
{
	using Core.Containers;
	using Core.Data;
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

		public void UpdateGold(int incr)
		{
			_profile.Gold += incr;
		}
		#endregion Data Updates

		#region Convert
		public void MineConverter(ResourceData resource, float beerCost, int resGain)
		{
			_profile.CurrentFortress.Beer -= beerCost;
			_profile.Resources[resource.Name].UpdateCount(resGain);
			_profile.TriggerInventoryChangeEvent();
			JSonManager.Instance.SavePlayerProfile();
		}

		public void ForgeConverter(WeaponData _toCraft, int nbToCraft)
		{
			for (int i = 0; i < _toCraft.Recipe.Length; i++)
			{
				_profile.Resources[_toCraft.Recipe[i].Key].UpdateCount(-_toCraft.Recipe[i].Count * nbToCraft);
			}
			_profile.Weapons[_toCraft.Name].UpdateCount(nbToCraft);
			_profile.TriggerInventoryChangeEvent();
			JSonManager.Instance.SavePlayerProfile();
		}

		public void ForgeConverterInstantSelling(WeaponData _toCraft, int nbToCraft, float goldBonus)
		{
			for (int i = 0; i < _toCraft.Recipe.Length; i++)
			{
				_profile.Resources[_toCraft.Recipe[i].Key].UpdateCount(-_toCraft.Recipe[i].Count * nbToCraft);
			}
			UpdateGold((int)(_toCraft.GoldValue * goldBonus));
			_profile.TriggerInventoryChangeEvent();
			JSonManager.Instance.SavePlayerProfile();
		}

		public void TradingPostConverter(WeaponData _toSell, int nbToSell, float goldMult)
		{
			int totalToSell = 0;
			if (nbToSell > _profile.Weapons[_toSell.Name].Count)
			{
				totalToSell = nbToSell - (nbToSell - _profile.Weapons[_toSell.Name].Count);
			}
			else
			{
				totalToSell = nbToSell;
			}
			totalToSell = Mathf.Clamp(totalToSell, 0, int.MaxValue);
			_profile.Weapons[_toSell.Name].UpdateCount(-totalToSell);

			_profile.Gold += (int)(_profile.Weapons[_toSell.Name].SellPrice * totalToSell * goldMult);
			_profile.TriggerInventoryChangeEvent();

			WeaponData[] weapons = _profile.CurrentFortress._weapons;
			for (int i = 0; i < weapons.Length; i++)
			{
				_profile.Weapons[weapons[i].Name].ModTimer++;
				if (_profile.Weapons[weapons[i].Name].ModTimer >= weapons[i].ModifierTimer)
				{
					if (weapons[i].Name == _profile.CurrentFortress.CurrentCraft.Name)
					{
						_profile.Weapons[weapons[i].Name].SellPrice -= weapons[i].PriceModifierDown;
						_profile.Weapons[weapons[i].Name].SellPrice = Mathf.Clamp(_profile.Weapons[weapons[i].Name].SellPrice, weapons[i].PriceMin, weapons[i].PriceMax);
					}
					else
					{
						_profile.Weapons[weapons[i].Name].SellPrice += weapons[i].PriceModifierUp;
						_profile.Weapons[weapons[i].Name].SellPrice = Mathf.Clamp(_profile.Weapons[weapons[i].Name].SellPrice, weapons[i].PriceMin, weapons[i].PriceMax);
					}
					_profile.Weapons[weapons[i].Name].ModTimer = 0;
				}
			}
		}
		#endregion Convert

		#region Compute Upgrades
		public int ComputeUpgradeCost(IntUpgrade up, int rank)
		{
			int cost = up.cost;
			for (int i= 0; i < rank; i++)
			{
				cost = (int)(cost * up.coef);
			}
			return cost;
		}

		public int ComputeUpgradeCost(FloatUpgrade up, int rank)
		{
			int cost = up.cost;
			for (int i = 0; i < rank; i++)
			{
				cost = (int)(cost * up.coef);
			}
			return cost;
		}
		#endregion Compute Upgrades
		#endregion Methods
	}
}