[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.Droid.Effects.DropShadowEffect),
                                      nameof(DevsDNA.Application.Droid.Effects.DropShadowEffect))]
namespace DevsDNA.Application.Droid.Effects
{
    using System.Linq;
    using Xamarin.Forms.Platform.Android;

    public class DropShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var view = Control ?? Container;
            var dropEffect = (Application.Effects.DropShadowEffect)Element.Effects.FirstOrDefault(e => e is Application.Effects.DropShadowEffect);
            if (view == null || dropEffect == null)
                return;

            view.Elevation = (float)(dropEffect.Offset.X + dropEffect.Offset.Y) / 2f * 5f;
            view.SetOutlineAmbientShadowColor(dropEffect.ShadowColor.ToAndroid());
            view.SetOutlineSpotShadowColor(dropEffect.ShadowColor.ToAndroid());
        }

        protected override void OnDetached()
        { 
        }
    }
}
