namespace DwarfClicker.Misc
{
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SoundController : MonoBehaviour
	{
		[SerializeField] private AudioSource _musicSource = null;
		[SerializeField] private AudioSource _soundSource = null;
		private bool _isSoundMuted = false;
		private bool _isMusicMuted = false;

		private void Start()
		{
			SoundManager.Instance.PlayMusicEvent += PlayMusic;
			SoundManager.Instance.PlaySoundEvent += PlaySound;
			SoundManager.Instance.MuteSoundEvent += MuteSoundToggle;
			SoundManager.Instance.MuteMusicEvent += MuteMusicToggle;
			_isSoundMuted = JSonManager.Instance.PlayerProfile._isSoundMuted;
			_isMusicMuted = JSonManager.Instance.PlayerProfile._isMusicMuted;
			MuteHandle();
			PlayMusic(DatabaseManager.Instance.ExtractMusic());
		}

		private void PlayMusic(AudioClip music)
		{
			_musicSource.loop = true;
			_musicSource.clip = music;
			_musicSource.Play();
		}

		private void PlaySound(AudioClip sound)
		{
			_musicSource.PlayOneShot(sound);
		}

		public void MuteMusicToggle()
		{
			_isMusicMuted = !_isMusicMuted;
			MuteHandle();
		}

		public void MuteSoundToggle()
		{
			_isSoundMuted = !_isSoundMuted;
			MuteHandle();
		}

		private void MuteHandle()
		{
			SoundManager.Instance.IsSoundMuted = _isSoundMuted;
			SoundManager.Instance.IsMusicMuted = _isMusicMuted;
			if (_isSoundMuted == false)
			{
				_soundSource.volume = 1;
			}
			else
			{
				_soundSource.volume = 0;
			}

			if(_isMusicMuted == false)
			{
				_musicSource.volume = 1;
			}
			else
			{
				_musicSource.volume = 0;
			}
		}
	}
}