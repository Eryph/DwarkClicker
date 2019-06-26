namespace DwarfClicker.Misc
{
	using UnityEngine;

	public class FXController : MonoBehaviour
	{
		[SerializeField] private Transform _container = null;
		[SerializeField] private GameObject _tapParticle = null;
		[SerializeField] private GameObject _polteringParticle = null;

		public void CreateBeerTapParticle()
		{
			GameObject tapParticle = Instantiate(_tapParticle, _container);
			Vector3 screenPoint = Input.mousePosition;
			screenPoint.z = 90f; //distance of the plane from the camera
			tapParticle.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
		}

		public void CreatePolteringParticle()
		{
			GameObject tapParticle = Instantiate(_polteringParticle, _container);
			Vector3 screenPoint = Input.mousePosition;
			screenPoint.z = 90f; //distance of the plane from the camera
			tapParticle.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
		}
	}
}