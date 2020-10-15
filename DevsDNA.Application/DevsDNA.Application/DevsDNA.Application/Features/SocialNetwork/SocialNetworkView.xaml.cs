namespace DevsDNA.Application.Features.SocialNetwork
{
	using DevsDNA.Application.Features.Main;
    using ReactiveUI;
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public partial class SocialNetworkView : ITransitionable
	{
		public SocialNetworkView(double topPadding)
		{
			InitializeComponent();
            GridMain.RowDefinitions[0] = new RowDefinition() { Height = topPadding };
            CollectionViewHeader.HeightRequest = topPadding;
        }

        public IObservable<ItemsViewScrolledEventArgs> ScrolledObservable { get; private set; }
        public IObservable<Unit> OpeningDetailObservable { get; private set; }
        public IObservable<Unit> ClosingDetailObservable { get; private set; }


        public Task DoAppearingAnimationAsync()
		{
            return CollectionSocialNetwork.DoAppearingAnimationAsync();
        }

		public Task DoDissappearingAnimationAsync()
        {
            return Task.WhenAll(CollectionSocialNetwork.DoDissappearingAnimationAsync(), ScrollToInitAsync());
        }

        public Task Reset()
		{          
            return CollectionSocialNetwork.Reset();
        }

        public Task ScrollToInitAsync()
        {
            CollectionSocialNetwork.ScrollTo(0, position: ScrollToPosition.Start);
            return PostDetailView.HideAsync();
        }


        protected override CompositeDisposable CreateBindings(CompositeDisposable disposables)
        {
            base.CreateBindings(disposables);
                      
            disposables.Add(this.WhenAnyValue(view => view.ViewModel.PostsModel).Subscribe(async items =>
            {
                CollectionSocialNetwork.Opacity = 0;

                if (CollectionSocialNetwork.ItemsSource != null)
                    await ScrollToInitAsync();

                if (items != null)
                    CollectionSocialNetwork.ItemsSource = items;
            }));
            disposables.Add(this.Bind(ViewModel, vm => vm.SelectedPostModel, v => v.PostDetailView.BindingContext));
            disposables.Add(ViewModel.WhenAnyValue(vm => vm.SelectedPostModel).Where(pm => pm != null).Subscribe(pm => PostDetailView.ShowAsync()));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.OpenLinkCommand, v => v.PostDetailView.LinkCommand));
            disposables.Add(ViewModel.WhenAnyValue(vm => vm.PostsModel).Select(posts => posts == null).Subscribe(isLoading =>
            {
                if (isLoading)
                    StartLoadingAnimation(LottieLoading);
                else
                    StopLoadingAnimation(LottieLoading);
            }));

            ScrolledObservable = Observable.FromEventPattern<ItemsViewScrolledEventArgs>(h => CollectionSocialNetwork.Scrolled += h, h => CollectionSocialNetwork.Scrolled -= h).Select(ep => ep.EventArgs);
            OpeningDetailObservable = ViewModel.OpenPostDetailCommand.IsExecuting.Where(c => c).Select(_ => Unit.Default);
            ClosingDetailObservable = ViewModel.WhenAnyValue(vm => vm.SelectedPostModel).Select(_ => Unit.Default);

            return disposables;
        }
    }
}