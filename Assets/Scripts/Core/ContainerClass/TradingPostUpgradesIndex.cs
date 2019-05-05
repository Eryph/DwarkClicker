namespace Core.Containers
{
	using System;

	[Serializable]
	public class TradingPostUpgradesIndex
	{
		public int _workerNbIndex = 0;
		public int _sellByWorkerIndex = 0;
		public int _cycleDurationIndex = 0;
		public int _winBeerAmountIndex = 0;
		public int _winBeerChanceIndex = 0;
		public int _goldMultIndex = 0;

		public void ResetUpgrades()
		{
			_sellByWorkerIndex = 0;
			_workerNbIndex = 0;
			_cycleDurationIndex = 0;
			_winBeerAmountIndex = 0;
			_winBeerChanceIndex = 0;
			_goldMultIndex = 0;
		}
	}
}