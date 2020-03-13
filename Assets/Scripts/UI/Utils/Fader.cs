namespace DwarfClicker.Misc
{
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class Fader : MonoBehaviour
	{
		[SerializeField] private Image _imageToFade = null;
		[SerializeField] private GameObject _text = null;
		[SerializeField] private float _speed = 1;

		private Color _startColor = Color.black;
		private Color _endColor = Color.black;
		private float _t = 0;

		private void Start()
		{
			_endColor.a = 0;
		}

		public void FadeStart()
		{
			_text.SetActive(false);
			GameLoopManager.Instance.GameLoop += Fade;
		}

		private void Fade()
		{
			_t += _speed * Time.deltaTime;
			_imageToFade.color = Color.Lerp(_startColor, _endColor, _t);
			if (_t >= 1)
			{
				GameLoopManager.Instance.GameLoop -= Fade;
				gameObject.SetActive(false);
			}
		}
	}
}