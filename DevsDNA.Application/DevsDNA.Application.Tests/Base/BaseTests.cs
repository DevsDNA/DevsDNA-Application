namespace DevsDNA.Application.Tests.Base
{
	using DevsDNA.Application.Services;
	using Moq;
	using System.Reactive.Disposables;

	public abstract class BaseTests
	{
		protected MockDependencyService dependencyService;
		protected Mock<ILogService> logService;
		protected CompositeDisposable disposables;
		private object lockTest= new object();
		private bool isInitialized;

		protected BaseTests()
		{
			lock (lockTest)
			{
				if (!isInitialized)
				{
					isInitialized = true;
					InitializeBaseMockServices();
				}
			}
		}

		protected void InitializeBaseMockServices()
		{
			dependencyService = new MockDependencyService();

			logService = new Mock<ILogService>();
			dependencyService.Register(logService.Object);
		}

		public void ResetBaseMockServices()
		{
			logService.Reset();

			disposables?.Dispose();
			disposables = new CompositeDisposable();
		}
	}
}
