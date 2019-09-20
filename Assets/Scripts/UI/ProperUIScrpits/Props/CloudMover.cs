namespace DwarfClicker.UI.Props
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class CloudMover : MonoBehaviour
	{
		[SerializeField] private float _speed = 1f;
		[SerializeField] private int _xThreshold = 900;
		//[SerializeField] private 

		private bool _isMoving = false;
		private Vector3 _moveVector = Vector3.zero; 
		private Vector3 _startPos = Vector3.zero;
		private int _index = 0;

		public bool IsMoving { get { return _isMoving; } }

		private Action<int> _resetClouds = null;

		public event Action<int> ResetClouds
		{
			add
			{
				_resetClouds -= value;
				_resetClouds += value;
			}
			remove
			{
				_resetClouds -= value;
			}
		}

		private void Start()
		{
			_moveVector.x = _speed;
			_startPos = transform.localPosition;
		}

		private void Update()
		{
			if (_isMoving)
			{
				transform.position += _moveVector;
			}


			
			if (transform.localPosition.x > _xThreshold)
				Reset();
		}

		public void Reset()
		{
			transform.localPosition = _startPos;
			_isMoving = false;
			if (_resetClouds != null)
				_resetClouds(_index);
		}

		public void ToggleMove(int index)
		{
			_isMoving = !_isMoving;
			_index = index;
		}
	}
}