[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.News.NewsDetail.WebviewNews), typeof(DevsDNA.Application.iOS.Renderers.WebviewNewsRenderer))]
namespace DevsDNA.Application.iOS.Renderers
{
    using System;
    using WebKit;
    using Xamarin.Essentials;
    using Xamarin.Forms.Platform.iOS;

    public class WebviewNewsNavigationDelegate : WKNavigationDelegate
    {
        public async override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (!webView.IsLoading && webView is WkWebViewRenderer wkWebViewRenderer)
            {
                var contentHeight = await webView.EvaluateJavaScriptAsync("document.body.scrollHeight");
                if(double.TryParse(contentHeight?.ToString(), out double height))
                {
                    wkWebViewRenderer.Element.HeightRequest = height;
                }
            }
        }
        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            if (!webView.IsLoading)
            {
                Launcher.TryOpenAsync(navigationAction.Request.Url.ToString());
                decisionHandler(WKNavigationActionPolicy.Cancel);
                return;
            }

            decisionHandler(WKNavigationActionPolicy.Allow);
        }
    }
}