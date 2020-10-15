[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.iOS.Services.NotificationService))]
namespace DevsDNA.Application.iOS.Services
{
	using DevsDNA.Application.Common;
	using DevsDNA.Application.Services;
	using Foundation;
	using System;
	using System.Threading.Tasks;
	using UIKit;
	using UserNotifications;
	using WindowsAzure.Messaging;

	public class NotificationService : INotificationService
	{
		private readonly ILogService logService;
		private readonly SBNotificationHub notificationHub;
		private NSData notificationToken;

		public NotificationService()
		{
			logService = CustomDependencyService.Instance.Get<ILogService>();
			notificationHub = new SBNotificationHub(SettingsKeyValues.NotificationConnectionString, SettingsKeyValues.NotificationHubName);
		}


		public void EnableNotifications()
		{
			// register for remote notifications based on system version
			if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
			{
				UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Sound,
					(granted, error) =>
					{
						if (granted)
							Xamarin.Forms.Device.BeginInvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
					});
			}
			else
			{
				throw new Exception("Application is not supported for lowers versions.");
			}
		}

		public void DisableNotifications()
		{
			try
			{
				if (notificationToken != null)
				{
					notificationHub.UnregisterAll(notificationToken, out NSError error);

					if (error != null)
					{
						logService.Log(error.DebugDescription);
					}
				}
			}
			catch (Exception ex)
			{
				logService.LogError(ex);
			}
		}

		public async Task DidRegisterRemoteNotificationsAsync(NSData deviceToken)
		{
			notificationToken = deviceToken;
			try
			{
				if (notificationToken != null)
				{
					await notificationHub.RegisterNativeAsync(notificationToken, null);
				}
			}
			catch (Exception ex)
			{
				logService.LogError(ex);
			}
		}
	}
}