namespace Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "InnStartStats", menuName = "Data/InnStartStats")]
	public class InnStartData : ScriptableObject
	{
		[SerializeField] private int _beerByTap = 0;
		[SerializeField] private int _storage = 0;

		public int BeerByTap { get { return _beerByTap; } }
		public int Storage { get { return _storage; } }
	}
}