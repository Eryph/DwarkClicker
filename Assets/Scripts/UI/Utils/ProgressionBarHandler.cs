namespace Engine.UI.Utils
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Engine.Manager;
	using TMPro;
	using DwarkClicker.Helper;

	public class ProgressionBarHandler : MonoBehaviour
	{
		#region Fields
		[SerializeField] private RectTransform _bar = null;
		[SerializeField] private TextMeshProUGUI _consumedText = null;
		[SerializeField] private TextMeshProUGUI _producedText = null;

		private Vector3 _fullPosition = Vector3.zero;
		private Vector3 _emptyPosition = Vector3.zero;
		#endregion Fields

		#region Methods
		private void Start()
		{
			_fullPosition = _bar.localPosition + new Vector3(_bar.rect.width, 0, 0);
			_emptyPosition = _bar.localPosition;
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

		public void UpdateTexts(int consumed, int totalConsumed, int produced)
		{
			if (consumed > totalConsumed)
			{
				_consumedText.color = Color.red;
			}
			else
			{
				_consumedText.color = Color.white;
			}

			_consumedText.text = UIHelper.FormatIntegerString(consumed) + " / " + UIHelper.FormatIntegerString(totalConsumed);
			_producedText.text = UIHelper.FormatIntegerString(produced);
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