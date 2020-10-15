[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.Droid.Effects.LabelShadowEffect),
                                      nameof(DevsDNA.Application.Droid.Effects.LabelShadowEffect))]
namespace DevsDNA.Application.Droid.Effects
{
    using Android.Widget;
    using System.Linq;
    using Xamarin.Forms.Platform.Android;    

    public class LabelShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var labelShadowEffect = (Application.Effects.LabelShadowEffect)Element.Effects.FirstOrDefault(e => e is Application.Effects.LabelShadowEffect);
            if (Control is TextView textView)
            {
                textView.SetShadowLayer(1, 1, 1, labelShadowEffect.Color.ToAndroid());
            }           
        }

        protected override void OnDetached()
        {
        }
    }
}