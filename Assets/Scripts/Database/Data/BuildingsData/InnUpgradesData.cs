namespace DwarfClicker.Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "InnUpgrades", menuName = "Data/InnUpgrades")]
	public class InnUpgradesData : ScriptableObject
	{
		[Header("Incremental")]
		[SerializeField] private IntUpgrade _beerByTap;
		[Header("Incremental")]
		[SerializeField] private IntUpgrade _storage;

		public IntUpgrade BeerByTap { get { return _beerByTap; } }
		public IntUpgrade Storage { get { return _storage; } }
	}
}