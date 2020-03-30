namespace DwarfClicker.UI.TradingPost
{
	using DwarkClicker.Helper;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class UpgradeButtonHandler : MonoBehaviour
	{
		[SerializeField] private Sprite _offBgSprite = null;
		[SerializeField] private Sprite _onBgSprite = null;
		[SerializeField] private Image _bg = null;

		[SerializeField] private TextMeshProUGUI _upgradeName = null;
		[SerializeField] private TextMeshProUGUI _upgradeDesc = null;
		[SerializeField] private TextMeshProUGUI _price = null;
		[SerializeField] private TextMeshProUGUI _currentRank = null;

		public void Init(string upgradeName, string upgradeDesc, int currentRank, int price, int currentAmount)
		{
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;
			if (currentAmount >= price)
			{
				_bg.sprite = _onBgSprite;
			}
			else
			{
				_bg.sprite = _offBgSprite;
			}
			_upgradeName.text = upgradeName;
			_upgradeDesc.text = upgradeDesc;
			_price.text = UIHelper.FormatIntegerString(price);

            _currentRank.text = "Rank : " + UIHelper.FormatIntegerString(currentRank);
		}
	}
}