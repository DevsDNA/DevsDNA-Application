[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.Droid.Effects.PageStatusBarEffect),
                                      nameof(DevsDNA.Application.Droid.Effects.PageStatusBarEffect))]
namespace DevsDNA.Application.Droid.Effects
{
    using Android.Graphics;
    using System.Linq;
    using Xamarin.Forms.Platform.Android;

    public class PageStatusBarEffect : PlatformEffect
    {
        Color defaultStatusBarColor;

        protected override void OnAttached()
        {
            var pageStatusBarEffect = (Application.Effects.PageStatusBarEffect)Element.Effects.FirstOrDefault(e => e is Application.Effects.PageStatusBarEffect);
            defaultStatusBarColor = new Color(Xamarin.Essentials.Platform.CurrentActivity.Window.StatusBarColor);
            SetStatusBarColor(pageStatusBarEffect.Color.ToAndroid());
        }

        protected override void OnDetached()
        {            
            SetStatusBarColor(defaultStatusBarColor);
        }

        private void SetStatusBarColor(Color color)
        {
            var window = Xamarin.Essentials.Platform.CurrentActivity.Window;
            window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(color);
        }
    }
}