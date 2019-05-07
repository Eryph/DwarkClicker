namespace DwarfClicker.Database
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "DailyRewardList", menuName = "Data/DailyRewards/DailyRewardList")]
	public class DailyRewardList : ScriptableObject
	{
		[SerializeField] private DailyReward[] _dailyRewards = null;

		public DailyReward[] DailyRewards { get { return _dailyRewards; } }
	}
}