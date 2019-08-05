namespace Engine.Manager
{
	using DwarfClicker.Core.Achievement;
	using DwarfClicker.Core.Data;
	using DwarfClicker.Database;
	using Engine.Manager;
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AchievementManager : Singleton<AchievementManager>
	{
		#region Fields
		[SerializeField] private AchievementContainer[] _achievementContainers = null;
		[SerializeField] private TaskData[] _tasks = null;
		[SerializeField] private int _taskCooldownH = 1;
		private PlayerProfile _profile = null;
		#endregion Fields

		#region Properties
		public AchievementContainer[] AchievementContainers { get { return _achievementContainers; } }
		#endregion Properties

		#region Methods
		#region Achievement
		public DictionaryStringAchievement GenerateAchievementCollection()
		{
			DictionaryStringAchievement retAchievement = new DictionaryStringAchievement();
			for (int i = 0; i < _achievementContainers.Length; i++)
			{
				Achievement newAchievement = new Achievement();
				newAchievement.Init(_achievementContainers[i]);
				retAchievement.Add(newAchievement.Key, newAchievement);
			}
			return retAchievement;
		}

		public void UpdateAchievement(string key, int value)
		{
			if (_profile == null)
			{
				_profile = JSonManager.Instance.PlayerProfile;
			}

			_profile._achievements[key].AddValue(value);
		}
		#endregion Achievement
		#region KingTask
		public void ResetTask()
		{
			_profile._kingTask = null;
			_profile._kingTask = GenerateTask();
		}

		public KingTask GenerateTask()
		{
			KingTask task = new KingTask();
			TaskData randomTask = _tasks[UnityEngine.Random.Range(0, _tasks.Length)];
			task.Init(randomTask);
			return task;
		}

		public void ClaimTaskReward()
		{
			KingTask task = _profile._kingTask;
			switch (task.TaskType)
			{
				case ETaskType.GOLD:
					_profile.Gold -= task.Amount;
					break;
				case ETaskType.RESOURCE:
					_profile.Resources[task._resource.Name].UpdateCount(-task.Amount);
					break;
				case ETaskType.TOOL:
					_profile.Weapons[task._weapon.Name].UpdateCount(-task.Amount);
					break;
			}

			if (task.IsMithrilReward)
			{
				_profile.Mithril += task.GoldRewardAmount;
			}
			else
			{
				_profile.Gold += task.GoldRewardAmount;
			}

			_profile._kingTask = null;
			_profile._taskTimeStamp = DateTime.Now + new TimeSpan(_taskCooldownH, 0, 0);
		}
		#endregion KingTask
		#endregion Methods
	}
}