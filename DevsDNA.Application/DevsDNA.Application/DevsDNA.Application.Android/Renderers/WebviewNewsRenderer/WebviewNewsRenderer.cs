[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.News.NewsDetail.WebviewNews), typeof(DevsDNA.Application.Droid.Renderers.WebviewNewsRenderer))]
namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Content;
    using Android.Webkit;
    using System;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    public class WebviewNewsRenderer : WebViewRenderer
    {
        public WebviewNewsRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                Element.Navigating -= OnWebviewNavigating;
            }

            if (e.NewElement != null && Control != null)
            {
                Element.Navigating += OnWebviewNavigating;
                Control.HorizontalScrollBarEnabled = false;
                Control.VerticalScrollBarEnabled = false;
            }
        }

        protected override WebViewClient GetWebViewClient()
        {
            return new DynamicSizeWebViewClient(this);
        }

        private void OnWebviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (Uri.IsWellFormedUriString(e.Url, UriKind.Absolute))
            {
                Launcher.TryOpenAsync(e.Url);
                e.Cancel = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {                
                if (Element != null)
                {
                    Element.Navigating -= OnWebviewNavigating;
                }
            }
        }
    }
}