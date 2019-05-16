namespace DwarfClicker.UI.TradingPost
{
	using DwarkClicker.Helper;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

	public class UpgradeButtonHandler : MonoBehaviour
	{

		[SerializeField] private TextMeshProUGUI _upgradeName = null;
		[SerializeField] private TextMeshProUGUI _price = null;
		[SerializeField] private TextMeshProUGUI _currentRank = null;

		public void Init(string upgradeName, int currentRank, int price)
		{
			_upgradeName.text = upgradeName;
			_price.text = "Price : " + UIHelper.FormatIntegerString(price);
			_currentRank.text = "Rank : " + UIHelper.FormatIntegerString(currentRank);
		}
	}
}