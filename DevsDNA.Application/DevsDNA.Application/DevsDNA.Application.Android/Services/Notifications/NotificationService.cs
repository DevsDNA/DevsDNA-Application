[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Droid.Services.NotificationService))]
namespace DevsDNA.Application.Droid.Services
{
	using System;
	using System.Threading.Tasks;
	using DevsDNA.Application.Common;
	using DevsDNA.Application.Services;
	using WindowsAzure.Messaging;
	using Xamarin.Essentials;

	public class NotificationService : INotificationService
	{
		private readonly ILogService logService;

		public NotificationService()
		{
			logService = CustomDependencyService.Instance.Get<ILogService>();
		}

		public static string NotificationToken { get; set; }

		public void EnableNotifications()
		{
			try
			{
				string token = NotificationToken;

				if (string.IsNullOrWhiteSpace(token))
				{
					token = Preferences.Get(SettingsKeyValues.PreferenceKeyNotificationToken, string.Empty);
				}

				if (!string.IsNullOrEmpty(token))
				{
					NotificationHub hub = new NotificationHub(SettingsKeyValues.NotificationHubName, SettingsKeyValues.NotificationConnectionString, MainActivity.CurrentActivity);
					Task.Run(() => hub.Register(NotificationToken, null)).ConfigureAwait(false);
				}
			}
			catch (Exception ex)
			{
				logService.LogError(ex);
			}
		}

		public void DisableNotifications()
		{
			try
			{
				if (!string.IsNullOrEmpty(NotificationToken))
				{
					Task.Run(() =>
						new NotificationHub(SettingsKeyValues.NotificationHubName, SettingsKeyValues.NotificationConnectionString, MainActivity.CurrentActivity)
						   .UnregisterAll(NotificationToken));
				}
			}
			catch (Exception ex)
			{
				logService.LogError(ex);
			}
		}
	}
}