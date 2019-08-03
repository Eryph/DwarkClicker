namespace DwarfClicker.UI.KingPanel
{
	using DwarfClicker.Core.Containers;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class InventoryPanel : MonoBehaviour
	{
		[SerializeField] private InventoryCell _cell = null;
		[SerializeField] private Transform _container = null;
		private PlayerProfile _profile = null;

		private void Start()
		{
			_profile = JSonManager.Instance.PlayerProfile;
		}

		public void Display()
		{
			if (_profile == null)
			{
				_profile = JSonManager.Instance.PlayerProfile;
			}
			foreach (KeyValuePair<string, Resource> resource in _profile.Resources)
			{
				if (resource.Value.Count == 0)
				{
					continue;
				}
				GameObject cell = Instantiate(_cell.gameObject, _container);
				InventoryCell inventoryCell = cell.GetComponent<InventoryCell>();
				inventoryCell.Init(resource.Value);
			}
			foreach (KeyValuePair<string, Weapon> weapon in _profile.Weapons)
			{
				if (weapon.Value.Count == 0)
				{
					continue;
				}
				GameObject cell = Instantiate(_cell.gameObject, _container);
				InventoryCell inventoryCell = cell.GetComponent<InventoryCell>();
				inventoryCell.Init(weapon.Value);
			}
		}
	}
}