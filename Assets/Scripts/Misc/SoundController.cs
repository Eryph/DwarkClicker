namespace DwarfClicker.Misc
{
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SoundController : MonoBehaviour
	{
		[SerializeField] private AudioSource _musicSource = null;
		[SerializeField] private AudioSource _soundSource = null;
		[SerializeField] private AudioSource _polteringSource = null;

		[SerializeField] private float _beerSoundLifeTime = 0.5f;
		private bool _isSoundMuted = false;
		private bool _isMusicMuted = false;

		private Timer _timer = null;

		private void Start()
		{
			SoundManager.Instance.PlayMusicEvent += PlayMusic;
			SoundManager.Instance.PlaySoundEvent += PlaySound;
			SoundManager.Instance.MuteSoundEvent += MuteSoundToggle;
			SoundManager.Instance.MuteMusicEvent += MuteMusicToggle;
			_isSoundMuted = JSonManager.Instance.PlayerProfile._isSoundMuted;
			_isMusicMuted = JSonManager.Instance.PlayerProfile._isMusicMuted;
			_timer = new Timer();
			MuteHandle();
			PlayMusic(DatabaseManager.Instance.ExtractMusic());
		}

		private void Update()
		{
			if (_timer != null && !_timer.IsStopped && _timer.TimeLeft <= 0)
			{
				_timer.Stop();
				_polteringSource.Stop();
			}
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

		public void PlayPoltering(string soundTag)
		{
			_polteringSource.PlayOneShot(DatabaseManager.Instance.ExtractRandomSound(soundTag));
		}

		public void PlayBeerSound()
		{
			if (_timer.IsStopped)
			{
				_polteringSource.clip = DatabaseManager.Instance.ExtractSound("POLTERING_INN");
				_polteringSource.Play();
			}
			_timer.ResetTimer(_beerSoundLifeTime);
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
				_polteringSource.volume = 1;
			}
			else
			{
				_polteringSource.volume = 0;
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

		#region SoundTrigger
		public void PlayStantdardSound()
		{
			SoundManager.Instance.PlaySound("STANDARD_CLICK");
		}
		#endregion SoundTrigger
	}
}