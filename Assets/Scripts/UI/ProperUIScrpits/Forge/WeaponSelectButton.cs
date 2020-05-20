namespace DwarfClicker.UI.Forge
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;
	using Core.Data;
	using Core.Containers;

	public class WeaponSelectButton : MonoBehaviour
	{
		#region Fields
		[SerializeField] private Sprite _maxStream = null;
		[SerializeField] private Sprite _upStream = null;
		[SerializeField] private Sprite _downStream = null;
		[SerializeField] private Sprite _minStream = null;

		[SerializeField] private Sprite _offBgSprite = null;
		[SerializeField] private Sprite _onBgSprite = null;
		[SerializeField] private Image _bg = null;

		[SerializeField] private TextMeshProUGUI _weaponName = null;
		[SerializeField] private TextMeshProUGUI _weaponPrice = null;
		[SerializeField] private Image _marketStreamImage = null;
		[SerializeField] private Image _itemImage = null;

		[SerializeField] private Button _button = null;

        private bool _isSelected = false;

		private bool _min = false;
		private bool _max = false;
		#endregion Fields

		#region Methods
		private void Start()
		{
			_button = GetComponent<Button>();
		}

		public void Init(Weapon weapon, WeaponData weaponData)
		{
			_min = weapon.SellPrice == weaponData.PriceMin;
			_max = weapon.SellPrice == weaponData.PriceMax;
			_button = GetComponent<Button>();
			_weaponName.text = weapon.Name;
			_weaponPrice.text = weapon.SellPrice.ToString();
			_itemImage.sprite = weaponData.WeaponSprite;
		}

		public void Select()
		{
			if (_min)
				_marketStreamImage.sprite = _minStream;
			else
				_marketStreamImage.sprite = _downStream;
			_button.interactable = false;
			_isSelected = true;
				_bg.sprite = _onBgSprite;
		}

		public void Deselect()
		{
			_bg.sprite = _offBgSprite;
			if (_max)
				_marketStreamImage.sprite = _maxStream;
			else
				_marketStreamImage.sprite = _upStream;
			_button.interactable = true;
			_isSelected = false;
		}
		#endregion Methods
	}
}
