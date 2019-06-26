namespace Engine.Utils
{
	using UnityEngine;

	public class SelfDestruct : MonoBehaviour
	{
		[SerializeField] private float _lifeTime = 5f;

		private void Start()
		{
			Destroy(gameObject, _lifeTime);
		}
	}
}