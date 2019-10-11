namespace DwarfClicker.UI.GainRecap
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using TMPro;

	public class ScrollIconHandler : MonoBehaviour
	{
		[SerializeField] private Image _icon = null;
		[SerializeField] private TextMeshProUGUI _text = null;

		public void Init(Sprite itemSprite, int amount)
		{
			_icon.sprite = itemSprite;
			_text.text = amount.ToString();
		}
	}
}