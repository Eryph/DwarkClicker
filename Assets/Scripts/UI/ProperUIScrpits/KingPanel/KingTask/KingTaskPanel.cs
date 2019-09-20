namespace DwarfClicker.UI.KingPanel
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;
	using Engine.Utils;
	using DwarfClicker.Core.Achievement;
	using Engine.Manager;

	public class KingTaskPanel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _description = null;
		[SerializeField] private Image _goalImage = null;
		[SerializeField] private Image _rewardImage = null;
		[SerializeField] private TextMeshProUGUI _goalAmount = null;
		[SerializeField] private TextMeshProUGUI _rewardAmount = null;
		[SerializeField] private GameObject _noTaskPanel = null;
		[SerializeField] private Button _claimButton = null;

		public void Display(PlayerProfile profile)
		{
			if (profile._kingTask == null)
			{
				_noTaskPanel.SetActive(true);
			}
			else
			{
				KingTask task = profile._kingTask;
				_noTaskPanel.SetActive(false);
				_description.text = task.Description;


				_goalImage.sprite = task.GetGoalSprite();
				_goalAmount.text = task.Amount.ToString();

				if (task.IsMithrilReward)
				{
					_rewardImage.sprite = DatabaseManager.Instance.MithrilIcon;
					_rewardAmount.text = task.MithrilRewardAmount.ToString();
				}
				else
				{
					_rewardImage.sprite = DatabaseManager.Instance.GoldIcon;
					_rewardAmount.text = task.GoldRewardAmount.ToString();
				}

				int goalCount = 0;
				switch (task.TaskType)
				{
					case Core.Data.ETaskType.GOLD:
						goalCount = profile.Gold;
						break;
					case Core.Data.ETaskType.RESOURCE:
						goalCount = profile.Resources[task._resource.Name].Count;
						break;
					case Core.Data.ETaskType.TOOL:
						goalCount = profile.Weapons[task._weapon.Name].Count;
						break;
				}

				if (task.Amount > goalCount)
				{
					_claimButton.interactable = false;
				}
				else
				{
					_claimButton.interactable = true;
				}
			}
		}

		public void ClaimReward()
		{
			AchievementManager.Instance.ClaimTaskReward();
			Display(JSonManager.Instance.PlayerProfile);
		}

		public void ShowAdToResetTask()
		{
			if (!JSonManager.Instance.PlayerProfile._noMoreAdsBonus)
			{
				MonetizationManager.Instance.AdFinished += ResetTask;
				MonetizationManager.Instance.ShowAd();
			}
			else
			{
				ResetTask();
			}
		}

		private void ResetTask()
		{
			MonetizationManager.Instance.AdFinished -= ResetTask;
			AchievementManager.Instance.ResetTask();
			Display(JSonManager.Instance.PlayerProfile);
		}
	}
}