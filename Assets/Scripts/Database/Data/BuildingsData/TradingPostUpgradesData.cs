namespace DwarfClicker.Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "TradingPostUpgrades", menuName = "Data/TradingPostUpgrades")]
	public class TradingPostUpgradesData : ScriptableObject
	{
		[SerializeField] private IntUpgrade _workerAmount;
		[SerializeField] private IntUpgrade _sellByWorker;
		[SerializeField] private FloatUpgrade _cycleDuration;
		[SerializeField] private IntUpgrade _winBeerAmount;
		[SerializeField] private IntUpgrade _winBeerChance;
		[SerializeField] private FloatUpgrade _goldMult;

		public IntUpgrade WorkerAmount { get { return _workerAmount; } }
		public IntUpgrade SellByWorker { get { return _sellByWorker; } }
		public FloatUpgrade CycleDuration { get { return _cycleDuration; } }
		public FloatUpgrade GoldMult { get { return _goldMult; } }
		public IntUpgrade WinBeerChance { get { return _winBeerChance; } }
		public IntUpgrade WinBeerAmount { get { return _winBeerAmount; } }
	}
}