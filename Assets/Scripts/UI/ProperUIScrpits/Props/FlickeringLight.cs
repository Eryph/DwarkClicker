namespace DwarfClicker.UI.Props
{
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class FlickeringLight : MonoBehaviour
	{
		// ATTRIBUTES
		// Fields
		[SerializeField] private float _minScale = 0.2f;
		[SerializeField] private float _maxScale = 0.8f;
		[SerializeField] private float _lerpSpeed = 0.5f;
		// Fields

		private float _startScaleValueX = 0f;
		private float _startScaleValueY = 0f;
		private float _targetScale = 0f;
		private float _t = 0;
		// ATTRIBUTES

		// METHODS
		private void Start()
		{
			Restart();
			GameLoopManager.Instance.GameLoop += Loop;
		}

		private void Loop()
		{
			float i = 0;
			i = Mathf.Lerp(_startScaleValueX, _targetScale, _t);
			_t += _lerpSpeed;
			transform.localScale = new Vector3(i, i, 0);
			if (_t >= 1)
			{
				Restart();
			}
		}

		private void Restart()
		{
			_t = 0;
			_targetScale = Random.Range(_minScale, _maxScale);
			_startScaleValueX = transform.localScale.x;
			_startScaleValueY = transform.localScale.y;
		}
		// METHODS
	}
}