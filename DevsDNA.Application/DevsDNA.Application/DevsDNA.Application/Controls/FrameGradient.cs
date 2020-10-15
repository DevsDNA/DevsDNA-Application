namespace DevsDNA.Application.Controls
{
    using Xamarin.Forms;

    public class FrameGradient : Frame
    {        
        public static BindableProperty StartColorProperty = BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(FrameGradient), Color.Default);

        public static BindableProperty EndColorProperty = BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(FrameGradient), Color.Default);

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }
    }
}
