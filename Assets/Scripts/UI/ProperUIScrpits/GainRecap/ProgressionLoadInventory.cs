namespace DwarfClicker.UI.GainRecap
{
	using DwarfClicker.Core.Containers;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ProgressionLoadInventory
	{
		private Dictionary<string, int> _producedWeapons;
		private Dictionary<string, int> _producedResources;
		private Dictionary<string, int> _consumedWeapons;
		private Dictionary<string, int> _consumedResources;
		private int _gold;
		private int _mithril;
		private TimeSpan _timePassed;
		private bool _hasChanges = false;

		public Dictionary<string, int> ProducedWeapons { get { return _producedWeapons; } }
		public Dictionary<string, int> ProducedResource { get { return _producedResources; } }
		public Dictionary<string, int> ConsumedWeapons { get { return _consumedWeapons; } }
		public Dictionary<string, int> ConsumedResource { get { return _consumedResources; } }
		public int Gold { get { return _gold; } }
		public int Mithril { get { return _mithril; } }
		public TimeSpan TimePassed { get { return _timePassed; } }
		public bool HasChanges { get { return _hasChanges; } }

		public void Init()
		{
			_producedWeapons = new Dictionary<string, int>();
			_producedResources = new Dictionary<string, int>();
			_consumedWeapons = new Dictionary<string, int>();
			_consumedResources = new Dictionary<string, int>();
		}

		public void SetTimePassed(TimeSpan timePassed)
		{
			_timePassed = timePassed;
		}

		public void SetConsumedWeapon(string weaponName, int count)
		{
			if (HasChanges == false && count != 0)
				_hasChanges = true;
			if (ConsumedWeapons.ContainsKey(weaponName))
			{
				ConsumedWeapons[weaponName] += count;
			}
			else
			{
				ConsumedWeapons.Add(weaponName, count);
			}
		}

		public void SetConsumedResource(string resourceName, int count)
		{
			if (HasChanges == false && count != 0)
				_hasChanges = true;
			if (ConsumedResource.ContainsKey(resourceName))
			{
				ConsumedResource[resourceName] += count;
			}
			else
			{
				ConsumedResource.Add(resourceName, count);
			}
		}

		public void SetProducedWeapon(string weaponName, int count)
		{
			if (HasChanges == false && count != 0)
				_hasChanges = true;
			if (ProducedWeapons.ContainsKey(weaponName))
			{
				ProducedWeapons[weaponName] += count;
			}
			else
			{
				ProducedWeapons.Add(weaponName, count);
			}
		}

		public void SetProducedResource(string resourceName, int count)
		{
			if (HasChanges == false && count != 0)
				_hasChanges = true;
			if (ProducedResource.ContainsKey(resourceName))
			{
				ProducedResource[resourceName] += count;
			}
			else
			{
				ProducedResource.Add(resourceName, count);
			}
		}

		public void SetGold(int count)
		{
			if (HasChanges == false && count != 0)
				_hasChanges = true;
			_gold += count;
		}

		public void SetMithril(int count)
		{
			if (HasChanges == false && count != 0)
				_hasChanges = true;
			_mithril += count;
		}
	}
}