namespace Engine.Manager
{
	using Engine.Utils;
	using System.Collections;
	using System.Collections.Generic;
//	using Unity.Notifications.Android;
	using UnityEngine;

	public class DeviceManager : Singleton<DeviceManager>
	{


#region Fields
		[SerializeField] private float _minTimeToNotifThrow = 24;
#if ANDROID 
	private NotifProfile _notifProfile = null;
#endif
#endregion Fields

#region Properties
#endregion Properties

#region Methods
#if ANDROID
		protected override void Start()
		{
			JSonManager.Instance.OnProfileLoaded += Init;
		}

		private void Init()
		{
			return;
			base.Start();
	/*		_notifProfile = JSonManager.Instance.NotifProfile;
			AndroidNotificationChannel channel = AndroidNotificationCenter.GetNotificationChannel("Notif_ChanID");

			Debug.Log(channel.Id);
			if (channel.Id != "Null")
			{
				AndroidNotificationChannel c = new AndroidNotificationChannel("Notif_ChanID", "AndroidNotifChan", "Generic notifications", Importance.High);
				AndroidNotificationCenter.RegisterNotificationChannel(c);
			}

			// Cancel all local notifications
			for (int i = 0; i < _notifProfile.AndroidNotifIds.Count; i++)
			{
				if (AndroidNotificationCenter.CheckScheduledNotificationStatus(_notifProfile.AndroidNotifIds[i]) == NotificationStatus.Scheduled)
				{
					// Replace the currently scheduled notification with a new notification.
					AndroidNotificationCenter.CancelNotification(_notifProfile.AndroidNotifIds[i]);
					_notifProfile.AndroidNotifIds.Remove(i);
				}
			}
			// Cancel all local notifications
		}

		public void PushLocalNotification(string title, string text, float hourToAdd)
		{
			return;
			if (hourToAdd < _minTimeToNotifThrow)
			{
				return;
			}

			// Create notification
			AndroidNotification notification = new AndroidNotification();
			notification.Title = title;
			notification.Text = text;
			notification.FireTime = System.DateTime.Now.AddDays(hourToAdd);
			// Create notification

			// Push Notification
			int notifID = AndroidNotificationCenter.SendNotification(notification, "Notif_ChanID");
			_notifProfile.AndroidNotifIds.Add(notifID);

			if (AndroidNotificationCenter.CheckScheduledNotificationStatus(notifID) == NotificationStatus.Scheduled)
			{
				// Replace the currently scheduled notification with a new notification.
				Debug.Log(string.Format("notif {0} is scheduled.", notifID));
			}
			// Push Notification*/
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			JSonManager.Instance.OnProfileLoaded -= Init;
		}
#endif
#endregion Methods
	}
}