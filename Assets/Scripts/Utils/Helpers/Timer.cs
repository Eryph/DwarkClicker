namespace Engine.Utils
{
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Timer
	{
		#region Fields
		private float _timeStamp = 0;
		private bool _isStopped = false;
		#endregion Fields

		#region Properties
		public float TimeLeft { get { return (_timeStamp - Time.time); } }
		public bool IsStopped { get { return _isStopped; } set { _isStopped = value; } }
		#endregion Properties

		#region Methods
		public void ReduceRemainingTime(float value)
		{
			_timeStamp -= value;
		}

		public void ResetTimer(float time)
		{
			_timeStamp = Time.time + time;
		}

		public bool IsTimerEnd()
		{
			if (_isStopped)
			{
				return false;
			}
			return (TimeLeft <= 0);
		}

		public void Stop()
		{
			_timeStamp = Time.time;
			_isStopped = true;
		}
		#endregion Methods
	}
}