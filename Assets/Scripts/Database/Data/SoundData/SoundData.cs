namespace DwarfClicker.Core.Data
{
	using Engine.Manager;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Sound", menuName = "Data/Sound")]
	public class SoundData : ScriptableObject
	{
		[SerializeField] private string _soundTag = "";
		[SerializeField] private AudioClip _sound = null;

		public string SoundTag { get { return _soundTag; } }
		public AudioClip Sound { get { return _sound; } }
	}
}