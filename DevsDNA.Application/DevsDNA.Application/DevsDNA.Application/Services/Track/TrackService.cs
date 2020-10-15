[assembly:Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Services.Track.TrackService))]
namespace DevsDNA.Application.Services.Track
{
	using DevsDNA.Application.Helpers;
	using Microsoft.AppCenter;
	using Microsoft.AppCenter.Analytics;
	using Microsoft.AppCenter.Crashes;
	using Microsoft.AppCenter.Distribute;
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Xamarin.Essentials;

	public class TrackService : ITrackService
	{
		private readonly string appCenterSecretIOS;
		private readonly string appCenterSecretDROID;


		public TrackService()
		{
			appCenterSecretIOS = Secrets.AppCenterIOS;
			appCenterSecretDROID = Secrets.AppCenterDROID;
		}

		public void ConfigureAppCenter()
		{
			AppCenter.Start($"ios={appCenterSecretIOS};android={appCenterSecretDROID};", typeof(Analytics), typeof(Crashes), typeof(Distribute));
			Distribute.CheckForUpdate();
		}

		public void TrackError(Exception ex, [CallerMemberName] string methodCaller = null)
		{
			SendError(ex, null, methodCaller);
		}

		public void TrackError(Exception ex, Dictionary<string, string> properties, [CallerMemberName] string methodCaller = null)
		{
			SendError(ex, properties, methodCaller);
		}

		public void TrackEvent(string message, [CallerMemberName] string methodCaller = null)
		{
			SendEvent(message, null, methodCaller);
		}

		public void TrackEvent(string message, Dictionary<string, string> properties, [CallerMemberName] string methodCaller = null)
		{
			SendEvent(message, properties, methodCaller);
		}



		private void SendError(Exception ex, Dictionary<string, string> properties, string methodCaller)
		{
			if (properties == null)
			{
				properties = new Dictionary<string, string>();
			}
			properties.Add("Method caller", methodCaller ?? "Without information.");
			properties = GetDeviceProperties(properties);

			Crashes.TrackError(ex, properties);
		}

		private void SendEvent(string message, Dictionary<string, string> properties, string methodCaller)
		{
			if (properties == null)
			{
				properties = new Dictionary<string, string>();
			}
			properties.Add("Method caller", methodCaller ?? "Without information.");
			properties = GetDeviceProperties(properties);
			Analytics.TrackEvent(message, properties);
		}

		private Dictionary<string, string> GetDeviceProperties(Dictionary<string, string> properties)
		{
#if DEBUG
			properties.Add("Debug", "True");
#else
			properties.Add("Debug", "False");
#endif

			properties.Add("Device Idiom", DeviceInfo.Idiom.ToString());
			properties.Add("Device Platform", DeviceInfo.Platform.ToString());
			properties.Add("Device Type", DeviceInfo.DeviceType.ToString());
			properties.Add("Device Manufacturer", DeviceInfo.Manufacturer);
			properties.Add("Device Model", DeviceInfo.Model);
			properties.Add("Device Name", DeviceInfo.Name);
			properties.Add("Device Version", DeviceInfo.Version.ToString());
			properties.Add("Device VersionString", DeviceInfo.VersionString);

			return properties;
		}
	}
}
