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
			int i = 0;

			GameObject tmpCell;
			if (_container.childCount <= i)
			{
				tmpCell = Instantiate(_cell.gameObject, _container);
			}
			InventoryCell tmpInventoryCell = _container.GetChild(i).GetComponent<InventoryCell>();
			tmpInventoryCell.Init(DatabaseManager.Instance.GoldIcon, _profile.Gold);
			i++;

			if (_container.childCount <= i)
			{
				tmpCell = Instantiate(_cell.gameObject, _container);
			}
			tmpInventoryCell = _container.GetChild(i).GetComponent<InventoryCell>();
			tmpInventoryCell.Init(DatabaseManager.Instance.MithrilIcon, _profile.Mithril);
			i++;

			foreach (KeyValuePair<string, Resource> resource in _profile.Resources)
			{
				if (resource.Value.Count == 0)
				{
					continue;
				}
				GameObject cell;
				if (_container.childCount <= i)
				{
					cell = Instantiate(_cell.gameObject, _container);
				}
				InventoryCell inventoryCell = _container.GetChild(i).GetComponent<InventoryCell>();
				inventoryCell.Init(resource.Value);
				i++;
			}
			foreach (KeyValuePair<string, Weapon> weapon in _profile.Weapons)
			{
				if (weapon.Value.Count == 0)
				{
					continue;
				}
				GameObject cell;
				if (_container.childCount <= i)
				{
					cell = Instantiate(_cell.gameObject, _container);
				}
				InventoryCell inventoryCell = _container.GetChild(i).GetComponent<InventoryCell>();
				inventoryCell.Init(weapon.Value);
				i++;
			}
		}
	}
}