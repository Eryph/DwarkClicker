namespace DwarfClicker.UI.DailyReward
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;

	public class DailyRewardBox : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _amount = null;
		[SerializeField] private Image _sprite = null;
		[SerializeField] private GameObject _alreadyRedeemedPanel = null;
		[SerializeField] private GameObject _notAvailablePanel = null;

		public void Init(int amount, Sprite sprite, Color color)
		{
			_amount.text = amount.ToString();
			_sprite.sprite = sprite;
			_sprite.color = color;
			_alreadyRedeemedPanel.SetActive(false);
			_notAvailablePanel.SetActive(false);
		}

		public void SetAlreadyRedeemed()
		{
			_alreadyRedeemedPanel.SetActive(true);
		}

		public void SetNotAvailable()
		{
			_notAvailablePanel.SetActive(true);
		}
	}
}