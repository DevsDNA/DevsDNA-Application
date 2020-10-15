[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.Droid.Effects.LabelHeightAdjustEffect),
                                      nameof(DevsDNA.Application.Droid.Effects.LabelHeightAdjustEffect))]
namespace DevsDNA.Application.Droid.Effects
{
    using Android.Widget;
    using Java.Lang;
    using Xamarin.Forms.Platform.Android;

    public class LabelHeightAdjustEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control is TextView textView)
            {
                textView.LayoutChange += TextView_LayoutChange;
            }           
        }

        protected override void OnDetached()
        {
            if (Control is TextView textView)
            {
                textView.LayoutChange -= TextView_LayoutChange;
            }
        }

        private void TextView_LayoutChange(object sender, Android.Views.View.LayoutChangeEventArgs e)
        {
            if(sender is FormsTextView textView)
            {
                int maxLines = (int)Math.Floor((double)textView.Height / (double)textView.LineHeight);
                textView.SetLines(maxLines);
            }
        }      
    }
}