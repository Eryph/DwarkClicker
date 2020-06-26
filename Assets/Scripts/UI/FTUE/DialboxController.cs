namespace Engine.UI.FTUE
{
	using Engine.Manager;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
	using DwarfClicker.Misc;

	public class DialboxController : MonoBehaviour
	{
		[SerializeField] private ScrollRect _scroll = null;
		[SerializeField] private int _posToSwitch = 1500;

		[SerializeField] private Fader _firstFullScreenDialbox = null;

		[SerializeField] private GameObject _topDialbox;
		[SerializeField] private GameObject _bottomDialbox;

		[SerializeField] private TextMeshProUGUI _topText = null;
		[SerializeField] private TextMeshProUGUI _bottomText = null;

		[SerializeField] private GameObject _fullScreenQuitButton = null;
        [SerializeField] private GameObject _downArrow = null;
        [SerializeField] private GameObject _upArrow = null;

        [Header("HighLights")]
        private int _highlightsIndex = 0;
        [SerializeField] private GameObject _fullHighlight = null;
        [SerializeField] private GameObject[] _highlights = null;

        public int HighLightIndex
        {
            get
            {
                return _highlightsIndex;
            }
        }

        private void Start()
		{
            if (!FTUEManager.Instance.IsDebug)
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
                    _fullHighlight.SetActive(true);
                }
                if (JSonManager.Instance.PlayerProfile.FTUEStep <= 3)
                {
                    JSonManager.Instance.PlayerProfile.CurrentFortress.OnBeerChange += CheckBeerStorage;
                }
                FTUEManager.Instance.TriggerStep();
            }
		}

		private void OnDisable()
		{
			_topDialbox.SetActive(false);
			_bottomDialbox.SetActive(false);
		}

        public void QuitButtonHighlight()
        {
            if (HighLightIndex == 3 ||
            HighLightIndex == 6 ||
            HighLightIndex == 9 ||
            HighLightIndex == 12 ||
            !FTUEManager.Instance.IsActivated)
            {
                TriggerNextHiglight();
            }
        }

        public void TriggerNextHiglight()
        {
            if (_highlightsIndex > 0)
                _highlights[_highlightsIndex - 1].SetActive(false);
            if (_highlightsIndex < _highlights.Length)
            {
                _highlights[_highlightsIndex].SetActive(true);
                _highlightsIndex++;
            }
        }

		public void TriggerDialbox(string text, int currentStep)
		{
			if (FTUEManager.Instance.IsActivated)
			{
                if (FTUEManager.Instance.CurrentStep >= 1)
                    TriggerNextHiglight();
                gameObject.SetActive(true);
                SetTopBottomDialbox();
				_topText.text = text;
				_bottomText.text = text;
                if (FTUEManager.Instance.CurrentStep == 11)
                {
                    _fullHighlight.SetActive(true);
                }

                switch (HighLightIndex)
                {
                    case 0:
                        EnableArrow(false);
                        break;
                    case 1:
                        EnableArrow(true);
                        break;
                    case 2:
                        EnableArrow(false);
                        break;
                    case 3:
                        EnableArrow(false);
                        break;
                    case 4:
                        EnableArrow(true);
                        break;
                    case 5:
                        EnableArrow(false);
                        break;
                    case 6:
                        EnableArrow(false);
                        break;
                    case 7:
                        EnableArrow(true);
                        break;
                    case 8:
                        EnableArrow(false);
                        break;
                    case 9:
                        EnableArrow(false);
                        break;
                    case 10:
                        EnableUpArrow(true);
                        EnableArrow(false);
                        break;
                    case 11:
                        EnableUpArrow(false);
                        EnableArrow(false);
                        break;
                    case 12:
                        EnableUpArrow(false);
                        EnableArrow(false);
                        break;
                    case 13:
                        EnableUpArrow(true);
                        EnableArrow(false);
                        break;
                }

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
                TriggerNextHiglight();
            }
        }

		public void SetTopBottomDialbox()
		{
            if (FTUEManager.Instance.IsActivated)
            {
                if (_scroll.verticalNormalizedPosition >= 0.5)
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
		}

		//DEBUG

		public void SetStepFinished()
		{
            FTUEManager.Instance.StepFinished();
		}

        public void DialboxStepFinished()
        {
            if (FTUEManager.Instance.CurrentStep == 0 || FTUEManager.Instance.CurrentStep == 11)
            {
                SetStepFinished();
                _fullHighlight.SetActive(false);
            }
        }

		public void FinishFirstStep()
		{
			_firstFullScreenDialbox.FadeStart();
		}

        public void EnableArrow(bool v)
        {
            _downArrow.SetActive(v);
        }

        public void EnableUpArrow(bool v)
        {
            _upArrow.SetActive(v);
        }
    }
}