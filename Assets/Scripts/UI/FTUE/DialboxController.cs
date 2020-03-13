namespace Engine.UI.FTUE
{
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using DwarfClicker.Misc;

	public class DialboxController : MonoBehaviour
	{
		[SerializeField] private Transform _content = null;
		[SerializeField] private int _posToSwitch = 1500;

		[SerializeField] private Fader _firstFullScreenDialbox = null;

		[SerializeField] private GameObject _topDialbox;
		[SerializeField] private GameObject _bottomDialbox;

		[SerializeField] private TextMeshProUGUI _topText = null;
		[SerializeField] private TextMeshProUGUI _bottomText = null;

		[SerializeField] private GameObject _fullScreenQuitButton = null;

        [Header("HighLights")]
        [SerializeField] private GameObject _innHighlight = null;
        [SerializeField] private GameObject _mineHighlight = null;
        [SerializeField] private GameObject _forgeHighlight = null;
        [SerializeField] private GameObject _tradingPostHighlight = null;
        [SerializeField] private GameObject _closeButtonHighlight = null;
        [SerializeField] private GameObject _craftSelectorHighlight = null;

        private void Start()
		{
			if (FTUEManager.Instance.Dialbox == null)
			{
				FTUEManager.Instance.SetDialbox(this);
			}
			else
			{
				Debug.LogError("Multiple DialboxController is not supported.");
			}
			if (JSonManager.Instance.PlayerProfile.FTUEStep <= 0)
			{
				_firstFullScreenDialbox.gameObject.SetActive(true);
			}
            if (JSonManager.Instance.PlayerProfile.FTUEStep <= 3)
            {
                JSonManager.Instance.PlayerProfile.CurrentFortress.OnBeerChange += CheckBeerStorage;
            }
			FTUEManager.Instance.TriggerStep();
		}

		private void OnDisable()
		{
			_topDialbox.SetActive(false);
			_bottomDialbox.SetActive(false);
		}

        private void SetHiglight(int currentStep)
        {
            Debug.Log("CURRENT STEP : " + currentStep);
            switch (currentStep)
            {
                case 2:
                    _innHighlight.SetActive(true);
                    break;
                case 4:
                    _closeButtonHighlight.SetActive(true);
                    break;
                case 5:
                    _mineHighlight.SetActive(true);
                    break;
                case 8:
                    _forgeHighlight.SetActive(true);
                    break;
                case 9:
                    _craftSelectorHighlight.SetActive(true);
                    break;
                case 14:
                    _tradingPostHighlight.SetActive(true);
                    break;
                default:
                    DisableHighlight();
                    break;
            }
        }

        private void DisableHighlight()
        {
            _innHighlight.SetActive(false);
            _mineHighlight.SetActive(false);
            _forgeHighlight.SetActive(false);
            _tradingPostHighlight.SetActive(false);
            _closeButtonHighlight.SetActive(false);
            _closeButtonHighlight.SetActive(false);
            _craftSelectorHighlight.SetActive(false);
        }

		public void TriggerDialbox(string text, int currentStep)
		{
			if (FTUEManager.Instance.IsActivated)
			{
				gameObject.SetActive(true);
				_bottomDialbox.SetActive(true);
				_topDialbox.SetActive(false);
				_topText.text = text;
				_bottomText.text = text;
                SetHiglight(currentStep);
			}
			else
				DisableDialbox();
		}

		public void DisableDialbox()
		{
			gameObject.SetActive(false);
		}

        public void CheckBeerStorage()
        {
            if (JSonManager.Instance.PlayerProfile.CurrentFortress.Beer >= JSonManager.Instance.PlayerProfile.CurrentFortress.BeerStorage)
            {
                JSonManager.Instance.PlayerProfile.CurrentFortress.OnBeerChange -= CheckBeerStorage;
                SetStepFinished();
            }
        }

		public void SetTopBottomDialbox()
		{
			if (FTUEManager.Instance.IsActivated)
			if (_content.position.y < _posToSwitch)
			{
				_bottomDialbox.SetActive(true);
				_topDialbox.SetActive(false);
			}
			else
			{
				_bottomDialbox.SetActive(false);
				_topDialbox.SetActive(true);
			}
		}

		//DEBUG

		public void SetStepFinished()
		{
            DisableHighlight();
            FTUEManager.Instance.StepFinished();
		}

        public void DialboxStepFinished()
        {
            if (_innHighlight.activeSelf == false &&
                _mineHighlight.activeSelf == false &&
                _forgeHighlight.activeSelf == false &&
                _tradingPostHighlight.activeSelf == false &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 3 &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 4 &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 7 &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 13 &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 14 &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 15 &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 18 &&
                JSonManager.Instance.PlayerProfile.FTUEStep != 19)

                SetStepFinished();
        }

		public void FinishFirstStep()
		{
			_firstFullScreenDialbox.FadeStart();
		}
	}
}