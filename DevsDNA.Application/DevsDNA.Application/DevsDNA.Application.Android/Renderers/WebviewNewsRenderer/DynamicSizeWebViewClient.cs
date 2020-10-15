namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Runtime;
    using System;
    using Xamarin.Forms.Platform.Android;

    class DynamicSizeWebViewClient : FormsWebViewClient
    {
        private WebViewRenderer webViewRenderer;

        public DynamicSizeWebViewClient(WebViewRenderer webViewRenderer) 
            : base(webViewRenderer)
        {
            this.webViewRenderer = webViewRenderer;
        }

        protected DynamicSizeWebViewClient(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference,transfer)
        {

        }

        public async override void OnPageFinished(Android.Webkit.WebView view, string url)
        {            
            base.OnPageFinished(view, url);

            if (webViewRenderer?.Element == null)
                return;

            var contentHeight = await webViewRenderer.Element.EvaluateJavaScriptAsync("document.body.scrollHeight");
            if (double.TryParse(contentHeight?.ToString(), out double height))
            {
                webViewRenderer.Element.HeightRequest = height;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                webViewRenderer = null;
        }
    }
}