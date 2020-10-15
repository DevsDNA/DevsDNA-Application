namespace DevsDNA.Application.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.Content.Res;
    using Android.OS;
    using Android.Runtime;
    using DevsDNA.Application.Common;
    using DevsDNA.Application.Messages;
    using DevsDNA.Application.Services;
    using DevsDNA.Application.Services.Notifications;
    using Lottie.Forms.Droid;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Orientation = Messages.Orientation;

    [Activity(
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Icon = "@drawable/launcher",
        Label = "DevsDNA",
        LaunchMode = LaunchMode.SingleTop,
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/SplashTheme"
        )]
    public class MainActivity : FormsAppCompatActivity
    {
        private ILogService logService;
        private ITrackService trackService;

        public static Context CurrentActivity { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentOnUnhandledException;

            SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);

			CurrentActivity = this;
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			Forms.SetFlags("CarouselView_Experimental", "Markup_Experimental");
			Forms.Init(this, savedInstanceState);
			AnimationViewRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            logService = CustomDependencyService.Instance.Get<ILogService>();
            trackService = CustomDependencyService.Instance.Get<ITrackService>();

            LoadApplication(new App());
            OnNewIntent(Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

         

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {                
                MessagingCenter.Instance.Send(Xamarin.Forms.Application.Current, SettingsKeyValues.MessagingCenterOrientationChangedMessage, new OrientationMessage(Orientation.Landscape));
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                MessagingCenter.Instance.Send(Xamarin.Forms.Application.Current, SettingsKeyValues.MessagingCenterOrientationChangedMessage, new OrientationMessage(Orientation.Portrait));
            }

            base.OnConfigurationChanged(newConfig);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            PushNotificationsType pushType = PushNotificationsType.None;
            if (intent != null && intent.HasExtra("pushType"))
            {
                pushType = (PushNotificationsType)intent.Extras.GetInt("pushType");
            }

            (Xamarin.Forms.Application.Current as App).NewNotificationReceivedAsync(pushType).ConfigureAwait(false);
        }



        private void AndroidEnvironmentOnUnhandledException(object sender, RaiseThrowableEventArgs e)
        {
            logService.LogError(e.Exception);
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            logService.LogError(e.Exception);

            Dictionary<string, string> dict = GetExceptionProperties(e.Exception);
            trackService.TrackError(e.Exception, dict);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logService.LogError((Exception)e.ExceptionObject);

            Dictionary<string, string> dict = GetExceptionProperties(e.ExceptionObject as Exception);
            trackService.TrackError(e.ExceptionObject as Exception, dict);
        }

        private Dictionary<string, string> GetExceptionProperties(Exception ex, string prefix = "EX", int i = 0, Dictionary<string, string> dict = null)
		{
			dict = dict ?? new Dictionary<string, string>();
			if (i == 0)
            {
                dict.Add($"{prefix}Message", ex.Message);
                dict.Add($"{prefix}StackTrace", ex.StackTrace);
            }
            else
            {
                dict.Add($"{prefix}{i}Message", ex.Message);
                dict.Add($"{prefix}{i}StackTrace", ex.StackTrace);
            }

            if (ex.InnerException != null)
            {
                dict = GetExceptionProperties(ex.InnerException, "INEX", i++, dict);
            }
            return dict;
        }
    }
}