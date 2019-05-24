namespace DwarfClicker.UI.DailyReward
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using Engine.Manager;
	using UnityEngine.UI;
	using Engine.Utils;
	using DwarfClicker.Core.Data;
	using DwarfClicker.Core;
	using System;
	using Preprod.UI;

	public class DailyRewardPanel : MonoBehaviour
	{
		[SerializeField] private DailyRewardController _controller = null;
		[SerializeField] private DailyRewardBox[] _dailyRewardBox = null;
		[SerializeField] private GameObject _alreadyRedeemeedeemButton = null;
		[SerializeField] private AllDataDisplayer _header = null;

		private void OnEnable()
		{
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;
			Display(profile);
			

		}

		private void Display(PlayerProfile profile)
		{
			_alreadyRedeemeedeemButton.SetActive(false);
			if (profile._dailyRewardIndex != 0)
			{
				if (profile.IsDailyRewardAvailable)
				{
					_alreadyRedeemeedeemButton.SetActive(false);
				}
				else
				{
					if (profile.IsDailyRewardReset)
					{
						profile._dailyRewardIndex = 0;
					}
					_alreadyRedeemeedeemButton.SetActive(true);
				}
			}

			for (int i = 0; i < _dailyRewardBox.Length; i++)
			{
				if (i < profile._dailyRewardIndex)
				{
					_dailyRewardBox[i].SetAlreadyRedeemed();
				}
				else
				{
					if (DatabaseManager.Instance.DailyRewards[i].BeerGain > 0)
					{
						_dailyRewardBox[i].Init(DatabaseManager.Instance.DailyRewards[i].BeerGain, DatabaseManager.Instance.BeerIcon, Color.white);
					}
					if (DatabaseManager.Instance.DailyRewards[i].GoldGain > 0)
					{
						_dailyRewardBox[i].Init(DatabaseManager.Instance.DailyRewards[i].GoldGain, DatabaseManager.Instance.GoldIcon, Color.white);
					}
					if (DatabaseManager.Instance.DailyRewards[i].ResourceGain > 0)
					{
						ResourceData resource = profile.LastFortress.ResourceProduced;
						_dailyRewardBox[i].Init(DatabaseManager.Instance.DailyRewards[i].ResourceGain, resource.ResourceSprite, Color.white);
					}
					if (DatabaseManager.Instance.DailyRewards[i].MithrilGain > 0)
					{
						_dailyRewardBox[i].Init(DatabaseManager.Instance.DailyRewards[i].MithrilGain, DatabaseManager.Instance.MithrilIcon, Color.white);
					}

					if (i == profile._dailyRewardIndex)
					{
						_dailyRewardBox[i].SetAvailable();
					}

					if (i > profile._dailyRewardIndex)
					{
						_dailyRewardBox[i].SetNotAvailable();
					}
				}
			}
		}

		public void RedeemDailyReward()
		{
			_controller.RedeemDailyReward();
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;
			_header.SetDailyRewardButtonDisabled();
			_alreadyRedeemeedeemButton.SetActive(true);
			Display(profile);
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}