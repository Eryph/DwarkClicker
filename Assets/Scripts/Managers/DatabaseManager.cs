namespace Engine.Manager
{
	using Engine.Utils;
    using DwarfClicker.Core.Data;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using DwarfClicker.Database;

	public class DatabaseManager : Singleton<DatabaseManager>
	{
		#region Fields
		[Header("General Data")]
		[SerializeField] private float _polteringValue = 0.05f;

		[Header("DailyReward")]
		[SerializeField] private DailyReward[] _dailyRewards;

		[Header("Music")]
		[SerializeField] private MusicData[] _musics;

		[Header("Sound")]
		[SerializeField] private SoundData[] _sounds;

		[Header("Inn")]
		[SerializeField] private InnUpgradesData _innUpgrades = null;
		[SerializeField] private InnStartData _innStats = null;

		[Header("Mine")]
		[SerializeField] private MineUpgradesData _mineUpgrades = null;
		[SerializeField] private MineStartData _mineStats = null;

		[Header("Forge")]
		[SerializeField] private ForgeUpgradesData _forgeUpgrades = null;
		[SerializeField] private ForgeStartData _forgeStats = null;

		[Header("TradingPost")]
		[SerializeField] private TradingPostUpgradesData _tPUpgrades = null;
		[SerializeField] private TradingPostStartData _tPStats = null;

		[Header("Inventory")]
		[SerializeField] private ResourceListData _resourceList = null;
		[SerializeField] private WeaponListData _weaponList = null;

		[Header("Fortress")]
		[SerializeField] private FortressData[] _fortress = null;

		[Header("FTUE")]
		[SerializeField] private DialboxData[] _ftueDialboxList = null;

		[Header("Sprites")]
		[SerializeField] private Sprite _beerIcon = null;
		[SerializeField] private Sprite _mithrilIcon = null;
		[SerializeField] private Sprite _goldIcon = null;
		#endregion Fields

		#region Properties
		public float PolteringValue { get { return _polteringValue; } }

		public DailyReward[] DailyRewards { get { return _dailyRewards; } }

		public InnUpgradesData InnUpgrades { get { return _innUpgrades; } }
		public InnStartData InnStats { get { return _innStats; } }

		public MineUpgradesData MineUpgrades { get { return _mineUpgrades; } }
        public MineStartData MineStats { get { return _mineStats; } }

		public ForgeUpgradesData ForgeUpgrades { get { return _forgeUpgrades; } }
		public ForgeStartData ForgeStats { get { return _forgeStats; } }

		public TradingPostUpgradesData TradingPostUpgrades { get { return _tPUpgrades; } }
		public TradingPostStartData TradingPostStats { get { return _tPStats; } }

		public ResourceListData ResourceList { get { return _resourceList; } }
		public WeaponListData WeaponList { get { return _weaponList; } }

		public FortressData[] Fortress { get { return _fortress; } }

		public DialboxData[] Dialboxs { get { return _ftueDialboxList; } }

		public Sprite BeerIcon { get { return _beerIcon; } }
		public Sprite MithrilIcon { get { return _mithrilIcon; } }
		public Sprite GoldIcon { get { return _goldIcon; } }
		#endregion Properties

		#region Methods
		public WeaponData ExtractWeapon(string key)
		{
			for (int i = 0; i < _weaponList.Weapons.Length; i++)
			{
				if (_weaponList.Weapons[i].Name == key)
					return _weaponList.Weapons[i];
			}
			return null;
		}

		public ResourceData ExtractResource(string key)
		{
			for (int i = 0; i < _resourceList.Resources.Length; i++)
			{
				if (_resourceList.Resources[i].Name == key)
					return _resourceList.Resources[i];
			}
			return null;
		}

		public string ExtractFTUEDialboxByStep(int step)
		{
			for (int i = 0; i < _ftueDialboxList.Length; i++)
			{
				if (_ftueDialboxList[i].Step == step)
					return _ftueDialboxList[i].Text;
			}
			return "No Step found. Text does not exist.";
		}

		public AudioClip ExtractSound(string soundTag = "Random")
		{
			if (soundTag == "Random")
			{
				int i = Random.Range(0, _sounds.Length);
				return _sounds[i].Sound;
			}
			else
			{
				for (int i = 0; i < _sounds.Length; i++)
				{
					if (_sounds[i].SoundTag == soundTag)
						return _sounds[i].Sound;
				}
			}
			Debug.LogError("sound tag " + soundTag + " not found.");
			return null;
		}

		public AudioClip ExtractRandomSound(string soundTag = "Random")
		{
			if (soundTag == "Random")
			{
				int i = Random.Range(0, _sounds.Length);
				return _sounds[i].RandomSound;
			}
			else
			{
				for (int i = 0; i < _sounds.Length; i++)
				{
					if (_sounds[i].SoundTag == soundTag)
						return _sounds[i].RandomSound;
				}
			}
			Debug.LogError("sound tag " + soundTag + " not found.");
			return null;
		}

		public AudioClip ExtractMusic(string musicTag = "Random")
		{
			if (musicTag == "Random")
			{
				int i = Random.Range(0, _musics.Length);
				return _musics[i].Music;
			}
			else
			{
				for (int i = 0; i < _musics.Length; i++)
				{
					if (_musics[i].MusicTag == musicTag)
						return _musics[i].Music;
				}
			}
			Debug.LogError("music tag " + musicTag + " not found.");
			return null;
		}
		#endregion Methods
	}
}