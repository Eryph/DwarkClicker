namespace Engine.Manager
{
	using Engine.Utils;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using UnityEngine;

	public class JSonManager : Singleton<JSonManager>
	{
		#region Fields
		[SerializeField] private string _jsonDataPath = "Profile.json";
		[SerializeField] private string _jsonNotifDataPath = "NotifProfile.json";

		private PlayerProfile _playerProfile = null;
		private NotifProfile _notifProfile = null;
		private string _dataPath = null;
		#endregion Fields

		#region Properties
		public PlayerProfile PlayerProfile { get { return _playerProfile; } }
		public NotifProfile NotifProfile { get { return _notifProfile; } }
		#endregion Properties

		#region Events

		private Action _onProfileLoaded = null;

		public event Action OnProfileLoaded
		{
			add
			{
				_onProfileLoaded -= value;
				_onProfileLoaded += value;
			}
			remove
			{
				_onProfileLoaded -= value;
			}
		}
		#endregion Events

		#region Methods
		protected override void Start()
		{
			base.Start();

#if ANDROID
			_dataPath = Application.persistentDataPath + "/";
#else
			_dataPath = Application.dataPath + "/";
#endif

			LoadPlayerProfile();
			LoadNotifProfile();
			_onProfileLoaded();
		}

		public void SavePlayerProfile()
		{
			string dataAsJson = JsonUtility.ToJson(_playerProfile);
			Debug.Log(dataAsJson);
			string filePath = Path.Combine(_dataPath, _jsonDataPath);
			if (!File.Exists(filePath))
			{
				File.Create(filePath).Close();
			}
			File.WriteAllText(filePath, dataAsJson);
		}
		
		public void SaveNotifProfile()
		{
			string dataAsJson = JsonUtility.ToJson(_notifProfile);
			Debug.Log(dataAsJson);
			string filePath = _dataPath + _jsonNotifDataPath;

			if (!File.Exists(filePath))
			{
				File.Create(filePath).Close();
			}
			File.WriteAllText(filePath, dataAsJson);
		}

		public void LoadPlayerProfile()
		{
			string filePath = _dataPath + _jsonDataPath;
			if (File.Exists(filePath))
			{
				string dataAsJson = File.ReadAllText(filePath);
				Debug.Log(dataAsJson);
				_playerProfile = JsonUtility.FromJson<PlayerProfile>(dataAsJson);
			}
			else
			{
				_playerProfile = new PlayerProfile();
				_playerProfile.Init();
			}
			Debug.Log(filePath);
			_playerProfile.DeserializeDate();
		}

		public void LoadNotifProfile()
		{
			string filePath = _dataPath + _jsonNotifDataPath;
			if (File.Exists(filePath))
			{
				string dataAsJson = File.ReadAllText(filePath);
				Debug.Log(dataAsJson);
				_notifProfile = JsonUtility.FromJson<NotifProfile>(dataAsJson);
			}
			else
			{
				_notifProfile = new NotifProfile();
				_notifProfile.Init();
			}
		}
	
		#endregion Methods
	}
}