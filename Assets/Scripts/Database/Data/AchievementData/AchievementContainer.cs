namespace DwarfClicker.Database
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Achievement", menuName = "Data/Achievement/Achievement")]
	public class AchievementContainer : ScriptableObject
	{
		#region Fields
		[SerializeField] private string _key = null;
		[SerializeField] private string _name = null;
		[SerializeField] private string _objective = null;
		[SerializeField] private int[] _stepMax = null;
		[SerializeField] private int _mithrilReward = 0;
		[SerializeField] private Sprite _achievementSprite = null;
		#endregion Fields

		#region Properties
		public string Key { get { return _key; } }
		public string Name { get { return _name; } }
		public string Objective { get { return _objective; } }
		public int[] StepMax { get { return _stepMax; } }
		public int MithrilReward { get { return _mithrilReward; } }
		public Sprite AchievementSprite { get { return _achievementSprite; } }
		#endregion Properties
	}
}