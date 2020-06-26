namespace DwarfClicker.Core.Data
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Fortress", menuName = "Data/Fortress")]
	public class FortressData : ScriptableObject
	{
		[SerializeField] private int _UID = 0;
		[SerializeField] private string _name = "resourceName";
		[SerializeField] private int _price = 1000;
		[SerializeField] private ResourceData _resourceToProduce = null;
		[SerializeField] private WeaponData[] _weaponsToProduce = null;
        [SerializeField] private Sprite _fortressIcon = null;

        public Sprite FortressIcon { get { return _fortressIcon; } }
		public int UID { get { return _UID; } }
		public string Name { get { return _name; } }
		public int Price { get { return _price; } }
		public ResourceData ResourceToProduce { get { return _resourceToProduce; } }
		public WeaponData[] WeaponsToProduce { get { return _weaponsToProduce; } }
	}
}