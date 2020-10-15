namespace DevsDNA.Application.Features.News
{
    using DevsDNA.Application.Controls.Carousel;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public partial class NewsCarouselItemView : ContentViewAnimated
    {
        private const double verticalTranslation = 35;
        private const double horizontalSpacing = 15;

        public const string ImageTransitionName = "BlogItemImage";

        public NewsCarouselItemView()
        {
            InitializeComponent();
            FrontTranslation = verticalTranslation;
            SdContent.Margin = new Thickness(0, verticalTranslation, 0, 0);
            SdContent.Padding = new Thickness(horizontalSpacing, 0);
            FrameContent.Margin = new Thickness(0, 0, 0, verticalTranslation);
        }

        public override View OpacityView => BoxViewOpacity;


        public override Task EnterDownAsync()
        {
            SdContent.TranslationY = SdContent.Height;
            return Task.WhenAll(OpacityView.FadeTo(0), SdContent.TranslateTo(0, -FrontTranslation));
        }

        public override Task EnterLeftAsync()
        {
            SdContent.TranslationX = -SdContent.Width;
            return Task.WhenAll(OpacityView.FadeTo(BackOpacity), SdContent.TranslateTo(0, 0));
        }

        public override Task EnterRightAsync()
        {
            SdContent.TranslationX = SdContent.Width;
            return Task.WhenAll(OpacityView.FadeTo(BackOpacity), SdContent.TranslateTo(0, 0));
        }

        public override Task LeaveDownAsync()
        {
            return Task.WhenAll(OpacityView.FadeTo(0), SdContent.TranslateTo(0, SdContent.Height));
        }

        public override async Task LeaveRightAsync()
        {
            await Task.Delay(250);
            await Task.WhenAll(OpacityView.FadeTo(0), SdContent.TranslateTo(SdContent.Width, 0));
        }

        public override async Task LeaveLeftAsync()
        {
            await Task.Delay(250);
            await Task.WhenAll(OpacityView.FadeTo(0), SdContent.TranslateTo(-SdContent.Width, 0));
        }

        public static Size CalculateVisibleArea(Size baseArea)
        {
            return new Size(baseArea.Width - (horizontalSpacing * 2), baseArea.Height - (verticalTranslation * 2));
        }
    }
}