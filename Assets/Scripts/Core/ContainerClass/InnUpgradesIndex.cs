namespace DwarfClicker.Core.Containers
{
	using System;

	[Serializable]
	public class InnUpgradesIndex
	{
		public int _beerByTapIndex = 0;
		public int _storageIndex = 0;

		public void ResetUpgrades()
		{
			_beerByTapIndex = 0;
			_storageIndex = 0;
		}
	}
}