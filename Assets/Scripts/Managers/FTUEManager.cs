namespace Engine.Manager
{
	using Engine.UI.FTUE;
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class FTUEManager : Singleton<FTUEManager>
	{
		#region Fields
		private DialboxController _dialbox = null;
		private int _currentStep = 0;
        [SerializeField] private bool _isDebug = false;
        [SerializeField] private int _stepAmount = 15;
        #endregion Fields

        #region Properties
        public bool IsDebug { get { return _isDebug; } }
        public int StepAmount { get { return _stepAmount; } }
        public int CurrentStep { get { return _currentStep; } }
        public DialboxController Dialbox { get { return _dialbox; } }
		public bool IsActivated { get { return _currentStep < _stepAmount; } }
        #endregion Properties

        protected override void Start()
        {
            base.Start();

            
            _stepAmount = DatabaseManager.Instance.Dialboxs.Length;
            if (_isDebug)
                _currentStep = _stepAmount;
            else
                JSonManager.Instance.OnProfileLoaded += SetCurrentStep;
        }

		private void SetCurrentStep()
		{
			JSonManager.Instance.PlayerProfile.OnFTUEStepChange += TriggerStep;
			_currentStep = JSonManager.Instance.PlayerProfile.FTUEStep;
		}

		public void TriggerStep()
		{
			_currentStep = JSonManager.Instance.PlayerProfile.FTUEStep;
			if (IsActivated)
			{
				string text = DatabaseManager.Instance.ExtractFTUEDialboxByStep(_currentStep);
				_dialbox.TriggerDialbox(text, _currentStep);
			}
		}

        public void SetNewHighlight()
        {
            _dialbox.TriggerNextHiglight();
        }

		public void StepFinished()
		{
			JSonManager.Instance.PlayerProfile.FTUEStep++;
			_currentStep = JSonManager.Instance.PlayerProfile.FTUEStep;
			if (!IsActivated)
			{
				_dialbox.DisableDialbox();
			}
		}

		public void SetDialbox(DialboxController dialbox)
		{
			_dialbox = dialbox;
		}
	}
}