namespace DwarfClicker.UI
{
	using DwarfClicker.Misc;
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;

    public class ConfigPanel : MonoBehaviour
	{
		[SerializeField] private GameObject _resetProfileConfimationPanel = null;
		[SerializeField] private SoundController _soundController = null;

        [Header("Buttons")]
        [SerializeField] private Image _soundMutedButton = null;
        [SerializeField] private Image _musicMutedButton = null;

        [Header("Sprites")]
        [SerializeField] private Sprite _soundMutedSprite = null;
        [SerializeField] private Sprite _soundNotMutedSprite = null;
        [SerializeField] private Sprite _musicMutedSprite = null;
        [SerializeField] private Sprite _musicNotMutedSprite = null;

        private void OnEnable()
		{
			
		}

        private void Start()
        {
            UpdateMuteButtons();
        }

        public void QuitPanel()
		{
			gameObject.SetActive(false);
		}

		public void MuteSound()
		{
			_soundController.MuteSoundToggle();
            UpdateMuteButtons();
        }

		public void MuteMusic()
		{
            _soundController.MuteMusicToggle();
            UpdateMuteButtons();
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
            Application.Quit();
		}

        private void UpdateMuteButtons()
        {
            if (!SoundManager.Instance.IsMusicMuted)
                _musicMutedButton.sprite = _musicMutedSprite;
            else
                _musicMutedButton.sprite = _musicNotMutedSprite;
            if (SoundManager.Instance.IsSoundMuted)
                _soundMutedButton.sprite = _soundMutedSprite;
            else
                _soundMutedButton.sprite = _soundNotMutedSprite;
        }

        public void OpenTwitter()
        {
            Application.OpenURL("https://twitter.com/DwarfClicker?s=09");
        }

        public void OpenFacebook()
        {
            Application.OpenURL("https://www.facebook.com/DwarfClicker/");
        }
    }
}