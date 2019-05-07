namespace DwarfClicker.Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "MineStartStats", menuName = "Data/MineStartStats")]
	public class MineStartData : ScriptableObject
	{
		[SerializeField] private int _workerNb;
		[SerializeField] private int _resByWorker;
		[SerializeField] private int _luck;
		[SerializeField] private float _beerConsumption;
		[SerializeField] private float _cycleDuration;
		[SerializeField] private int _richVein;
		[SerializeField] private int _mithrilChance;

		public int WorkerNb { get { return _workerNb; } }
		public int ResByWorker { get { return _resByWorker; } }
		public int Luck { get { return _luck; } }
		public float BeerConsumption { get { return _beerConsumption; } }
		public float CycleDuration { get { return _cycleDuration; } }
		public int RichVein { get { return _richVein; } }
		public int Mithril { get { return _mithrilChance; } }
	}
}