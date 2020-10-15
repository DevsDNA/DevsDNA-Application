[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Services.Notifications.NotificationManagerService))]
namespace DevsDNA.Application.Services.Notifications
{
    using DevsDNA.Application.Features.Main;
    using DevsDNA.Application.Features.Splash;
    using System;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using System.Reactive.Linq;
    using System.ComponentModel;

    public class NotificationManagerService : INotificationManagerService
    {
        private readonly IDataService dataService;

        public NotificationManagerService() : this(CustomDependencyService.Instance)
        {
        }

        public NotificationManagerService(IDependencyService dependencyService)
        {
            dataService = dependencyService.Get<IDataService>();
        }

        public async Task ProcessNotificationAsync(PushNotificationsType pushType)
        {
            TabMain tabMain = NotificationToTabMain(pushType);
            if (tabMain == TabMain.Unassigned)
            {
                return;
            }

            NavigationPage navigationPage = (NavigationPage)Application.Current.MainPage;
            Page rootPage = navigationPage.RootPage;

            if (rootPage is SplashView splashView && splashView.BindingContext is SplashViewModel splashViewModel)
            {             
                splashViewModel.TabMainToOpen = tabMain;
            }
            else if(rootPage is MainView mainView)
            {
                DataType dataType = NotificationToDataType(pushType);

                dataService.Clear(dataType);

                if (navigationPage.Navigation.NavigationStack.Count > 1)
                    await navigationPage.PopAsync();

                mainView.SelectedTabCommand.CanExecute.Where(ce => ce).Take(1)
                    .Do(_ => mainView.SelectedTabCommand.Execute(tabMain).Subscribe(_ => { dataService.Retrieve(dataType); })).Subscribe();
            }
        }

        private TabMain NotificationToTabMain(PushNotificationsType pushNotificationsType)
        {
            return pushNotificationsType switch
            {
                PushNotificationsType.News => TabMain.News,
                PushNotificationsType.Video => TabMain.Videos,
                PushNotificationsType.SocialNetwork => TabMain.SocialNetwork,
                _ => TabMain.Unassigned,
            };
        }

        private DataType NotificationToDataType(PushNotificationsType pushNotificationsType)
        {
            return pushNotificationsType switch
            {
                PushNotificationsType.News => DataType.News,
                PushNotificationsType.Video => DataType.Videos,
                PushNotificationsType.SocialNetwork => DataType.SocialNetwork,
                _ => throw new InvalidEnumArgumentException(),
            };
        }

    }
}
