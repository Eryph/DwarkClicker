namespace DwarfClicker.Core.Achievement
{
	using DwarfClicker.Database;
	using Engine.Manager;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class Achievement
	{
		#region Fields
		public string _key = null;
		public string _name = null;
		public string _objective = null;
		public int[] _stepMax = null;
		public int _currentValue = 0;
		public int _currentStep = 0;
		public int _mithrilReward = 0;
		public bool _isfinished = false;
		public Sprite _achievementSprite = null;
		#endregion Fields

		#region Properties
		public string Key { get { return _key; } }
		public string Name { get { return _name; } }
		public string Objective { get { return _objective; } }
		public int[] StepMax { get { return _stepMax; } }
		public int CurrentValue { get { return _currentValue; } }
		public int CurrentStep { get { return _currentStep; } }
		public int MithrilReward { get { return _mithrilReward; } }
		public bool IsFinished { get { return _isfinished; } }
		public Sprite AchievementSprite { get { return _achievementSprite; } }

		public bool CanGetReward {
            get
            {
                if (_currentStep < _stepMax.Length)
                    return _currentValue >= _stepMax[_currentStep];
                else
                    return false;
            }
        }
		public int CurrentStepMax {
            get
            {
                if (_currentStep < _stepMax.Length)
                    return _stepMax[_currentStep];
                else
                    return _stepMax[_stepMax.Length - 1];
            }
        }
		#endregion Properties

		#region Methods
		public void Init(AchievementContainer achievement)
		{
			_key = achievement.Key;
			_name = achievement.Name;
			_objective = achievement.Objective;
			_stepMax = achievement.StepMax;
			_mithrilReward = achievement.MithrilReward;
			_achievementSprite = achievement.AchievementSprite;
		}

		public void AddValue(int value)
		{
			_currentValue += value;
		}

		public void GetReward()
		{
			JSonManager.Instance.PlayerProfile.Mithril += _mithrilReward;
			_currentStep++;
			if (_currentStep >= _stepMax.Length)
			{
				_isfinished = true;
			}
		}
		#endregion Methods
	}
}