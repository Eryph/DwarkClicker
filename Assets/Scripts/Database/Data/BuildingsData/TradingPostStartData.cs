namespace DwarfClicker.Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "TradingPostStartStats", menuName = "Data/TradingPostStartStats")]
	public class TradingPostStartData : ScriptableObject
	{
		[SerializeField] private int _workerAmount;
		[SerializeField] private int _sellByWorker;
		[SerializeField] private int _winBeerAmount;
		[SerializeField] private int _winBeerChance;
		[SerializeField] private float _goldMult;
		[SerializeField] private float _cycleDuration;

		public int WorkerAmount { get { return _workerAmount; } }
		public int SellByWorker { get { return _sellByWorker; } }
		public float CycleDuration { get { return _cycleDuration; } }
		public float GoldMult { get { return _goldMult; } }
		public int WinBeerChance { get { return _winBeerChance; } }
		public int WinBeerAmount { get { return _winBeerAmount; } }
	}
}