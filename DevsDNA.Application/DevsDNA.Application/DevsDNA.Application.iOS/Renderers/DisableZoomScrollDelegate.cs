[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.Videos.VideoDetail.WebviewYoutube), typeof(DevsDNA.Application.iOS.Renderers.WebviewYoutubeRenderer))]
namespace DevsDNA.Application.iOS.Renderers
{
    using UIKit;

    class DisableZoomScrollDelegate : UIScrollViewDelegate
    {        
        public override void ZoomingStarted(UIScrollView scrollView, UIView view)
        {
            if (scrollView.PinchGestureRecognizer != null)
            {
                scrollView.PinchGestureRecognizer.Enabled = false;
            }
        }
    }
}