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
		#endregion Fields

		#region Properties

		#endregion Properties

		#region Events
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
