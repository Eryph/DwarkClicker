namespace DwarfClicker.Core.Data
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "ForgeStartStats", menuName = "Data/ForgeStartStats")]
	public class ForgeStartData : ScriptableObject
	{
		[SerializeField] private int _workerAmount = 0;
		[SerializeField] private int _wByWorker = 0;
		[SerializeField] private float _cycleDuration = 0;
		[SerializeField] private int _instantSellingChance = 0;
		[SerializeField] private float _instantSellingGoldBonus = 0;

		public int WorkerAmount { get { return _workerAmount; } }
		public int WByWorker { get { return _wByWorker; } }
		public float CycleDuration { get { return _cycleDuration; } }
		public int InstantSellingChance { get { return _instantSellingChance; } }
		public float InstantSellingGoldBonus { get { return _instantSellingGoldBonus; } }
	}
}