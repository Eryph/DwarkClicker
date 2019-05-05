namespace Core.Data
{
	using Core.Containers;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public struct KeyCountPair
	{
		public string Key;
		public int Count;
	};

	[CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapon")]
	public class WeaponData : ScriptableObject
	{
		#region Fields
		[SerializeField] private int _UID = 0;
		[SerializeField] private int _goldValue = 0;
		[SerializeField] private string _name = "WeaponName";
		[SerializeField] private KeyCountPair[] _recipe = null;

		[Header("Remanent Market Data")]
		[SerializeField] private int _modifierTimer = 100;
		[SerializeField] private int _priceModifierUp = 1;
		[SerializeField] private int _priceModifierDown = 2;
		[SerializeField] private int _priceMax = 7;
		[SerializeField] private int _priceMin = 1;

		#endregion Fields
		public int UID { get { return _UID; } }
		public int GoldValue { get { return _goldValue; } }
		public string Name { get { return _name; } }
		public KeyCountPair[] Recipe { get { return _recipe; } }

		public int ModifierTimer { get { return _modifierTimer; } }
		public int PriceModifierUp { get { return _priceModifierUp; } }
		public int PriceModifierDown { get { return _priceModifierDown; } }
		public int PriceMax { get { return _priceMax; } }
		public int PriceMin { get { return _priceMin; } }
	}
}