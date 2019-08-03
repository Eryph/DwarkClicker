namespace DwarfClicker.Core.Achievement
{
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using DwarfClicker.Database;
	using Engine.Manager;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class KingTask
	{
		#region Fields
		public int _amount = 200;
		public string _description = "fluff";
		public ETaskType _taskType = ETaskType.TOOL;
		public int _goldRewardAmount = 100;
		public int _mithrilRewardAmount = 1;
		public bool _isMithrilReward = false;
		public WeaponData _weapon = null;
		public ResourceData _resource = null;
		#endregion Fields

		#region Properties
		public int Amount { get { return _amount; } }
		public string Description { get { return _description; } }
		public ETaskType TaskType { get { return _taskType; } }
		public int GoldRewardAmount { get { return _goldRewardAmount; } }
		public int MithrilRewardAmount { get { return _mithrilRewardAmount; } }
		public bool IsMithrilReward { get { return _isMithrilReward; } }
		#endregion Properties

		#region Methods
		public void Init(TaskData task)
		{
			_amount = (int)(task.Amount * UnityEngine.Random.Range(task.PriceMultMin, task.PriceMultMax));
			_description = task.Description;
			_taskType = task.TaskType;
			_goldRewardAmount = (int)(task.GoldRewardAmount * UnityEngine.Random.Range(task.RewardMultMin, task.RewardMultMax)); ;
			_mithrilRewardAmount = task.MithrilRewardAmount;
			_isMithrilReward = UnityEngine.Random.Range(0f, 1f) <= task.MithrilRewardChance;

			if (_taskType == ETaskType.TOOL)
			{
				WeaponData[] weaponList = DatabaseManager.Instance.WeaponList.Weapons;
				_weapon = weaponList[UnityEngine.Random.Range(0, weaponList.Length)];
			}
			else if (_taskType == ETaskType.RESOURCE)
			{
				ResourceData[] resourceList = DatabaseManager.Instance.ResourceList.Resources;
				_resource = resourceList[UnityEngine.Random.Range(0, resourceList.Length)];
			}
		}

		public void GetReward()
		{
			if (_isMithrilReward)
				JSonManager.Instance.PlayerProfile.Mithril += _mithrilRewardAmount;
			else
				JSonManager.Instance.PlayerProfile.Gold += _goldRewardAmount;
		}
		#endregion Methods
	}
}