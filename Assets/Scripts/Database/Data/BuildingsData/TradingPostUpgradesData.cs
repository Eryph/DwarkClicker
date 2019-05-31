namespace DwarfClicker.Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "TradingPostUpgrades", menuName = "Data/TradingPostUpgrades")]
	public class TradingPostUpgradesData : ScriptableObject
	{
		[Header("Incremental")]
		[SerializeField] private IntUpgrade _workerAmount;
		[Header("Incremental")]
		[SerializeField] private IntUpgrade _sellByWorker;
		[Header("Percentage 0-1")]
		[SerializeField] private FloatUpgrade _cycleDuration;
		[Header("Incremental")]
		[SerializeField] private IntUpgrade _winBeerAmount;
		[Header("Decremental")]
		[SerializeField] private IntUpgrade _winBeerChance;
		[Header("Incremental / Percentage 0-1")]
		[SerializeField] private FloatUpgrade _goldMult;

		public IntUpgrade WorkerAmount { get { return _workerAmount; } }
		public IntUpgrade SellByWorker { get { return _sellByWorker; } }
		public FloatUpgrade CycleDuration { get { return _cycleDuration; } }
		public FloatUpgrade GoldMult { get { return _goldMult; } }
		public IntUpgrade WinBeerChance { get { return _winBeerChance; } }
		public IntUpgrade WinBeerAmount { get { return _winBeerAmount; } }
	}
}