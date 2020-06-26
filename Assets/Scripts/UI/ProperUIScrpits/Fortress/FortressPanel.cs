namespace Preprod
{
	using DwarkClicker.Helper;
	using Engine.Manager;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
    using TMPro;
    using DwarfClicker.UI.PopUp;
    using DwarfClicker.Core.Data;

    public class FortressPanel : MonoBehaviour
    {
        #region Fields
        [SerializeField] private PopUpWindowController _popUp = null;
        [SerializeField] private TextMeshProUGUI _fortressName = null;
        [SerializeField] private TextMeshProUGUI _nextFortressPrice = null;
        [SerializeField] private Image _currentFortressIcon = null;
        [SerializeField] private Image _currentFortressResource = null;
        [SerializeField] private Image[] _currentFortressItems = null;
        [SerializeField] private Image[] _nextFortressIcons = null;
        [SerializeField] private Image[] _nextFortressItems = null;
        [SerializeField] private Image _nextFortressResource = null;
        [SerializeField] private Button _buyButton = null;
        [SerializeField] private GameObject _resourceText = null;
        [SerializeField] private GameObject _itemText = null;
        [SerializeField] private GameObject _forNowText = null;


        private int _index = 0;
		private PlayerProfile _profile = null;
		#endregion Fields

		private void Start()
		{
			if (_profile == null)
			{
				_profile = JSonManager.Instance.PlayerProfile;
            }
		}

		private void OnEnable()
		{
			if (_profile == null)
			{
				_profile = JSonManager.Instance.PlayerProfile;
			}
			DisplayPanel();
		}

		public void DisplayPanel()
		{
            PlayerProfile profile = JSonManager.Instance.PlayerProfile;
            FortressData currentFortress = DatabaseManager.Instance.Fortress[profile._currentFortressIndex];
            
            if (profile._currentFortressIndex + 1 < DatabaseManager.Instance.Fortress.Length)
            {
                FortressData nextFortress = DatabaseManager.Instance.Fortress[profile._currentFortressIndex + 1];
                _fortressName.text = nextFortress.Name;
                _nextFortressPrice.text = UIHelper.FormatIntegerString(nextFortress.Price);
                _currentFortressIcon.sprite = currentFortress.FortressIcon;
                _currentFortressResource.sprite = currentFortress.ResourceToProduce.ResourceSprite;
                for (int i = 0; i < _currentFortressItems.Length; i++)
                {
                    _currentFortressItems[i].sprite = currentFortress.WeaponsToProduce[i].WeaponSprite;
                }
                for (int i = 0; i < _nextFortressItems.Length; i++)
                {
                    _nextFortressItems[i].sprite = nextFortress.WeaponsToProduce[i].WeaponSprite;
                }

                for (int i = 0; i < _nextFortressIcons.Length; i++)
                {
                    _nextFortressIcons[i].sprite = nextFortress.FortressIcon;
                }

                _buyButton.interactable = profile.Gold >= nextFortress.Price;
            }
            else
            {
                _currentFortressIcon.sprite = currentFortress.FortressIcon;
                _currentFortressResource.sprite = currentFortress.ResourceToProduce.ResourceSprite;
                for (int i = 0; i < _currentFortressItems.Length; i++)
                {
                    _currentFortressItems[i].sprite = currentFortress.WeaponsToProduce[i].WeaponSprite;
                }

                _fortressName.text = "Not available";
                _buyButton.gameObject.SetActive(false);
                _nextFortressResource.gameObject.SetActive(false);
                _resourceText.gameObject.SetActive(false);
                _itemText.gameObject.SetActive(false);

                for (int i = 0; i < _nextFortressIcons.Length; i++)
                {
                    _nextFortressIcons[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < _nextFortressItems.Length; i++)
                {
                    _nextFortressItems[i].gameObject.SetActive(false);
                }
                _forNowText.SetActive(true);
            }

        }

        

		public void SwitchFortress()
		{
			_profile.CurrentFortressIndex = JSonManager.Instance.PlayerProfile._currentFortressIndex + 1;
            SoundManager.Instance.PlaySound("STANDARD_CLICK");
        }

		public void BuyFortress()
		{
			GameManager.Instance.BuyFortress(JSonManager.Instance.PlayerProfile._currentFortressIndex + 1);
            AchievementManager.Instance.UpdateAchievement("FORTRESS", 1);
            SoundManager.Instance.PlaySound("STANDARD_CLICK");
            _popUp.Display(2, "Transaction Complete !\nFortress unlocked !", true);
            SwitchFortress();
            QuitPanel();
        }

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}