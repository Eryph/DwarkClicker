namespace Engine.Manager
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Engine.Utils;
	using UnityEngine.SceneManagement;
	using UnityEngine.Assertions;
	using System.IO;
	using DwarkClicker.Helper;
	using DwarfClicker.UI.GainRecap;

	public class SoundManager : Singleton<SoundManager>
	{
		#region Fields
		private bool _isSoundMuted = false;
		private bool _isMusicMuted = false;
		#endregion Fields

		#region Properties
		public bool IsSoundMuted {
			get { return _isSoundMuted; }
			set
			{
				_isSoundMuted = value;
				JSonManager.Instance.PlayerProfile._isSoundMuted = value;
			}
		}

		public bool IsMusicMuted
		{
			get { return _isMusicMuted; }
			set
			{
				_isMusicMuted = value;
				JSonManager.Instance.PlayerProfile._isMusicMuted = value;
			}
		}
		#endregion Properties

		#region Events
		private Action _muteSoundEvent = null;

		public event Action MuteSoundEvent
		{
			add
			{
				_muteSoundEvent -= value;
				_muteSoundEvent += value;
			}
			remove
			{
				_muteSoundEvent -= value;
			}
		}

		private Action _muteMusicEvent = null;

		public event Action MuteMusicEvent
		{
			add
			{
				_muteMusicEvent -= value;
				_muteMusicEvent += value;
			}
			remove
			{
				_muteMusicEvent -= value;
			}
		}

		private Action<AudioClip> _playMusicEvent = null;

		public event Action<AudioClip> PlayMusicEvent
		{
			add
			{
				_playMusicEvent -= value;
				_playMusicEvent += value;
			}
			remove
			{
				_playMusicEvent -= value;
			}
		}

		private Action<AudioClip> _playSoundEvent = null;

		public event Action<AudioClip> PlaySoundEvent
		{
			add
			{
				_playSoundEvent -= value;
				_playSoundEvent += value;
			}
			remove
			{
				_playSoundEvent -= value;
			}
		}
		#endregion Events

		#region Methods
		#endregion Monobehaviour
		#region Monobehaviour

		public void PlayMusic(string musicTag = "Random")
		{
			AudioClip music = DatabaseManager.Instance.ExtractMusic(musicTag);
			_playMusicEvent(music);
		}

		public void PlaySound(string soundTag = "Random")
		{
			AudioClip sound = DatabaseManager.Instance.ExtractSound(soundTag);
			_playSoundEvent(sound);
		}
		#endregion Methods
	}
}
