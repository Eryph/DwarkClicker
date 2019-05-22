namespace DwarfClicker.Core
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class BuildingBase : MonoBehaviour
	{
		protected bool _isPaused = false;

		public bool IsPaused { get { return _isPaused; } }

		private Action<bool> _onPause = null;

		public event Action<bool> OnPause
		{
			add
			{
				_onPause -= value;
				_onPause += value;
			}
			remove
			{
				_onPause -= value;
			}
		}

		public void TogglePause()
		{
			_isPaused = !_isPaused;
			if (_onPause != null)
			{
				_onPause(_isPaused);
			}
		}
	}
}