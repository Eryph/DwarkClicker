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
	using UnityEngine.Advertisements;
    using DwarfClicker.UI.PopUp;

    public class DailyRewardPanel : MonoBehaviour
	{
		[SerializeField] private DailyRewardController _controller = null;
		[SerializeField] private DailyRewardBox[] _dailyRewardBox = null;
		[SerializeField] private GameObject _alreadyRedeemeedeemButton = null;
		[SerializeField] private AllDataDisplayer _header = null;
		[SerializeField] private float _adRewardMult = 2;
        [SerializeField] private PopUpWindowController _popUpWindowController = null;

        private PlayerProfile _profile = null;

		private void OnEnable()
		{
            if (_profile == null)
			{
				_profile = JSonManager.Instance.PlayerProfile;
			}
			Display();
		}

        private void OnDisable()
        {
        }

        private void Display()
		{
			_alreadyRedeemeedeemButton.SetActive(false);
			if (_profile._dailyRewardIndex != 0)
			{
				if (_profile.IsDailyRewardAvailable)
				{
					_alreadyRedeemeedeemButton.SetActive(false);
				}
				else
				{
					if (_profile.IsDailyRewardReset)
					{
						_profile._dailyRewardIndex = 0;
					}
					_alreadyRedeemeedeemButton.SetActive(true);
				}
			}

			for (int i = 0; i < _dailyRewardBox.Length; i++)
			{
				if (i < _profile._dailyRewardIndex)
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
						_dailyRewardBox[i].Init((int)DatabaseManager.Instance.DailyRewards[i].GoldGain, DatabaseManager.Instance.GoldIcon, Color.white);
					}
					if (DatabaseManager.Instance.DailyRewards[i].ResourceGain > 0)
					{
						ResourceData resource = _profile.LastFortress.ResourceProduced;
						_dailyRewardBox[i].Init(DatabaseManager.Instance.DailyRewards[i].ResourceGain, resource.ResourceSprite, Color.white);
					}
					if (DatabaseManager.Instance.DailyRewards[i].MithrilGain > 0)
					{
						_dailyRewardBox[i].Init(DatabaseManager.Instance.DailyRewards[i].MithrilGain, DatabaseManager.Instance.MithrilIcon, Color.white);
					}

					if (i == _profile._dailyRewardIndex)
					{
						_dailyRewardBox[i].SetAvailable();
					}

					if (i > _profile._dailyRewardIndex)
					{
						_dailyRewardBox[i].SetNotAvailable();
					}
				}
			}
		}

		public void RedeemDailyReward()
		{
			_controller.RedeemDailyReward();
			_header.SetDailyRewardButtonDisabled();
			_alreadyRedeemeedeemButton.SetActive(true);
			Display();
            _popUpWindowController.Display(0, "Daily reward redeemed !");
		}

		public void RedeemAdDailyReward()
		{
			_controller.RedeemDailyReward(_adRewardMult);
			_header.SetDailyRewardButtonDisabled();
			_alreadyRedeemeedeemButton.SetActive(true);
			Display();
			MonetizationManager.Instance.AdFinished -= RedeemAdDailyReward;
            _popUpWindowController.Display(1, "Transaction succesfull,\nDaily rewards doubled.");
        }

		public void LaunchAd()
		{

			if (!JSonManager.Instance.PlayerProfile._noMoreAdsBonus)
			{
				MonetizationManager.Instance.ShowAd();
				MonetizationManager.Instance.AdFinished += RedeemAdDailyReward;
			}
			else
			{
				RedeemAdDailyReward();
			}
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}