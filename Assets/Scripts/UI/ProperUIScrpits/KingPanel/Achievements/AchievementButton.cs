namespace DwarfClicker.UI.Achievement
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;
	using DwarfClicker.Core.Achievement;
	using DwarkClicker.Helper;
	using Engine.Manager;

	public class AchievementButton : MonoBehaviour
	{
		#region Fields
		[SerializeField] private TextMeshProUGUI _nameText = null;
		[SerializeField] private TextMeshProUGUI _descText = null;
		[SerializeField] private TextMeshProUGUI _rewardText = null;
		[SerializeField] private TextMeshProUGUI _barProgText = null;
		[SerializeField] private Image _achievementImage = null;
		[SerializeField] private Transform _bar = null;

		private Vector3 _emptyPos = Vector3.zero;

		private Achievement _achievement = null;
		
		private Vector3 _fullPos = Vector3.zero;
		#endregion Fields

		#region Properties
		#endregion Properties

		#region Methods
		public void Start()
		{
			_emptyPos = _bar.transform.localPosition;
		}

		public void Display(Achievement achievement = null)
		{
			// Set Data
			if (achievement != null)
			{
				_achievement = achievement;
				_fullPos = _bar.localPosition;
				_emptyPos = new Vector3(0, _bar.localPosition.y, _bar.localPosition.z);
			}

			// Insert Data
			_nameText.text = _achievement.Name;

			if (_achievement._isfinished)
			{
				_barProgText.text = "Fnished !";
			}
			else if (_achievement.CanGetReward)
			{
				_barProgText.text = "Tap to claim your reward !";
			}
			else
			{
				_barProgText.text = UIHelper.FormatIntegerString(_achievement.CurrentValue) + "/" + UIHelper.FormatIntegerString(_achievement.CurrentStepMax);
			}

			_descText.text = _achievement.Objective;

			_rewardText.text = _achievement.MithrilReward.ToString();

			_achievementImage.sprite = AchievementManager.Instance.ExtractAchievement(_achievement.Name).AchievementSprite;

			// Progress Bar set up
			float t = (float)_achievement.CurrentValue / (float)_achievement.CurrentStepMax;
			_bar.localPosition = Vector3.Lerp(_fullPos, _emptyPos, t);
		}

		public void ClaimReward()
		{
			if (_achievement.CanGetReward)
			{
				_achievement.GetReward();
			}
			Display();
		}
		#endregion Methods
	}
}