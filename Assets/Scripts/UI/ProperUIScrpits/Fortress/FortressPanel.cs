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

    public class FortressPanel : MonoBehaviour
	{
        #region Fields
        [SerializeField] private Button _buyButton = null;
        [SerializeField] private Image _buttonImage = null;
        [SerializeField] private TextMeshProUGUI _fortressName = null;
        [SerializeField] private TextMeshProUGUI _buttonText = null;
        [SerializeField] private Image _fortressImage = null;
        [SerializeField] private Image _coinImage = null;
        [SerializeField] private Sprite _buttonOnSprite = null;
        [SerializeField] private Sprite _buttonOffSprite = null;
        [SerializeField] private Button _redeemButton = null;
        [SerializeField] private PopUpWindowController _popUp = null;

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
            _index = _profile.CurrentFortressIndex;
			DisplayPanel();
		}

		public void DisplayPanel()
		{
            //_fortressImage.sprite = _profile.Fortress[_index].FortressImage;
            FortressProfile fortress = _profile.Fortress[_index];
            _fortressName.text = fortress.Name;

            if (fortress._isBought)
            {
                _coinImage.gameObject.SetActive(false);
                _buttonText.text = "Go to";
                if (_index == _profile.CurrentFortressIndex)
                {
                    _buyButton.interactable = false;
                }
                else
                {
                    _buyButton.interactable = true;
                }
            }
            else
            {
                _coinImage.gameObject.SetActive(true);
                int price = DatabaseManager.Instance.Fortress[_index].Price;
                _buttonText.text = "Price : " + UIHelper.FormatIntegerString(price);
                if (price > _profile.Gold)
                {
                    _buttonImage.sprite = _buttonOffSprite;
                    _buyButton.interactable = false;
                }
                else
                {
                    _buttonImage.sprite = _buttonOnSprite;
                    _buyButton.interactable = true;
                }
            }
        }

        public void BuyOrSwitchFortress()
        {
            if (_profile.Fortress[_index]._isBought)
                SwitchFortress();
            else
                BuyFortress();
        }

        public void NextFortress()
        {
            _index++;
            if (_index >= _profile.Fortress.Count)
            {
                _index = 0;
            }
            SoundManager.Instance.PlaySound("STANDARD_CLICK");
            DisplayPanel();
        }

        public void PreviousFortress()
        {
            _index--;
            if (_index < 0)
            {
                _index = _profile.Fortress.Count;
            }
            SoundManager.Instance.PlaySound("STANDARD_CLICK");
            DisplayPanel();
        }

		public void SwitchFortress()
		{
			_profile.CurrentFortressIndex = _index;
			DisplayPanel();
            SoundManager.Instance.PlaySound("STANDARD_CLICK");
        }

		public void BuyFortress()
		{
			GameManager.Instance.BuyFortress(_index);
            AchievementManager.Instance.UpdateAchievement("FORTRESS", 1);
            DisplayPanel();
            SoundManager.Instance.PlaySound("STANDARD_CLICK");
            _popUp.Display(2, "Transaction Complete !\nFortress unlocked !");
        }

		public void RedeemProduction()
		{
			GameManager.Instance.LoadProgression(true);
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}
	}
}