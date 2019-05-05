namespace Core.Data
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(fileName = "WeaponList", menuName = "Data/WeaponList")]
	public class WeaponListData : ScriptableObject
	{
		[SerializeField] private WeaponData[] _weapons = null;

		public WeaponData[] Weapons { get { return _weapons; } }
	}
}