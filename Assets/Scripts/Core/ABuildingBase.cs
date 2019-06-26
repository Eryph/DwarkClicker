namespace DwarfClicker.Core
{
	using DwarfClicker.Misc;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class ABuildingBase : MonoBehaviour
	{
		[SerializeField] protected FXController _FXController = null;

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

		public abstract void Poltering();

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