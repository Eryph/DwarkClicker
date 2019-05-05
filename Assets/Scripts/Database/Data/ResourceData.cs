namespace Core.Data
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Resource", menuName = "Data/Resource")]
	public class ResourceData : ScriptableObject
	{
		[SerializeField] private int _UID = 0;
		[SerializeField] private string _name = "resourceName";

		public int UID { get { return _UID; } }
		public string Name { get { return _name; } }
	}
}