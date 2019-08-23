namespace DwarfClicker.UI
{
	using DwarfClicker.Misc;
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ConfigPanel : MonoBehaviour
	{
		[SerializeField] private GameObject _resetProfileConfimationPanel = null;
		[SerializeField] private SoundController _soundController = null;

		private void OnEnable()
		{
			
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}

		public void MuteSound()
		{
			_soundController.MuteSoundToggle();
		}

		public void MuteMusic()
		{
			_soundController.MuteMusicToggle();
		}

		public void ResetProfileButton()
		{
			_resetProfileConfimationPanel.SetActive(true);
		}

		public void QuitResetProfilePanel()
		{
			_resetProfileConfimationPanel.SetActive(false);
		}

		public void ResetProfileConfirmButton()
		{
			QuitResetProfilePanel();
			JSonManager.Instance.PlayerProfile.Reset();
		}
	}
}