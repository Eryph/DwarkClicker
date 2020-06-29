namespace DwarfClicker.UI.KingPanel
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;
	using DwarfClicker.Core.Containers;
	using Engine.Manager;
    using DwarkClicker.Helper;

    public class InventoryCell : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _name = null;
		[SerializeField] private TextMeshProUGUI _amount = null;
		[SerializeField] private Image _image = null;

		public TextMeshProUGUI Name { get { return _name; } }
		public TextMeshProUGUI Amount { get { return _amount; } }
		public Image ItemImage { get { return _image; } }

		public void Init(Sprite sprite, ulong amount)
		{
			_amount.text = UIHelper.FormatIntegerString(amount);
			_image.sprite = sprite;
		}

        public void Init(Sprite sprite, int amount)
        {
            _amount.text = UIHelper.FormatIntegerString(amount);
            _image.sprite = sprite;
        }

        public void Init(Resource resource)
		{
			//_name.text = resource.Name;
			_amount.text = resource.Count.ToString();
			_image.sprite = DatabaseManager.Instance.ExtractResource(resource.Name).ResourceSprite;
		}

		public void Init(Weapon weapon)
		{
			//_name.text = weapon.Name;
			_amount.text = weapon.Count.ToString();
			_image.sprite = DatabaseManager.Instance.ExtractWeapon(weapon.Name).WeaponSprite;
		}
	}
}