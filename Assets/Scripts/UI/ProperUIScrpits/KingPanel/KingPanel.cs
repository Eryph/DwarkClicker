namespace DwarfClicker.UI.KingPanel
{
	using DwarfClicker.UI.Achievement;
	using Engine.Manager;
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class KingPanel : MonoBehaviour {
		[SerializeField] private KingTaskPanel _kingTaskPanel = null;
		[SerializeField] private AchievementPanel _achievementPanel = null;
		[SerializeField] private InventoryPanel _inventoryPanel = null;

		private void OnEnable()
		{
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;
			if (profile._kingTask == null && DateTime.Now >= profile._taskTimeStamp)
			{
				profile._kingTask = AchievementManager.Instance.GenerateTask();
			}

			_achievementPanel.Display();
			_inventoryPanel.Display();
			_kingTaskPanel.Display(profile);
            
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}