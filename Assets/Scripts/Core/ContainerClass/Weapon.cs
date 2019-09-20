namespace DwarfClicker.Core.Containers
{
	using DwarfClicker.Core.Data;
	using Engine;
	using System;
	using UnityEngine;

	[Serializable]
	public class Weapon
	{
		#region Fields
		[SerializeField] private int _UID = 0;
		[SerializeField] private string _name = "weaponName";
		[SerializeField] private int _count = 0;
		[SerializeField] private int _sellPrice = 0;
		[SerializeField] private int _modTimer = 0;
		[SerializeField] private int _modRank = 0;
		[SerializeField] private KeyCountPair[] _recipie;
		[SerializeField] private Sprite _itemSprite = null;
		#endregion Fields

		#region Fields
		public int UID { get { return _UID; } }
		public string Name { get { return _name; } }
		public int Count { get { return _count; } }
		public int SellPrice { get { return _sellPrice; } set { _sellPrice = value; } }
		public KeyCountPair[] Recipie { get { return _recipie; } }
		public int ModTimer { get { return _modTimer; } set { _modTimer = value; } }
		public int ModRank { get { return _modRank; } set { _modRank = value; } }
		public Sprite ItemSprite { get { return _itemSprite; } }
		#endregion Fields

		#region Methods
		public void Init(WeaponData weapon)
		{
			_itemSprite = weapon.WeaponSprite;
			_UID = weapon.UID;
			_name = weapon.Name;
			_sellPrice = weapon.GoldValue;
			_count = 0;
			_recipie = weapon.Recipe;
		}

		public void SetSprite(Sprite sprite)
		{
			_itemSprite = sprite;
		}

		public void UpdateCount(int count)
		{
			_count += count;
			_count = Mathf.Clamp(_count, 0, int.MaxValue);
		}

		public void ResetCount()
		{
			_count = 0;
			ModRank = 0;
		}
		#endregion Methods
	}
}