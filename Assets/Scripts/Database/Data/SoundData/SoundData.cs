namespace DwarfClicker.Core.Data
{
	using Engine.Manager;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Sound", menuName = "Data/Sound")]
	public class SoundData : ScriptableObject
	{
		[SerializeField] private string _soundTag = "";
		[SerializeField] private AudioClip[] _sound = null;

		public string SoundTag { get { return _soundTag; } }
		public AudioClip Sound { get { return _sound[0]; } }
		public AudioClip RandomSound { get { return _sound[Random.Range(0, _sound.Length)]; } }
	}
}