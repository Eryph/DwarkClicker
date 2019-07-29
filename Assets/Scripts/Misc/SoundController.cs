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

		private void Start()
		{
			SoundManager.Instance.PlayMusicEvent += PlayMusic;
			SoundManager.Instance.PlaySoundEvent += PlaySound;
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
	}
}