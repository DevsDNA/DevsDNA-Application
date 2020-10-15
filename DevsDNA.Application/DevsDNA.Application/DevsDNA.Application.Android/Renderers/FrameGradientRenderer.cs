[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Controls.FrameGradient), typeof(DevsDNA.Application.Droid.Renderers.FrameGradientRenderer))]
namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Content;
    using Android.Graphics.Drawables;
    using DevsDNA.Application.Controls;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    public class FrameGradientRenderer : Xamarin.Forms.Platform.Android.FastRenderers.FrameRenderer
    {
        private FrameGradient FrameGradient => Element as FrameGradient;

        public FrameGradientRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                SetGradientBackgroundColor();
            }
        }

        private void SetGradientBackgroundColor()
        {
            if (Background is GradientDrawable gradientDrawable)
            {
                gradientDrawable.SetOrientation(GradientDrawable.Orientation.LeftRight);
                gradientDrawable.SetColors(new int[] { FrameGradient.StartColor.ToAndroid(), FrameGradient.EndColor.ToAndroid() });
            }
        }
    }
}