namespace Preprod
{
	using DwarfClicker.Core.Containers;
	using Engine.Manager;
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;

	public class InventoryDisplayerDEBUG : MonoBehaviour
	{
		[SerializeField] private Transform _resourceContent = null;
		[SerializeField] private Transform _weaponContent = null;

		private PlayerProfile _profile = null;
		private void Start()
		{
			_profile = JSonManager.Instance.PlayerProfile;
			_profile.OnInventoryChange += UpdateDisplay;
			_profile.OnFortressChange += UpdateDisplay;
			UpdateDisplay();
		} 

		private void UpdateDisplay()
		{
			int i = 0;
			int y = 0;
			foreach (KeyValuePair<string, Resource> resource in _profile.Resources)
			{
				_resourceContent.GetChild(i).GetComponent<TextMeshProUGUI>().text = "" + resource.Value.Count + "x " + resource.Key;
				i++;
			}

			foreach (KeyValuePair<string, Weapon> weapon in _profile.Weapons)
			{
				_weaponContent.GetChild(y).GetComponent<TextMeshProUGUI>().text = "" + weapon.Value.Count + "x " + weapon.Key;
				y++;
			}

			for (int j = i; j < _resourceContent.childCount; j++)
			{
				
				_resourceContent.GetChild(j).gameObject.SetActive(false);
			}

			for (int k = y; k < _weaponContent.childCount; k++)
			{
				_weaponContent.GetChild(k).gameObject.SetActive(false);
			}
		}
	}
}