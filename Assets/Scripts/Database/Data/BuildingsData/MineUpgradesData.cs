namespace DwarfClicker.Core.Data
{
    using Engine.Utils;
    using UnityEngine;

    [CreateAssetMenu(fileName = "MineUpgrades", menuName = "Data/MineUpgrades")]
    public class MineUpgradesData : ScriptableObject
    {
		[Header("Incremental")]
		[SerializeField] private IntUpgrade _workerAmount;
		[Header("Incremental")]
		[SerializeField] private IntUpgrade _resByWorker;
		[Header("Decremental")]
		[SerializeField] private IntUpgrade _luck;
		[Header("Percentage 0-1")]
		[SerializeField] private FloatUpgrade _beerConsumption;
		[Header("Percentage 0-1")]
		[SerializeField] private FloatUpgrade _cycleDuration;
		[Header("Incremental / Percentage 0-1")]
		[SerializeField] private FloatUpgrade _noBeerConsumptionChance;
		[Header("Decremental")]
		[SerializeField] private IntUpgrade _richVein;
		[Header("Decremental")]
		[SerializeField] private IntUpgrade _mithrilChance;

        public IntUpgrade WorkerAmount { get { return _workerAmount; } }
        public IntUpgrade ResByWorker { get { return _resByWorker; } }
        public IntUpgrade Luck { get { return _luck; } }
		public FloatUpgrade BeerConsumption { get { return _beerConsumption; } }
		public FloatUpgrade NoBeerConsumption { get { return _noBeerConsumptionChance; } }
		public FloatUpgrade CycleDuration { get { return _cycleDuration; } }
        public IntUpgrade RichVein { get { return _richVein; } }
        public IntUpgrade Mithril { get { return _mithrilChance; } }
    }
} 