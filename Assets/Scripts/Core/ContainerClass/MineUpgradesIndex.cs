namespace DwarfClicker.Core.Containers
{
	using System;

	[Serializable]
	public class MineUpgradesIndex
	{
		public int _workerNbIndex = 0;
		public int _resByWorkerIndex = 0;
		public int _beerConsoIndex = 0;
		public int _luckIndex = 0;
		public int _cycleDurationIndex = 0;
		public int _richVeinIndex = 0;
		public int _mithrilChanceIndex = 0;

		public void ResetUpgrades()
		{
			_workerNbIndex = 0;
			_resByWorkerIndex = 0;
			_beerConsoIndex = 0;
			_luckIndex = 0;
			_cycleDurationIndex = 0;
			_richVeinIndex = 0;
			_mithrilChanceIndex = 0;
		}
	}
}