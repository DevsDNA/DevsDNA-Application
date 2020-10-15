[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.iOS.Effects.RemoveBouncesEffect),
                                      nameof(DevsDNA.Application.iOS.Effects.RemoveBouncesEffect))]
namespace DevsDNA.Application.iOS.Effects
{
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class RemoveBouncesEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Container is UIScrollView uiScrollView)
            {
                uiScrollView.Bounces = false;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}