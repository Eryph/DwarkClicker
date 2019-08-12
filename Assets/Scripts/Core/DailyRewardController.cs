namespace DwarfClicker.Core
{
	using Engine.Manager;
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class DailyRewardController : MonoBehaviour
	{
		[SerializeField] private Converter _converter = null;
		private PlayerProfile _playerProfile = null;

		private void Start()
		{
			_playerProfile = JSonManager.Instance.PlayerProfile;
			CheckDailyRewardAvailability();
		}

		private void CheckDailyRewardAvailability()
		{
			if (_playerProfile._dailyRewardIndex > DatabaseManager.Instance.DailyRewards.Length)
			{
				_playerProfile._dailyRewardIndex = 0;
				_playerProfile.IsDailyRewardAvailable = true;
			}
			else
			{
				TimeSpan oneDay = new TimeSpan(23, 0, 0);
				TimeSpan diff = DateTime.Now - _playerProfile._lastDailyRewardRedeemed;
				_playerProfile.IsDailyRewardAvailable = oneDay < diff;
			}
		}

		public void RedeemDailyReward(float mult = 1)
		{
			Gain(_playerProfile._dailyRewardIndex, mult);
			_playerProfile.IsDailyRewardAvailable = false;
			_playerProfile._lastDailyRewardRedeemed = DateTime.Now;
			_playerProfile._dailyRewardIndex++;
			if (_playerProfile._dailyRewardIndex > DatabaseManager.Instance.DailyRewards.Length)
			{
				_playerProfile._dailyRewardIndex = 0;
			}
		}

		private void Gain(int index, float mult = 1)
		{
			_converter.GainBeer((int)(DatabaseManager.Instance.DailyRewards[index].BeerGain * mult));
			_converter.GainGold((int)(DatabaseManager.Instance.DailyRewards[index].GoldGain * mult));
			_converter.GainMithril((int)(DatabaseManager.Instance.DailyRewards[index].MithrilGain * mult));
			_converter.GainResource(JSonManager.Instance.PlayerProfile.LastFortress.ResourceProduced.Name, (int)(DatabaseManager.Instance.DailyRewards[index].ResourceGain * mult));
		}
	}
}