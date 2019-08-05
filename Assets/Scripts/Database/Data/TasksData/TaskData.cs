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
		[SerializeField] private int _amount = 200;
		[SerializeField] private string _description = "fluff";
		[SerializeField] private ETaskType _taskType = ETaskType.TOOL;
		[SerializeField] private int _goldRewardAmount = 100;
		[SerializeField] private int _mithrilRewardAmount = 1;
		[SerializeField] private float _mithrilRewardChance = 0.1f;
		[SerializeField] private float _rewardMultMin = 0.5f;
		[SerializeField] private float _rewardMultMax = 1.2f;
		[SerializeField] private float _priceMultMin = 0.8f;
		[SerializeField] private float _priceMultMax = 1.5f;


		public int Amount { get { return _amount; } }
		public string Description { get { return _description; } }
		public ETaskType TaskType { get { return _taskType; } }
		public int GoldRewardAmount { get { return _goldRewardAmount; } }
		public int MithrilRewardAmount { get { return _mithrilRewardAmount; } }
		public float MithrilRewardChance { get { return _mithrilRewardChance; } }
		public float RewardMultMin { get { return _rewardMultMin; } }
		public float RewardMultMax { get { return _rewardMultMax; } }
		public float PriceMultMin { get { return _priceMultMin; } }
		public float PriceMultMax { get { return _priceMultMax; } }
	}
}