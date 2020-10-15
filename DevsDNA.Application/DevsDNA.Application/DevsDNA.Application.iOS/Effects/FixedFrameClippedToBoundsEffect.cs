[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.iOS.Effects.FixedFrameClippedToBoundsEffect),
                                      nameof(DevsDNA.Application.iOS.Effects.FixedFrameClippedToBoundsEffect))]
namespace DevsDNA.Application.iOS.Effects
{
    using System.Linq;
    using Xamarin.Forms.Platform.iOS;

    public class FixedFrameClippedToBoundsEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var frame = Container?.Subviews?.FirstOrDefault() ?? Control?.Subviews?.FirstOrDefault();
            if (frame != null)
            {
                frame.ClipsToBounds = true;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}