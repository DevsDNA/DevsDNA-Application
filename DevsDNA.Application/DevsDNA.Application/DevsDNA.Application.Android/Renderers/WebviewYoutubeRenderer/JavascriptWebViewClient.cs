[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.Videos.VideoDetail.WebviewYoutube), typeof(DevsDNA.Application.Droid.Renderers.WebviewYoutubeRenderer))]
namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Webkit;
    using Xamarin.Forms.Platform.Android;

    class JavascriptWebViewClient : FormsWebViewClient
    {
        private readonly string javascript;

        public JavascriptWebViewClient(WebviewYoutubeRenderer renderer, string javascript) : base(renderer)
        {
            this.javascript = javascript;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            view.EvaluateJavascript(javascript, null);
            base.OnPageFinished(view, url);
        }
    }
}