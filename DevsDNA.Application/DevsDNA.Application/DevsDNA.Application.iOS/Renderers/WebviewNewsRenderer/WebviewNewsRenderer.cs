[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.News.NewsDetail.WebviewNews), typeof(DevsDNA.Application.iOS.Renderers.WebviewNewsRenderer))]
namespace DevsDNA.Application.iOS.Renderers
{
    using Xamarin.Forms.Platform.iOS;

    public class WebviewNewsRenderer : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Element.HeightRequest = 1;
                ScrollView.ScrollEnabled = false;
                ScrollView.Bounces = false;
                ScrollView.Delegate = new DisableZoomScrollDelegate();
                NavigationDelegate = new WebviewNewsNavigationDelegate();
            }
        }
    }
}