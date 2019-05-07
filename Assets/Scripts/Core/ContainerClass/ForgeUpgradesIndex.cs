namespace DwarfClicker.Core.Containers
{
	using System;

	[Serializable]
	public class ForgeUpgradesIndex
	{
		public int _workerNbIndex = 0;
		public int _wByWorkerIndex = 0;
		public int _cycleDurationIndex = 0;
		public int _instantSellingChanceIndex = 0;
		public int _instantSellingGoldBonusIndex = 0;

		public void ResetUpgrades()
		{
			_workerNbIndex = 0;
			_wByWorkerIndex = 0;
			_cycleDurationIndex = 0;
			_instantSellingChanceIndex = 0;
			_instantSellingGoldBonusIndex = 0;
		}
	}
}