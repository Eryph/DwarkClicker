namespace DwarfClicker.UI.KingPanel
{
	using DwarfClicker.UI.Achievement;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class KingPanel : MonoBehaviour {

		[SerializeField] private AchievementPanel _achievementPanel = null;
		[SerializeField] private InventoryPanel _inventoryPanel = null;

		private void OnEnable()
		{
			_achievementPanel.Display();
			_inventoryPanel.Display();
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}