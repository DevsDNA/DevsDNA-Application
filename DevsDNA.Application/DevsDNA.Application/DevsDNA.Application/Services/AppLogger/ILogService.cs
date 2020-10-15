namespace DevsDNA.Application.Services
{
	using System;
	using System.Runtime.CompilerServices;

	public interface ILogService
	{
		void Log(string message, [CallerMemberName] string callerMemberName = null);
		void Log(string message, string tag, [CallerMemberName] string callerMemberName = null);
		void LogError(Exception ex);
		void LogError(Exception ex, [CallerMemberName] string callerMemberName = null);
		void LogError(Exception ex, string tag, [CallerMemberName] string callerMemberName = null);
	}
}
