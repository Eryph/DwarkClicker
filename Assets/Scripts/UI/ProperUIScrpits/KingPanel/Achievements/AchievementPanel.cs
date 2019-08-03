namespace DwarfClicker.UI.Achievement
{
	using DwarfClicker.Core.Achievement;
	using DwarfClicker.Database;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AchievementPanel : MonoBehaviour
	{
		[SerializeField] private AchievementButton _button = null;

		public void Display()
		{
			DictionaryStringAchievement achievements = JSonManager.Instance.PlayerProfile._achievements;

			int i = 0;
			Transform _parent = _button.transform.parent;
			foreach (KeyValuePair<string, Achievement> achievement in achievements)
			{
				if (i < _parent.childCount)
				{
					_parent.GetChild(i).GetComponent<AchievementButton>().Display(achievement.Value);
				}
				else
				{
					AchievementButton newButton = Instantiate(_button, _button.transform.parent);
					newButton.Display(achievement.Value);
				}
				i++;
			}
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}