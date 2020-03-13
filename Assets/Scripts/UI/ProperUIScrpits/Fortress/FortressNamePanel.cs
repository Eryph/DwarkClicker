namespace DwarfClicker.UI.Fortress
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using Engine.Manager;

    public class FortressNamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fortressNameText = null;
        
        // Start is called before the first frame update
        void Start()
        {
            JSonManager.Instance.PlayerProfile.OnFortressChange += UpdateName;
            _fortressNameText.text = JSonManager.Instance.PlayerProfile.CurrentFortress.Name;
        }

        private void UpdateName()
        {
            _fortressNameText.text = JSonManager.Instance.PlayerProfile.CurrentFortress.Name;
        }
    }
}