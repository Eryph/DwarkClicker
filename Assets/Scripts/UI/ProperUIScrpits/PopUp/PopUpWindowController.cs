namespace DwarfClicker.UI.PopUp
{
    using Engine.Manager;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PopUpWindowController : MonoBehaviour
    {
        [SerializeField] private Animator[] _anims = null;
        [SerializeField] private Image _backgroundImage = null;
        [SerializeField] private TextMeshProUGUI[] _popUpText = null;
        [SerializeField] private Sprite[] _backgroundSprites = null;
        [SerializeField] private float _lerpTime = 0.1f;

        [SerializeField] private float[] _fireWorksDelays = null;
        [SerializeField] private Animator[] _fireWorks = null;


        private GameObject _currentAnim = null;
        private Vector3 _startScale = Vector3.zero;
        private float _currentLerpTime = 0f;

        private float _currentAnimTime = 0f;
        private int _delaysIndex = 0;

        private void OnEnable()
        {
            _startScale = _backgroundImage.transform.localScale;
        }

        private void Start()
        {
           
        }

        public void Display(int popUpRank, string popUpText, bool isFortress = false)
        {
            SoundManager.Instance.PlaySound("REWARD_SOUND_R" + (popUpRank + 1).ToString());
            gameObject.SetActive(true);
            popUpRank = Mathf.Clamp(popUpRank, 0, _backgroundSprites.Length - 1);
            if (isFortress)
            {
                for (int i = 0; i < _anims.Length; i++)
                {
                    _anims[i].gameObject.SetActive(false);
                }
                _anims[3].gameObject.SetActive(true);
                _currentAnim = _anims[3].gameObject;
                _popUpText[3].text = popUpText;
            }
            else
            {
                for (int i = 0; i < _anims.Length; i++)
                {
                    if (i == popUpRank)
                    {
                        _anims[i].gameObject.SetActive(true);
                        _currentAnim = _anims[i].gameObject;
                        _popUpText[i].text = popUpText;
                    }
                    else
                    {
                        _anims[i].gameObject.SetActive(false);
                    }
                }
            }
            
            //_anim.SetTrigger("l" + popUpRank.ToString());

            
            GameLoopManager.Instance.GameLoop += AnimateFireWorks;
            GameLoopManager.Instance.GameLoop += AnimateBackground;
            
        }

        private void AnimateBackground()
        {
            if (_currentLerpTime <= _lerpTime)
            {
                float t = _currentLerpTime / _lerpTime;
                t = t * t * t * t * t * t;
                _currentAnim.transform.localScale = Vector3.Lerp(Vector3.zero, _startScale, t);
                _currentLerpTime += Time.deltaTime;
            }
            else
            {
                GameLoopManager.Instance.GameLoop -= AnimateBackground;
                _currentAnim.transform.localScale = _startScale;
                _currentLerpTime = 0;
            }
        }

        private void AnimateFireWorks()
        {
            _currentAnimTime += Time.deltaTime;
            if (_currentAnimTime >= _fireWorksDelays[_delaysIndex])
            {
                _fireWorks[_delaysIndex].SetTrigger("Trigger");
                _delaysIndex++;
            }
            if (_delaysIndex >= _fireWorksDelays.Length)
            {
                GameLoopManager.Instance.GameLoop -= AnimateFireWorks;
                _currentAnimTime = 0;
                _delaysIndex = 0;
            }
        }

        public void QuitPanel()
        {
            gameObject.SetActive(false);
        }


    }
}