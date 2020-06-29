namespace DwarfClicker.UI.Fortress
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using Engine.Manager;
    using UnityEngine.UI;

    public class FortressNamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fortressNameText = null;
        [SerializeField] private Sprite _ironBackground = null;
        [SerializeField] private Sprite _steelBackground = null;
        [SerializeField] private Sprite _goldBackground = null;
        [SerializeField] private Image _background = null;

        // Start is called before the first frame update
        void Start()
        {
            JSonManager.Instance.PlayerProfile.OnFortressChange += UpdateName;
            _fortressNameText.text = JSonManager.Instance.PlayerProfile.CurrentFortress.Name;
            UpdateName();
        }

        private void UpdateName()
        {
            _fortressNameText.text = JSonManager.Instance.PlayerProfile.CurrentFortress.Name;
            switch (JSonManager.Instance.PlayerProfile.CurrentFortressIndex)
            {
                case 0:
                    _background.sprite = _ironBackground;
                    break;
                case 1:
                    _background.sprite = _steelBackground;
                    break;
                case 2:
                    _background.sprite = _goldBackground;
                    break;
            }
        }
    }
}