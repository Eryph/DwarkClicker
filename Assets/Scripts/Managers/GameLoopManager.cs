namespace Engine.Manager
{
	using System;
	using Engine.Utils;

	public class GameLoopManager : Singleton<GameLoopManager>
	{
		#region Events
		private Action _gameLoop = null;

		public event Action GameLoop
		{
			add
			{
				_gameLoop -= value;
				_gameLoop += value;
			}
			remove
			{
				_gameLoop -= value;
			}
		}

		private Action _fixedGameLoop;

		public event Action FixedGameLoop
		{
			add
			{
				_fixedGameLoop -= value;
				_fixedGameLoop += value;
			}
			remove
			{
				_fixedGameLoop -= value;
			}
		}

		private Action _lateGameLoop;

		public event Action LateGameLoop
		{
			add
			{
				_lateGameLoop -= value;
				_lateGameLoop += value;
			}
			remove
			{
				_lateGameLoop -= value;
			}
		}
		#endregion Events

		#region Methods
		#region Loop
		protected override void Update()
		{
			base.Update();
			if (_gameLoop != null)
				_gameLoop();
		}

		private void FixedUpdate()
		{
			if (_fixedGameLoop != null)
				_fixedGameLoop();
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (_lateGameLoop != null)
				_lateGameLoop();
		}
		#endregion Loop
		#endregion Methods
	}
}