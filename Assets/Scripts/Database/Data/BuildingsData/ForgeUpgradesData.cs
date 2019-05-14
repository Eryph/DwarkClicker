﻿namespace DwarfClicker.Core.Data
{
	using Engine.Utils;
	using UnityEngine;

	[CreateAssetMenu(fileName = "ForgeUpgrades", menuName = "Data/ForgeUpgrades")]
	public class ForgeUpgradesData : ScriptableObject
	{
		[SerializeField] private IntUpgrade _workerAmount;
		[SerializeField] private IntUpgrade _wByWorker;
		[SerializeField] private FloatUpgrade _cycleDuration;
		[SerializeField] private IntUpgrade _instantSellingChance;
		[SerializeField] private FloatUpgrade _instantSellingGoldBonus;

		public IntUpgrade WorkerAmount { get { return _workerAmount; } }
		public IntUpgrade WByWorker { get { return _wByWorker; } }
		public FloatUpgrade CycleDuration { get { return _cycleDuration; } }
		public IntUpgrade InstantSellingChance { get { return _instantSellingChance; } }
		public FloatUpgrade InstantSellingGoldBonus { get { return _instantSellingGoldBonus; } }
	}
}