namespace DwarfClicker.UI.Props
{
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class CloudSpawner : MonoBehaviour
	{
		[SerializeField] private List<CloudMover> _cloud = null;
		[SerializeField] private float _coolDown = 100f;
		[SerializeField] private float _coolDownRandomThreshold = 5;

		private List<int> _cloudMovedIndex = null;
		private Timer _timer = null;
		// Use this for initialization
		void Start()
		{
			_cloudMovedIndex = new List<int>();
			_timer = new Timer();
			_timer.ResetTimer(1);
		}

		// Update is called once per frame
		void Update()
		{
			int index = 0;
			if (_timer.TimeLeft < 0)
			{
				do
				{
					index = Random.Range(0, _cloud.Count);
					if (_cloud.Count == _cloudMovedIndex.Count)
					{
						Debug.LogError("Clouds reset never happened. Quit Destroying cloud spawner");
						Destroy(gameObject);
					}
				} while (_cloudMovedIndex.Contains(index));
				_cloud[index].ToggleMove(index);
				_cloud[index].ResetClouds += Erase;
				_cloudMovedIndex.Add(index);
				_timer.ResetTimer(Random.Range(_coolDown - _coolDownRandomThreshold, _coolDown + _coolDownRandomThreshold));

            }
		}

		private void Erase(int index)
		{
			_cloudMovedIndex.Remove(index);
			_cloud[index].ResetClouds -= Erase;
		}

		
	}
}