namespace Engine.Manager
{
	using Engine.Utils;
    using Core.Data;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class DatabaseManager : Singleton<DatabaseManager>
	{
		#region Fields
		[Header("Inn")]
		[SerializeField] private InnUpgradesData _innUpgrades = null;
		[SerializeField] private InnStartData _innStats = null;

		[Header("Mine")]
		[SerializeField] private MineUpgradesData _mineUpgrades = null;
		[SerializeField] private MineStartData _mineStats = null;

		[Header("Forge")]
		[SerializeField] private ForgeUpgradesData _forgeUpgrades = null;
		[SerializeField] private ForgeStartData _forgeStats = null;

		[Header("TradingPost")]
		[SerializeField] private TradingPostUpgradesData _tPUpgrades = null;
		[SerializeField] private TradingPostStartData _tPStats = null;

		[Header("Inventory")]
		[SerializeField] private ResourceListData _resourceList = null;
		[SerializeField] private WeaponListData _weaponList = null;

		[Header("Fortress")]
		[SerializeField] private FortressData[] _fortress = null;
		#endregion Fields

		#region Properties
		public InnUpgradesData InnUpgrades { get { return _innUpgrades; } }
		public InnStartData InnStats { get { return _innStats; } }

		public MineUpgradesData MineUpgrades { get { return _mineUpgrades; } }
        public MineStartData MineStats { get { return _mineStats; } }

		public ForgeUpgradesData ForgeUpgrades { get { return _forgeUpgrades; } }
		public ForgeStartData ForgeStats { get { return _forgeStats; } }

		public TradingPostUpgradesData TradingPostUpgrades { get { return _tPUpgrades; } }
		public TradingPostStartData TradingPostStats { get { return _tPStats; } }

		public ResourceListData ResourceList { get { return _resourceList; } }
		public WeaponListData WeaponList { get { return _weaponList; } }

		public FortressData[] Fortress { get { return _fortress; } }
		#endregion Properties

		#region Methods
		public WeaponData ExtractWeapon(string key)
		{
			for (int i = 0; i < _weaponList.Weapons.Length; i++)
			{
				if (_weaponList.Weapons[i].Name == key)
					return _weaponList.Weapons[i];
			}
			return null;
		}
		#endregion Methods
	}
}