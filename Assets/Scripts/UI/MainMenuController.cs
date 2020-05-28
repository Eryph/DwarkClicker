using DwarfClicker.UI.PopUp;
using Engine.Manager;
using Engine.UI.FTUE;
using System;
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

    [SerializeField] private DialboxController _dialboxController = null;
    #endregion Fields

    #region Methods
    public void DisplayInnMenu()
    {
       if (JSonManager.Instance.PlayerProfile.FTUEStep == 1)
       {
           _dialboxController.SetStepFinished();
           _innUpgradeMenu.SetActive(true);
           _quitMenuButton.SetActive(true);
       }  
    }

    public void DisplayMineMenu()
    {
        if (JSonManager.Instance.PlayerProfile.FTUEStep == 3)
        {
            _dialboxController.SetStepFinished();
        }
        _mineUpgradeMenu.SetActive(true);
        _quitMenuButton.SetActive(true);
    }

    public void DisplayForgeMenu()
    {
       
         if (JSonManager.Instance.PlayerProfile.FTUEStep == 5)
         {
                _dialboxController.SetStepFinished();
        }
         _forgeUpgradeMenu.SetActive(true);
         _quitMenuButton.SetActive(true);
    }

    public void DisplayTradingPostMenu()
    {

            if (JSonManager.Instance.PlayerProfile.FTUEStep == 7)
            {
            _dialboxController.SetStepFinished();
        }
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
            if (JSonManager.Instance.PlayerProfile.FTUEStep == 2)
            {
                _dialboxController.SetStepFinished();
            }
            if (JSonManager.Instance.PlayerProfile.FTUEStep == 4)
            {
                _dialboxController.SetStepFinished();
            }
            if (JSonManager.Instance.PlayerProfile.FTUEStep == 6)
            {
                _dialboxController.SetStepFinished();
            }
            if (JSonManager.Instance.PlayerProfile.FTUEStep == 8)
            {
                _dialboxController.SetStepFinished();
            }
    }
	#endregion Methods
}
