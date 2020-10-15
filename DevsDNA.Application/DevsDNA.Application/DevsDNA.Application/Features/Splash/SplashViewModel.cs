namespace DevsDNA.Application.Features.Splash
{
	using DevsDNA.Application.Base;
	using DevsDNA.Application.Common;
	using DevsDNA.Application.Services;
	using ReactiveUI;
	using System;
	using System.Reactive;
	using System.Reactive.Subjects;
	using System.Threading.Tasks;
	using Xamarin.Essentials;
	using Xamarin.Forms;
	using System.Reactive.Linq;
	using DevsDNA.Application.Features.Main;

	public class SplashViewModel : BaseViewModel
	{
		private readonly INotificationService notificationService;
		private readonly IDataService dataService;
		private ObservableAsPropertyHelper<bool> isLoadingPropertyHelper;
		private readonly Subject<Unit> finishedSplash;

		public SplashViewModel(IDependencyService dependencyService) : base(dependencyService)
		{
			notificationService = dependencyService.Get<INotificationService>();
			dataService = dependencyService.Get<IDataService>();
			finishedSplash = new Subject<Unit>();

			NavigateCommand = ReactiveCommand.CreateFromTask(NavigateCommandExecuteAsync);
			StartCommand = ReactiveCommand.CreateFromTask(StartCommandExecuteAsync);
		}


		public IObservable<Unit> FinishedSplash => finishedSplash;

		public ReactiveCommand<Unit, Unit> NavigateCommand { get; }

		public ReactiveCommand<Unit, Unit> StartCommand { get; }

		protected override ObservableAsPropertyHelper<bool> IsLoadingPropertyHelper
		{
			get => isLoadingPropertyHelper;
			set => isLoadingPropertyHelper = value;
		}

		public TabMain TabMainToOpen { get; set; } = TabMain.News;


		public override async Task AppearingAsync()
		{
			await base.AppearingAsync();
			Disposables.Add(NavigateCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
			Disposables.Add(NavigateCommand.IsExecuting.ToProperty(this, vm => vm.IsLoading, out isLoadingPropertyHelper));
			Disposables.Add(StartCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
			Disposables.Add(StartCommand.IsExecuting.ToProperty(this, vm => vm.IsLoading, out isLoadingPropertyHelper));

			if (await CheckInternetConnectionAsync())
			{
				dataService.Retrieve(DataType.All);
			}
			else
			{
				AppInfo.ShowSettingsUI();
			}
		}

		private async Task<bool> CheckInternetConnectionAsync()
		{
			try
			{
				IsLoading = true;
				switch (Connectivity.NetworkAccess)
				{
					case NetworkAccess.None:
					case NetworkAccess.Local:
						await Application.Current.MainPage.DisplayAlert(Strings.Strings.AlertNotificationTitle, Strings.Strings.AlertNoConnectivity, Strings.Strings.AlertOk);
						return false;
					case NetworkAccess.Unknown:
					case NetworkAccess.ConstrainedInternet:
						bool result = await Application.Current.MainPage.DisplayAlert(Strings.Strings.AlertNotificationTitle, Strings.Strings.AlertPoorConnectivity, Strings.Strings.AlertIWantKeepGoing, Strings.Strings.AlertCheckConnectionAgain);
						if (result)
						{
							return true;
						}
						else
						{
							await Task.Delay(TimeSpan.FromSeconds(2));
							return await CheckInternetConnectionAsync();
						}
					case NetworkAccess.Internet:
					default:
						return true;
				}
			}
			catch (Exception ex)
			{
				TrackService.TrackError(ex);
				LogService.LogError(ex);

				bool result = await Application.Current.MainPage.DisplayAlert(Strings.Strings.AlertNotificationTitle, Strings.Strings.AlertErrorCheckingConnection, Strings.Strings.AlertCheckConnectionAgain, Strings.Strings.AlertNo);
				if (result)
				{
					await Task.Delay(TimeSpan.FromSeconds(2));
					return await CheckInternetConnectionAsync();
				}
				return false;
			}
			finally
			{
				IsLoading = false;
			}
		}

		private async Task NavigateCommandExecuteAsync()
		{
			if (Preferences.Get(SettingsKeyValues.PreferenceKeyFirstTimeKey, true))
			{
				Preferences.Set(SettingsKeyValues.PreferenceKeyFirstTimeKey, false);
				NavigateToFirstTime();
			}
			else
			{
				await NavigateToMainAsync();
			}
		}

		private async Task StartCommandExecuteAsync()
		{
			// Ask for notification
			bool result = await Application.Current.MainPage.DisplayAlert(Strings.Strings.AlertNotificationTitle, Strings.Strings.AlertNotificationText, Strings.Strings.AlertYeah, Strings.Strings.AlertNo);
			if (result)
			{
				TrackService.TrackEvent("SetNotificationTrue");
				notificationService.EnableNotifications();
			}
			else
			{
				TrackService.TrackEvent("SetNotificationFalse");
				notificationService.DisableNotifications();
			}

			TrackService.TrackEvent("Initial screen completed");
			await NavigateToMainAsync();
		}

		private void NavigateToFirstTime()
		{
			finishedSplash.OnNext(Unit.Default);
		}

		private async Task NavigateToMainAsync()
		{
			MainView mainView = new MainView(TabMainToOpen);
			INavigation navigation = (Application.Current.MainPage as NavigationPage)?.Navigation;
			NavigationPage.SetHasNavigationBar(mainView, false);
			Page splashView = navigation.NavigationStack[0];
			await navigation?.PushAsync(mainView, false);
			navigation.RemovePage(splashView);
		}

	}
}