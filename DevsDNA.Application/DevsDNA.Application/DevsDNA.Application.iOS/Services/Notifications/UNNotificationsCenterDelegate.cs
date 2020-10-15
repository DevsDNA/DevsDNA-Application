namespace DevsDNA.Application.iOS.Services
{
	using System;
	using UserNotifications;

	public class UNNotificationsCenterDelegate : UNUserNotificationCenterDelegate
	{
		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			// Tell system to display the notification anyway or use
			// `None` to say we have handled the display locally.
			completionHandler(UNNotificationPresentationOptions.Alert);
		}
	}
}