namespace DevsDNA.Application.Features.News
{
    using ReactiveUI;
    using System.Reactive.Disposables;
    using Xamarin.Forms;
    using System.Reactive.Linq;
    using System;
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
    using System.Threading.Tasks;

    public partial class NewsDetailView
    {
        public NewsDetailView()
        {
            InitializeComponent();
        }

        protected override CompositeDisposable CreateBindings(CompositeDisposable disposables)
        {
            base.CreateBindings(disposables);

            disposables.Add(this.OneWayBind(ViewModel, vm => vm.NewsHtmlSettings, v => v.WebviewNews.NewsHtmlSettings));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.BlogItemModel.Title, v => v.LabelTitle.Text));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.BlogItemModel.Created, v => v.LabelDate.Text, d => string.Format("{0:dd MMMM yyyy}", d)));
            disposables.Add(this.WhenAnyValue(x => x.ViewModel.BlogItemModel.Categories).Subscribe(c => LayoutCategories.SetValue(BindableLayout.ItemsSourceProperty, c)));
            disposables.Add(this.BindCommand(ViewModel, vm => vm.CloseCommand, v => v.ButtonClose));
            disposables.Add(this.BindCommand(ViewModel, vm => vm.ShareBlogItemCommand, v => v.ButtonShare, vm => vm.BlogItemModel));
            var observableScrolledEvent = Observable.FromEventPattern<ScrolledEventArgs>(s => ScrollViewContent.Scrolled += s, s => ScrollViewContent.Scrolled -= s);
            disposables.Add(observableScrolledEvent.ObserveOn(RxApp.MainThreadScheduler).Subscribe(ep => OnScrolled(ep.EventArgs)));

            return base.CreateBindings(disposables);
        }

        protected override async Task OnFirstAppearingAsync()
        {
            await base.OnFirstAppearingAsync();

            await AdjustUIAsync();
        }
        
        private async Task AdjustUIAsync()
        {
            ScrollViewContent.Padding = new Thickness(0, (ImageNews.Height - 15) - (GridMain.RowDefinitions[1].Height.Value + GridContent.RowDefinitions[0].Height.Value), 0, 0);
            GridContent.TranslationY = GridContent.Height;
            if (Device.RuntimePlatform == Device.iOS)
            {
                ScrollViewContent.Margin = new Thickness(0, 0, 0, -ScrollViewContent.Padding.Top * 2);
                var topSafeAreaMargin = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets().Top;
                ImageLogo.Margin = new Thickness(ImageLogo.Margin.Left, ImageLogo.Margin.Top + topSafeAreaMargin, ImageLogo.Margin.Right, ImageLogo.Margin.Bottom);
            }
            else
            {
                await Task.Delay(50);
            }

            await Task.WhenAll(ScrollViewContent.FadeTo(1, 100), GridContent.TranslateTo(0, 0, Device.RuntimePlatform == Device.iOS ? 150u : 500u, Easing.SinOut),
                GridContent.TranslateTo(0, 0, Device.RuntimePlatform == Device.iOS ? 150u : 550u, Easing.SinOut), ImageLogo.FadeTo(1, 750));
        }
                
        private void OnScrolled(ScrolledEventArgs scrolledEventArgs)
        {
            if (scrolledEventArgs.ScrollY < ImageNews.Height)
                ImageNews.TranslationY = -(Math.Max(scrolledEventArgs.ScrollY, 0) / 2);
        }
    }
}