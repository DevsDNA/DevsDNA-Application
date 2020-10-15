[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.Videos.VideoDetail.WebviewYoutube), typeof(DevsDNA.Application.iOS.Renderers.WebviewYoutubeRenderer))]
namespace DevsDNA.Application.iOS.Renderers
{
    using DevsDNA.Application.Features.Videos.VideoDetail;
    using Foundation;
    using WebKit;
    using Xamarin.Forms.Platform.iOS;

    public class WebviewYoutubeRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        private const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        private const string NativeScriptName = "invokeAction";
        private readonly WKUserContentController userController;
        private WebviewYoutube WebviewYoutube => Element as WebviewYoutube;

        public WebviewYoutubeRenderer() : this(new WKWebViewConfiguration() { AllowsInlineMediaPlayback = true })
        {
        }

        public WebviewYoutubeRenderer(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
            userController.AddUserScript(script);
            userController.AddScriptMessageHandler(this, "invokeAction");
            
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler(NativeScriptName);
            }

            if (e.NewElement != null)
            {
                ScrollView.ScrollEnabled = false;
                ScrollView.Delegate = new DisableZoomScrollDelegate();
            }
        }
        

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            WebviewYoutube?.InvokeAction(message?.Body?.ToString());
        }
    }
}