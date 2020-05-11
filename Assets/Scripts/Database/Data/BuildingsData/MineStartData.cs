namespace DwarfClicker.Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "MineStartStats", menuName = "Data/MineStartStats")]
	public class MineStartData : ScriptableObject
	{
		[SerializeField] private int _workerAmount;
		[SerializeField] private float _resByWorker;
		[SerializeField] private int _luck;
		[SerializeField] private float _beerConsumption;
		[SerializeField] private float _noBeerConsumptionChance;
		[SerializeField] private float _cycleDuration;
		[SerializeField] private int _richVein;
		[SerializeField] private int _mithrilChance;
         
		public int WorkerAmount { get { return _workerAmount; } }
		public float ResByWorker { get { return _resByWorker; } }
		public int Luck { get { return _luck; } }
		public float NoBeerConsumptionChance { get { return _noBeerConsumptionChance; } }
		public float BeerConsumption { get { return _beerConsumption; } }
		public float CycleDuration { get { return _cycleDuration; } }
		public int RichVein { get { return _richVein; } }
		public int Mithril { get { return _mithrilChance; } }
	}
}