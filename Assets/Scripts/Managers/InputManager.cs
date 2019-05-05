namespace Engine.Manager
{
	using UnityEngine;
	using Engine.Utils;

	public class InputManager : Singleton<InputManager>
	{
		#region Fields
		#endregion Fields

		#region Properties
		#endregion Properties

		#region Methods
		protected override void Start()
		{
			base.Start();
			GameLoopManager.Instance.GameLoop += GameLoop;
			GameLoopManager.Instance.FixedGameLoop += FixedGameLoop;
		}

		protected override void OnDestroy()
		{
			if (GameLoopManager.Instance)
			{
				GameLoopManager.Instance.GameLoop -= GameLoop;
				GameLoopManager.Instance.FixedGameLoop -= FixedGameLoop;
			}
			base.OnDestroy();
		}

		private void FixedGameLoop()
		{
			ResetInputs();
		}

		private void GameLoop()
		{
			GetGamePadInputs();
			GetMouseInputs();
			GetKeyboardInputs();
		}

		private void GetMouseInputs()
		{

		}

		private void GetKeyboardInputs()
		{

		}

		private void GetGamePadInputs()
		{

		}

		private void ResetInputs()
		{

		}
		#endregion Methods
	}
}