namespace DevsDNA.Application.iOS
{
	using DevsDNA.Application.iOS.Services;
	using DevsDNA.Application.Services;
	using DevsDNA.Application.Services.Notifications;
	using Foundation;
	using Lottie.Forms.iOS.Renderers;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using UIKit;
	using UserNotifications;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : FormsApplicationDelegate
	{
		private ILogService logService;
		private ITrackService trackService;
		private NotificationService notificationService;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Forms.Init();

			logService = CustomDependencyService.Instance.Get<ILogService>();
			trackService = CustomDependencyService.Instance.Get<ITrackService>();
			notificationService = (NotificationService)CustomDependencyService.Instance.Get<INotificationService>();

			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			AnimationViewRenderer.Init();
			Sharpnado.Shades.iOS.iOSShadowsRenderer.Initialize();
			FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
			UNUserNotificationCenter.Current.Delegate = new UNNotificationsCenterDelegate();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			await notificationService.DidRegisterRemoteNotificationsAsync(deviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			trackService.TrackError(new Exception("Failed to register for remote notifications"), new Dictionary<string, string>
			{
				{ "LocalizedDescription", error.LocalizedDescription },
				{ "LocalizedFailureReason", error.LocalizedFailureReason},
				{ "DebugDescription", error.DebugDescription }
			});
			logService?.Log("Failed To Register For RemoteNotifications");
		}

		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			PushNotificationsType notificationType = ReadNotificationType(userInfo);
			(Xamarin.Forms.Application.Current as App).NewNotificationReceivedAsync(notificationType).ConfigureAwait(false);
		}


		private PushNotificationsType ReadNotificationType(NSDictionary options)
		{
			//{"aps":{"alert" : {"title" : "DevsDNA", "body" : "Nuevo contenido en RRSS"}},"pushType":"2"}
			PushNotificationsType notificationsType = PushNotificationsType.None;

			if (options != null && options.ContainsKey(new NSString("pushType")))
			{
				string strPushType = (options[new NSString("pushType")]).ToString();
				if (!string.IsNullOrEmpty(strPushType))
				{
					notificationsType = (PushNotificationsType)int.Parse(strPushType);
				}
			}

			return notificationsType;
		}


		private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			logService.LogError(e.Exception);
			trackService.TrackError(e.Exception);
		}

		private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			logService.LogError((Exception)e.ExceptionObject);
			trackService.TrackError(e.ExceptionObject as Exception);
		}
	}
}
