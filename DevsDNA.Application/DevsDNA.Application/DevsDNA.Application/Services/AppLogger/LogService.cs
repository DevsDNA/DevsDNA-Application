[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Services.AppLogger.LogService))]
namespace DevsDNA.Application.Services.AppLogger
{
	using System;
	using System.Text;
	using System.Diagnostics;
	using System.Reactive.Subjects;
	using System.Runtime.CompilerServices;

	internal class LogService : ILogService
	{
		private readonly IObservable<string> loggerObserver;
		private readonly object lockPublish;
		private readonly Subject<string> log;
		private IDisposable logSubscription;
		private bool disposedValue;
		private int line;

		public LogService()
		{
			disposedValue = false;
			lockPublish = new object();
			log = new Subject<string>();
			loggerObserver = log;
			SubscribeToLog();
		}

		~LogService()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}


		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Log(string message, [CallerMemberName] string callerMemberName = null)
		{
			Log(message, string.Empty, callerMemberName);
		}

		public void Log(string message, string tag, [CallerMemberName] string callerMemberName = null)
		{
			Publish(message, tag, callerMemberName);
		}

		public void LogError(Exception ex)
		{
			LogError(ex, string.Empty);
		}

		public void LogError(Exception ex, [CallerMemberName] string callerMemberName = null)
		{
			LogError(ex, string.Empty, callerMemberName);
		}

		public void LogError(Exception ex, string tag, [CallerMemberName] string callerMemberName = null)
		{
			Publish(GetExceptionData(ex), tag, callerMemberName);
		}



		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					logSubscription?.Dispose();
				}
				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		private void Publish(string message, string tag, string callerMemberName)
		{
			if (string.IsNullOrWhiteSpace(tag))
				tag = "INFO";

			lock (lockPublish)
			{
				log.OnNext($"### {line++} | {DateTime.Now:hh:mm:ss} | {tag} | {callerMemberName}: {message}");
			}
		}

		private void SubscribeToLog()
		{
			logSubscription = loggerObserver.Subscribe(WriteLog,
				ex =>
				{
					string text = GetExceptionData(ex);
					Debug.WriteLine(text, "LOG ERROR");
				});
		}

		private void WriteLog(string line)
		{
			Debug.WriteLine(line);
		}

		private string GetExceptionData(Exception ex, string title = "EXCEPTION")
		{
			if (ex == null)
			{
				return string.Empty;
			}

			StringBuilder st = new StringBuilder();
			st.AppendLine($"--{title}--");
			st.AppendLine($"MESSAGE: {ex.Message}");
			st.AppendLine($"TOSTRING: {ex.ToString()}");
			st.AppendLine($"STACKTRACE: {ex.StackTrace}");

			if (ex.InnerException != null)
			{
				st.AppendLine(GetExceptionData(ex.InnerException, "INNER EXCEPTION"));
			}
			return st.ToString();
		}
	}
}