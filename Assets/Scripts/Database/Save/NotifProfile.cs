namespace Engine.Utils
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class NotifProfile
	{
		#region Fields
		public List<int> _androidNotifsIDs = null;
		public List<int> _IOSNotifsIDs = null;
		#endregion Fields

		#region Properties
		public List<int> AndroidNotifIds { get { return _androidNotifsIDs; } }
		public List<int> IOSNotifIds { get { return _IOSNotifsIDs; } }
		#endregion Properties

		#region Methods
		public void Init()
		{
			_androidNotifsIDs = new List<int>();
			_IOSNotifsIDs = new List<int>();
		}
		#endregion Methods
	}
}