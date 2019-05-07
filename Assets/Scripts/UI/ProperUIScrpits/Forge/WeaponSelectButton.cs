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
		[SerializeField] private Image _checkBoxImage = null;

		private Button _button = null;
		private bool _isSelected = false;
		#endregion Fields

		#region Methods
		private void Start()
		{
			_button = GetComponent<Button>();
		}

		public void Init(Weapon weapon, WeaponData weaponData)
		{
			_button = GetComponent<Button>();
			_weaponName.text = weapon.Name;
			_weaponPrice.text = weapon.SellPrice.ToString();
			_marketStreamImage.color = Color.green;
			_checkBoxImage.color = Color.white;
		}

		public void Select()
		{
			_marketStreamImage.color = Color.red;
			_checkBoxImage.color = Color.black;
			_button.interactable = false;
			_isSelected = true;
		}

		public void Deselect()
		{
			_marketStreamImage.color = Color.green;
			_checkBoxImage.color = Color.white;
			_button.interactable = true;
			_isSelected = false;
		}
		#endregion Methods
	}
}
