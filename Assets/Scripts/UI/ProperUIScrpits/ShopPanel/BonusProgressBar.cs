namespace DwarfClicker.UI.ShopPanel
{
    using Engine.Manager;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class BonusProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider = null;

        public void UpdateBar()
        {
            _slider.value = JSonManager.Instance.PlayerProfile._bonusTimeRemaining / DatabaseManager.Instance.ConsumableBonusData.BonusTimeMax;
        }
    }
}