namespace DwarfClicker.Core.Data
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public enum ETaskType
	{
		RESOURCE,
		GOLD,
		TOOL
	};

	[CreateAssetMenu(fileName = "KingTask", menuName = "Data/KingTask")]
	public class TaskData : ScriptableObject
	{
		[SerializeField] private ulong _amountAsked = 200;
		[SerializeField] private string _description = "fluff";
		[SerializeField] private ETaskType _typeAsked = ETaskType.TOOL;
		[SerializeField] private int _mithrilRewardAmount = 1;
		[SerializeField] private float _rewardMultMin = 0.8f;
		[SerializeField] private float _rewardMultMax = 1.5f;


		public ulong AmountAsked { get { return _amountAsked; } }
		public string Description { get { return _description; } }
		public ETaskType TypeAsked { get { return _typeAsked; } }
		public int MithrilRewardAmount { get { return _mithrilRewardAmount; } }
		public float PriceMultMin { get { return _rewardMultMin; } }
		public float PriceMultMax { get { return _rewardMultMax; } }
	}
}