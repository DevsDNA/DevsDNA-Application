namespace DevsDNA.Application.Features.Videos
{
    using DevsDNA.Application.Controls.Carousel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public partial class VideoCarouselItemView : ContentViewAnimated
    {
        private const double verticalTranslation = 35;
        private const double horizontalSpacing = 15;

        public const string ImageTransitionName = "VideoThumbnailImage";

        public static readonly BindableProperty ShareCommandProperty = BindableProperty.Create(nameof(ShareCommand), typeof(ICommand), typeof(VideoCarouselItemView), null);

        public VideoCarouselItemView()
        {
            InitializeComponent();
            FrontTranslation = verticalTranslation;
            GridContent.Margin = new Thickness(0, verticalTranslation, 0, 0);
            GridContent.Padding = new Thickness(horizontalSpacing, 0);
            FrameContent.Margin = new Thickness(0, 0, 0, verticalTranslation);
        }

        public override View OpacityView => BoxViewOpacity;

        public ICommand ShareCommand
        {
            get => (ICommand)GetValue(ShareCommandProperty);
            set => SetValue(ShareCommandProperty, value);
        }

        public override Task EnterDownAsync()
        {
            GridContent.TranslationY = GridContent.Height;
            return Task.WhenAll(OpacityView.FadeTo(0), GridContent.TranslateTo(0, -FrontTranslation));
        }

        public override Task EnterLeftAsync()
        {
            GridContent.TranslationX = -GridContent.Width;
            return Task.WhenAll(OpacityView.FadeTo(BackOpacity), GridContent.TranslateTo(0, 0));
        }

        public override Task EnterRightAsync()
        {
            GridContent.TranslationX = GridContent.Width;
            return Task.WhenAll(OpacityView.FadeTo(BackOpacity), GridContent.TranslateTo(0, 0));
        }

        public override Task LeaveDownAsync()
        {
            return Task.WhenAll(OpacityView.FadeTo(0), GridContent.TranslateTo(0, GridContent.Height));
        }

        public override async Task LeaveRightAsync()
        {
            await Task.Delay(250);
            await Task.WhenAll(OpacityView.FadeTo(0), GridContent.TranslateTo(GridContent.Width, 0));
        }

        public override async Task LeaveLeftAsync()
        {
            await Task.Delay(250);
            await Task.WhenAll(OpacityView.FadeTo(0), GridContent.TranslateTo(-GridContent.Width, 0));
        }

        public static Size CalculateVisibleArea(Size baseArea)
        {
            return new Size(baseArea.Width - (horizontalSpacing * 2), baseArea.Height - (verticalTranslation * 2));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext as VideoModel != null)
            {
                AutomationProperties.SetName(BtnShare, $"{Strings.Strings.AccessibleBtnShareVideo} {((VideoModel)BindingContext).Title}");
            }
        }
    }
}