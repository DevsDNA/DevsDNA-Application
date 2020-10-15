namespace DevsDNA.Application.Droid.Renderers
{
    using Android.App;
    using Android.Views;
    using Android.Webkit;
    using Android.Widget;

    class FullScreenClient : WebChromeClient
    {
        private readonly Activity context;
        private int originalUiOptions;
        private View customView;
        private ICustomViewCallback videoViewCallback;

        public FullScreenClient(Activity context)
        {
            this.context = context;
        }

        public override void OnShowCustomView(View view, ICustomViewCallback callback)
        {
            if (customView != null)
            {
                OnHideCustomView();
                return;
            }

            videoViewCallback = callback;
            customView = view;
            customView.SetBackgroundColor(Android.Graphics.Color.Black);
            context.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;

            originalUiOptions = (int)context.Window.DecorView.SystemUiVisibility;
            var newUiOptions = originalUiOptions | (int)SystemUiFlags.LayoutStable | (int)SystemUiFlags.LayoutHideNavigation | (int)SystemUiFlags.LayoutHideNavigation |
                            (int)SystemUiFlags.LayoutFullscreen | (int)SystemUiFlags.HideNavigation | (int)SystemUiFlags.Fullscreen | (int)SystemUiFlags.Immersive;
            context.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;

            if (context.Window.DecorView is FrameLayout layout)
                layout.AddView(customView, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
        }


        public override void OnHideCustomView()
        {
            if (context.Window.DecorView is FrameLayout layout)
                layout.RemoveView(customView);

            context.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            context.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)originalUiOptions;
            videoViewCallback.OnCustomViewHidden();
            customView = null;
            videoViewCallback = null;
        }
    }
}