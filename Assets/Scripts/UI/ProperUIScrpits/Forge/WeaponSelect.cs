namespace DwarfClicker.UI.Forge
{
	using Core.Data;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class WeaponSelect : MonoBehaviour
	{
		#region Fields
		[SerializeField] private WeaponSelectButton[] _weaponsSelectButtons = null;
		#endregion Fields

		#region Methods
		private void OnEnable()
		{
			WeaponData[] weapons = JSonManager.Instance.PlayerProfile.CurrentFortress._weapons;
			int currentCraft = JSonManager.Instance.PlayerProfile.CurrentFortress._weaponIndex;

			for (int i = 0; i < _weaponsSelectButtons.Length; i++)
			{
				if (i < weapons.Length)
				{
					_weaponsSelectButtons[i].gameObject.SetActive(true);
					_weaponsSelectButtons[i].Init(JSonManager.Instance.PlayerProfile.Weapons[weapons[i].Name], weapons[i]);
					if (i == currentCraft)
					{
						_weaponsSelectButtons[i].Select();
					}
				}
				else
				{
					_weaponsSelectButtons[i].gameObject.SetActive(false);
				}
			}
		}

		public void SelectWeapon(int selectedIndex)
		{
			for (int i = 0; i < _weaponsSelectButtons.Length; i++)
			{
				if (_weaponsSelectButtons[i].gameObject.activeSelf)
				{
					if (selectedIndex == i)
					{
						_weaponsSelectButtons[i].Select();
						JSonManager.Instance.PlayerProfile.CurrentFortress.WeaponIndex = i;
					}
					else
					{
						_weaponsSelectButtons[i].Deselect();
					}
				}
			}
		}
		#endregion Methods
	}
}