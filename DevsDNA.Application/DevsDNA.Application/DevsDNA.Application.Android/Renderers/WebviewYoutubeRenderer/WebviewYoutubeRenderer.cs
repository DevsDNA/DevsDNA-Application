[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.Videos.VideoDetail.WebviewYoutube), typeof(DevsDNA.Application.Droid.Renderers.WebviewYoutubeRenderer))]
namespace DevsDNA.Application.Droid.Renderers
{
    using Android.App;
    using Android.Content;
    using Xamarin.Forms.Platform.Android;

    public class WebviewYoutubeRenderer : WebViewRenderer
    {
        private const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
        private const string NativeScriptName = "jsBridge";

        public WebviewYoutubeRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface(NativeScriptName);
            }
            if (e.NewElement != null)
            {
                Control.SetWebViewClient(new JavascriptWebViewClient(this, $"javascript: {JavascriptFunction}"));
                Control.AddJavascriptInterface(new JSBridge(this), NativeScriptName);
                Control.SetWebChromeClient(new FullScreenClient(Context as Activity));
            }
        }
    }
}