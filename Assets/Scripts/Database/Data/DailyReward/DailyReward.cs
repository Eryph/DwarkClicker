namespace DwarfClicker.Database
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "DailyReward", menuName = "Data/DailyRewards/DailyReward")]
	public class DailyReward : ScriptableObject
	{
		[SerializeField] private int _beerGain = 0;
		[SerializeField] private int _goldGain = 0;
		[SerializeField] private int _resourceGain = 0;
		[SerializeField] private int _mithrilGain = 0;

		public int BeerGain { get { return _beerGain; } }
		public int GoldGain { get { return _goldGain; } }
		public int ResourceGain { get { return _resourceGain; } }
		public int MithrilGain { get { return _mithrilGain; } }
	}
}