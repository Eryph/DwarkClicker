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

	public class DailyRewardPanel : MonoBehaviour
	{
		[SerializeField] private Converter _converter = null;
		[SerializeField] private DailyRewardBox[] _dailyRewardBox = null;
		[SerializeField] private Button _redeemButton = null;
		[SerializeField] private TextMeshProUGUI _redeemButtonText = null;

		private void OnEnable()
		{
			Display();
		}

		private void Display()
		{
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;

			if (_redeemButton != null)
			{
				_redeemButton.interactable = true;
				_redeemButtonText.text = "Redeem";
				if (profile._dailyRewardIndex != 0)
				{
					TimeSpan oneDay = new TimeSpan(23, 0, 0);
					TimeSpan diff = DateTime.Now - profile._lastDailyRewardRedeemed;
					if (diff < oneDay)
					{
						_redeemButton.interactable = false;
						_redeemButtonText.text = "Already Redeemed Today";
					}
					else
					{
						if (diff > oneDay.Add(new TimeSpan(24, 0, 0)))
						{
							profile._dailyRewardIndex = 0;
						}
						_redeemButton.interactable = true;
					}
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

					if (i > profile._dailyRewardIndex)
					{
						_dailyRewardBox[i].SetNotAvailable();
					}
				}
			}
		}

		public void RedeemDailyReward()
		{
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;
			Gain(profile._dailyRewardIndex);
			profile._lastDailyRewardRedeemed = DateTime.Now;
			_redeemButton.interactable = false;
			profile._dailyRewardIndex++;
			if (profile._dailyRewardIndex > DatabaseManager.Instance.DailyRewards.Length)
			{
				profile._dailyRewardIndex = 0;
			}
			Display();
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}

		private void Gain(int index)
		{
			_converter.GainBeer(DatabaseManager.Instance.DailyRewards[index].BeerGain);
			_converter.GainGold(DatabaseManager.Instance.DailyRewards[index].GoldGain);
			_converter.GainMithril(DatabaseManager.Instance.DailyRewards[index].MithrilGain);
			_converter.GainResource(JSonManager.Instance.PlayerProfile.LastFortress.ResourceProduced.Name, DatabaseManager.Instance.DailyRewards[index].ResourceGain);
		}
	}
}