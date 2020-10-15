namespace DevsDNA.Application.Features.News
{
    using DevsDNA.Application.Features.Main;
    using ReactiveUI;
    using System.Reactive.Disposables;
    using System.Threading.Tasks;
	using System;
    using System.Reactive.Linq;
    using Xamarin.Forms;
    using System.Reactive;

    public partial class NewsView : ITransitionable
	{
		private const double carouselItemRatio = 1.5;

		public NewsView()
		{
			InitializeComponent();
		}

		public IObservable<ItemsViewScrolledEventArgs> ScrolledObservable { get; private set; }
		public IObservable<Unit> OpeningDetailObservable { get; private set; }


		public Task DoAppearingAnimationAsync()
		{
			return CarouselNews.OnAppearingAsync();
		}

		public Task DoDissappearingAnimationAsync()
		{
			return CarouselNews.OnDisappearingAsync();
		}

		public Task Reset()
		{			
			return Task.CompletedTask;
		}


		protected override CompositeDisposable CreateBindings(CompositeDisposable disposables)
		{
			base.CreateBindings(disposables);
					
			disposables.Add(this.WhenAnyValue(view => view.ViewModel.BlogItemsModel).Subscribe(CarouselNews.UpdateItemsSource));
			disposables.Add(ViewModel.WhenAnyValue(vm => vm.BlogItemsModel).Select(blogItem => blogItem == null).Subscribe(isLoading =>
			{
				if (isLoading)
					StartLoadingAnimation(LottieLoading);
				else
					StopLoadingAnimation(LottieLoading);
			}));
			ScrolledObservable = Observable.FromEventPattern<ItemsViewScrolledEventArgs>(h => CarouselNews.Scrolled += h, h => CarouselNews.Scrolled -= h).Select(ep => ep.EventArgs);
			OpeningDetailObservable = ViewModel.OpenDetailCommand.IsExecuting.Where(c => c).Select(_ => Unit.Default);

			return disposables;
		}

		protected override void OnFirstSizeAllocated(double width, double height)
		{
			base.OnFirstSizeAllocated(width, height);

			AdjustCarouselItemsRatio();
		}

		private void AdjustCarouselItemsRatio()
		{
			Size carouselItemSize = NewsCarouselItemView.CalculateVisibleArea(new Size(CarouselNews.Width, CarouselNews.Height));
			CarouselNews.AdjustCarouselItemsRatio(carouselItemRatio, carouselItemSize);
		}
	}
}