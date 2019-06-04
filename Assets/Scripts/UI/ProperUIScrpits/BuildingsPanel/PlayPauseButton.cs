namespace DwarfClicker.UI.BuildingCommon
{
	using DwarfClicker.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class PlayPauseButton : MonoBehaviour
	{
		[SerializeField] private GameObject _playIcon = null;
		[SerializeField] private GameObject _pauseIcon = null;
		[SerializeField] private ABuildingBase _buildingController = null;

		private void OnEnable()
		{
			_buildingController.OnPause += SwitchIcon;
			SwitchIcon(_buildingController.IsPaused);
		}

		public void SwitchIcon(bool isPaused)
		{
			_playIcon.SetActive(isPaused);
			_pauseIcon.SetActive(!isPaused);
		}
	}
}