namespace Core.Data
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "ResourceList", menuName = "Data/ResourceList")]
	public class ResourceListData : ScriptableObject
	{
		[SerializeField] private ResourceData[] _resources = null;

		public ResourceData[] Resources { get { return _resources; } }
	}
}