namespace DevsDNA.Application.Services
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	public interface ITrackService
	{
		void ConfigureAppCenter();
		void TrackError(Exception ex, [CallerMemberName] string methodCaller = null);
		void TrackError(Exception ex, Dictionary<string, string> properties, [CallerMemberName] string methodCaller = null);
		void TrackEvent(string message, [CallerMemberName] string methodCaller = null);
		void TrackEvent(string message, Dictionary<string, string> properties, [CallerMemberName] string methodCaller = null);
	}
}
