namespace DwarfClicker.UI.ShopPanel
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;

	public class IAPButtonTMPro : MonoBehaviour {

		[SerializeField] private int _productIndex = 0;

		[SerializeField] private ShopPanel _shopPanel = null;
		[SerializeField] private TextMeshProUGUI _description = null;
		[SerializeField] private TextMeshProUGUI _price = null;
		[SerializeField] private TextMeshProUGUI _title = null;

		private void OnEnable()
		{
			Display();
		}

		public void Display()
		{
		}
	}
}