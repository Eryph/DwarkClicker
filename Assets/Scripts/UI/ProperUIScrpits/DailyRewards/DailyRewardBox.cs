namespace DwarfClicker.UI.DailyReward
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;
	using Engine.Manager;

	public class DailyRewardBox : MonoBehaviour
	{
		[SerializeField] private Image _backgroundSprite = null;
		[SerializeField] private TextMeshProUGUI _amount = null;
		[SerializeField] private Image _rewardSprite = null;
		[SerializeField] private GameObject _alreadyRedeemedPanel = null;
		[SerializeField] private Sprite _available = null;
		[SerializeField] private Sprite _notAvailable = null;

		private Sprite _startSprite = null;

		private void OnEnable()
		{
			_startSprite = _backgroundSprite.sprite;
		}

		public void Init(int amount, Sprite sprite, Color color)
		{
			_amount.text = amount.ToString();
			_rewardSprite.sprite = sprite;
			_rewardSprite.color = color;
			_alreadyRedeemedPanel.SetActive(false);
		}

		public void SetAlreadyRedeemed()
		{
			_alreadyRedeemedPanel.SetActive(true);
		}

		public void SetAvailable()
		{
			_backgroundSprite.sprite = _available;
		}

		public void SetNotAvailable()
		{
			_backgroundSprite.sprite = _notAvailable;
		}
	}
}