using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	#region Fields
	[SerializeField] private GameObject _innUpgradeMenu = null;
	[SerializeField] private GameObject _mineUpgradeMenu = null;
	[SerializeField] private GameObject _forgeUpgradeMenu = null;
	[SerializeField] private GameObject _tradingPostUpgradeMenu = null;
	[SerializeField] private GameObject _quitMenuButton = null;
	#endregion Fields

	#region Methods
	public void DisplayInnMenu()
	{
		_innUpgradeMenu.SetActive(true);
		_quitMenuButton.SetActive(true);
	}

	public void DisplayMineMenu()
	{
		_mineUpgradeMenu.SetActive(true);
		_quitMenuButton.SetActive(true);
	}

	public void DisplayForgeMenu()
	{
		_forgeUpgradeMenu.SetActive(true);
		_quitMenuButton.SetActive(true);
	}

	public void DisplayTradingPostMenu()
	{
		_tradingPostUpgradeMenu.SetActive(true);
		_quitMenuButton.SetActive(true);
	}

	public void DisableMenu()
	{
		_innUpgradeMenu.SetActive(false);
		_mineUpgradeMenu.SetActive(false);
		_forgeUpgradeMenu.SetActive(false);
		_tradingPostUpgradeMenu.SetActive(false);
		_quitMenuButton.SetActive(false);
	}
	#endregion Methods
}
