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
		[SerializeField] private TextMeshProUGUI _weaponName = null;
		[SerializeField] private TextMeshProUGUI _weaponPrice = null;
		[SerializeField] private Image _marketStreamImage = null;

		private Button _button = null;
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
			_min = weapon.SellPrice == weaponData.PriceModifierDown;
			_max = weapon.SellPrice == weaponData.PriceModifierUp;
			_button = GetComponent<Button>();
			_weaponName.text = weapon.Name;
			_weaponPrice.text = weapon.SellPrice.ToString();
			_marketStreamImage.color = Color.blue;
		}

		public void Select()
		{
			if (_min)
				_marketStreamImage.color = Color.red;
			else
				_marketStreamImage.color = Color.magenta;
			_button.interactable = false;
			_isSelected = true;
		}

		public void Deselect()
		{
			if (_max)
				_marketStreamImage.color = Color.green;
			else
				_marketStreamImage.color = Color.blue;
			_button.interactable = true;
			_isSelected = false;
		}
		#endregion Methods
	}
}
