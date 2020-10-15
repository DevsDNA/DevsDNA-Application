namespace DevsDNA.Application
{
	using DevsDNA.Application.Features.Splash;
	using DevsDNA.Application.Services;
	using DevsDNA.Application.Services.Notifications;
    using Plugin.SharedTransitions;
    using System.Threading.Tasks;
	using Xamarin.Forms;

	public partial class App : Application
	{
		private readonly INotificationManagerService notificationManagerService;
		private readonly ITrackService trackService;

		public App()
		{
			InitializeComponent();

			IDependencyService dependencyService = CustomDependencyService.Instance;
			notificationManagerService = dependencyService.Get<INotificationManagerService>();
			trackService = dependencyService.Get<ITrackService>();

			SplashView splashView = new SplashView { ViewModel = new SplashViewModel(dependencyService) };
			NavigationPage navigationPage = new SharedTransitionNavigationPage(splashView)
			{
				BarTextColor = Color.White,
				BarBackgroundColor = Color.White,
				BackgroundColor = (Color)Resources["DarkBlueColor"]
			};
			NavigationPage.SetHasNavigationBar(splashView, false);
			MainPage = navigationPage;
		}

		protected override void OnStart()
		{
			trackService.ConfigureAppCenter();
			trackService.TrackEvent("Started");
		}

		protected override void OnSleep()
		{
			trackService.TrackEvent("Sleeped");
		}

		protected override void OnResume()
		{
			trackService.TrackEvent("Resumed");
		}

		public Task NewNotificationReceivedAsync(PushNotificationsType pushType)
		{
			return notificationManagerService.ProcessNotificationAsync(pushType);
		}
	}
}
