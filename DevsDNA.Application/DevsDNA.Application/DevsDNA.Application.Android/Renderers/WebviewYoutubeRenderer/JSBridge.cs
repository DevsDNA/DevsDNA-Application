[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.Videos.VideoDetail.WebviewYoutube), typeof(DevsDNA.Application.Droid.Renderers.WebviewYoutubeRenderer))]
namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Webkit;
    using DevsDNA.Application.Features.Videos.VideoDetail;
    using Java.Interop;
    using System;

    class JSBridge : Java.Lang.Object
    {
        private readonly WeakReference<WebviewYoutubeRenderer> webviewRenderer;

        public JSBridge(WebviewYoutubeRenderer webviewYoutubeRenderer)
        {
            webviewRenderer = new WeakReference<WebviewYoutubeRenderer>(webviewYoutubeRenderer);
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            if (webviewRenderer != null && webviewRenderer.TryGetTarget(out WebviewYoutubeRenderer webviewYoutubeRenderer) &&
                webviewYoutubeRenderer.Element is WebviewYoutube webviewYoutube)
            {
                webviewYoutube.InvokeAction(data);
            }
        }
    }
}