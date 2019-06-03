namespace Engine.UI.FTUE
{
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;

	public class DialboxController : MonoBehaviour
	{
		[SerializeField] private Transform _content = null;
		[SerializeField] private int _posToSwitch = 1500;

		[SerializeField] private GameObject _topDialbox;
		[SerializeField] private GameObject _bottomDialbox;

		[SerializeField] private TextMeshProUGUI _topText = null;
		[SerializeField] private TextMeshProUGUI _bottomText = null;

		private void Start()
		{
			if (FTUEManager.Instance.Dialbox == null)
			{
				FTUEManager.Instance.SetDialbox(this);
			}
			else
			{
				Debug.LogError("Multiple DialboxController is not supported.");
			}

			FTUEManager.Instance.TriggerStep();
		}

		private void OnDisable()
		{
			_topDialbox.SetActive(false);
			_bottomDialbox.SetActive(false);
		}

		public void TriggerDialbox(string text)
		{
			if (FTUEManager.Instance.IsActivated)
			{
				gameObject.SetActive(true);
				_bottomDialbox.SetActive(true);
				_topDialbox.SetActive(false);
				_topText.text = text;
				_bottomText.text = text;
			}
			else
				DisableDialbox();
		}

		public void DisableDialbox()
		{
			gameObject.SetActive(false);
		}

		public void SetTopBottomDialbox()
		{
			if (FTUEManager.Instance.IsActivated)
			if (_content.position.y < _posToSwitch)
			{
				_bottomDialbox.SetActive(true);
				_topDialbox.SetActive(false);
			}
			else
			{
				_bottomDialbox.SetActive(false);
				_topDialbox.SetActive(true);
			}
		}

		//DEBUG

		public void SetStepFinished()
		{
			FTUEManager.Instance.StepFinished();
		}
	}
}