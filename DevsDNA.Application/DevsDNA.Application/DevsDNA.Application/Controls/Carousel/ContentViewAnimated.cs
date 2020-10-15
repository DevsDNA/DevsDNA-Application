namespace DevsDNA.Application.Controls.Carousel
{
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public abstract partial class ContentViewAnimated : ContentView
    {    
        public static BindableProperty FrontTranslationProperty = BindableProperty.Create(nameof(FrontTranslation), typeof(double), typeof(ContentViewAnimated), 0d);
        
        public static BindableProperty BackOpacityProperty = BindableProperty.Create(nameof(BackOpacity), typeof(double), typeof(ContentViewAnimated), 0d);

  
        public double FrontTranslation
        {
            get { return (double)GetValue(FrontTranslationProperty); }
            set { SetValue(FrontTranslationProperty, value); }
        }

        public double BackOpacity
        {
            get { return (double)GetValue(BackOpacityProperty); }
            set { SetValue(BackOpacityProperty, value); }
        }

        public abstract View OpacityView { get; }

        public abstract Task EnterDownAsync();
        public abstract Task EnterLeftAsync();
        public abstract Task EnterRightAsync();
        public abstract Task LeaveDownAsync();
        public abstract Task LeaveRightAsync();
        public abstract Task LeaveLeftAsync();


        public void GoToFront(double percentage)
        {
            double partsPerUnit = percentage / 100;
            Content.TranslationY = -(partsPerUnit * FrontTranslation);
            OpacityView.Opacity = BackOpacity - (partsPerUnit * BackOpacity);
        }

        public void GoToBack(double percentage)
        {
            double partsPerUnit = percentage / 100;
            Content.TranslationY = -(FrontTranslation - (partsPerUnit * FrontTranslation));
            OpacityView.Opacity = partsPerUnit * BackOpacity;
        }

    }
}