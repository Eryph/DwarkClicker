namespace Engine.UI.Utils
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Engine.Manager;

	public class ProgressionBarHandler : MonoBehaviour
	{
		#region Fields
		[SerializeField] private RectTransform _bar = null;

		private Vector3 _fullPosition = Vector3.zero;
		private Vector3 _emptyPosition = Vector3.zero;
		#endregion Fields

		#region Methods
		private void Start()
		{
			_fullPosition = _bar.localPosition;
			_emptyPosition = _bar.localPosition - new Vector3(126, 0, 0);
			SetEmpty();
		}

		public void UpdateBar(float t)
		{
			if (t == 1 || t == 0)
			{
				SetEmpty();
			}
			else
			{
				_bar.localPosition = Vector3.Lerp(_emptyPosition, _fullPosition, t);
			}
		}

		public void SetEmpty()
		{
			_bar.localPosition = _emptyPosition;
		}

		public void SetFull()
		{
			_bar.localPosition = _fullPosition;
		}
		#endregion Methods
	}
}