namespace DwarfClicker.Core.Data
{
	using Engine.Manager;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Music", menuName = "Data/Music")]
	public class MusicData : ScriptableObject
	{
		[SerializeField] private string _musicTag = "";
		[SerializeField] private AudioClip _music = null;

		public string MusicTag { get { return _musicTag; } }
		public AudioClip Music { get { return _music; } }
	}
}