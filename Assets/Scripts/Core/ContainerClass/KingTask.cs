namespace DwarfClicker.Core.Achievement
{
	using DwarfClicker.Core.Containers;
	using DwarfClicker.Core.Data;
	using DwarfClicker.Database;
	using Engine.Manager;
	using Engine.Utils;
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
		public WeaponData _weapon = null;
		public ResourceData _resource = null;
		public Sprite _goalSprite = null;
		#endregion Fields

		#region Properties
		public int Amount { get { return _amount; } }
		public string Description { get { return _description; } }
		public ETaskType TaskType { get { return _taskType; } }
		public int GoldRewardAmount { get { return _goldRewardAmount; } }
		public int MithrilRewardAmount { get { return _mithrilRewardAmount; } }
		public Sprite GoalSprite { get { return _goalSprite; } }
		#endregion Properties

		#region Methods
		public void Init(TaskData task)
		{
			_amount = (int)(task.AmountAsked * UnityEngine.Random.Range(task.PriceMultMin, task.PriceMultMax));
			_description = task.Description;
            _taskType = task.TypeAsked;
			_mithrilRewardAmount = task.MithrilRewardAmount;
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;

			if (_taskType == ETaskType.TOOL)
			{
				List<WeaponData> weaponList = new List<WeaponData>();
				
				for (int i = 0; i < profile.Fortress.Count; i++)
				{
					if (profile.Fortress[i]._isBought == true)
					{
						for (int y = 0; y < profile.Fortress[i].WeaponToCraft.Length; y++)
						{
							weaponList.Add(profile.Fortress[i].WeaponToCraft[y]);
						}
					}
				}
				_weapon = weaponList[UnityEngine.Random.Range(0, weaponList.Count)];
				_goalSprite = DatabaseManager.Instance.ExtractWeapon(_weapon.Name).WeaponSprite;
			}
			else if (_taskType == ETaskType.RESOURCE)
			{
				List<ResourceData> resourceList = new List<ResourceData>();

				for (int i = 0; i < profile.Fortress.Count; i++)
				{
					if (profile.Fortress[i]._isBought == true)
					{
						resourceList.Add(profile.Fortress[i].ResourceProduced);
					}
				}
				_resource = resourceList[UnityEngine.Random.Range(0, resourceList.Count)];
				_goalSprite = DatabaseManager.Instance.ExtractResource(_resource.Name).ResourceSprite;
			}
			else
			{
				_goalSprite = DatabaseManager.Instance.GoldIcon;
			}
		}

		public Sprite GetGoalSprite()
		{
			if (_weapon != null)
			{
				return DatabaseManager.Instance.ExtractWeapon(_weapon.Name).WeaponSprite;
			}
			else if (_resource != null)
			{
				return DatabaseManager.Instance.ExtractResource(_resource.Name).ResourceSprite;
			}
			else
				return DatabaseManager.Instance.GoldIcon;
		}

		public void GetReward()
		{
            JSonManager.Instance.PlayerProfile.Mithril += _mithrilRewardAmount;
		}
		#endregion Methods
	}
}