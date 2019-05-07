namespace DwarfClicker.Core.Containers
{
	using DwarfClicker.Core.Data;
	using System;
	using UnityEngine;

	[Serializable]
	public class Resource
	{
		#region Fields
		[SerializeField] private int _UID = 0;
		[SerializeField] private string _name = "resourceName";
		[SerializeField] private int _count = 0;
		#endregion Fields

		#region Properties
		public int UID { get { return _UID; } }
		public string Name { get { return _name; } }
		public int Count { get { return _count; } }
		#endregion Properties

		#region Methods


		public void Init(ResourceData resource)
		{
			_UID = resource.UID;
			_name = resource.Name;
		}

		public void UpdateCount(int count)
		{
			_count += count;
		}

		public void ResetCount()
		{
			_count = 0;
		}
		#endregion Methods
	}
}