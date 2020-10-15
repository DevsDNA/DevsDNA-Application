[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.iOS.Effects.DropShadowEffect),
                                      nameof(DevsDNA.Application.iOS.Effects.DropShadowEffect))]
namespace DevsDNA.Application.iOS.Effects
{
    using CoreGraphics;
    using System.Linq;
    using Xamarin.Forms.Platform.iOS;

    public class DropShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var view = Control ?? Container;
            var dropEffect = (DevsDNA.Application.Effects.DropShadowEffect)Element.Effects.FirstOrDefault(e => e is DevsDNA.Application.Effects.DropShadowEffect);
            if (view == null || dropEffect == null)
                return;

            view.Layer.MasksToBounds = false;
            view.Layer.ShadowColor = dropEffect.ShadowColor.ToCGColor();
            view.Layer.ShadowOpacity = dropEffect.Opacity;
            view.Layer.ShadowOffset = new CGSize(dropEffect.Offset.X, dropEffect.Offset.Y);
            view.Layer.ShadowRadius = dropEffect.Radius;
        }

        protected override void OnDetached()
        {
        }
    }
}