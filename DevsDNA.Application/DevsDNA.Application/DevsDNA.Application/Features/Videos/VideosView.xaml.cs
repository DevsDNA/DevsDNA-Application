namespace DevsDNA.Application.Features.Videos
{
	using DevsDNA.Application.Features.Main;
    using ReactiveUI;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
	using System;
    using Xamarin.Forms;
    using System.Reactive;

    public partial class VideosView : ITransitionable
	{
		private const double carouselItemRatio = 1.5;

		public VideosView()
		{
			InitializeComponent();
		}

		public IObservable<ItemsViewScrolledEventArgs> ScrolledObservable { get; private set; }
		public IObservable<Unit> OpeningVideoObservable { get; private set; }


		public Task DoAppearingAnimationAsync()
		{
			return CarouselVideos.OnAppearingAsync();
		}

		public Task DoDissappearingAnimationAsync()
		{
			return CarouselVideos.OnDisappearingAsync();
		}

		public Task Reset()
		{
			return Task.CompletedTask;
		}


		protected override CompositeDisposable CreateBindings(CompositeDisposable disposables)
		{
			base.CreateBindings(disposables);
						
			disposables.Add(this.WhenAnyValue(view => view.ViewModel.VideosModel).Subscribe(CarouselVideos.UpdateItemsSource));
			disposables.Add(ViewModel.WhenAnyValue(vm => vm.VideosModel).Select(videos => videos == null).Subscribe(isLoading =>
			{
				if (isLoading)
					StartLoadingAnimation(LottieLoading);
				else
					StopLoadingAnimation(LottieLoading);
			}));
			ScrolledObservable = Observable.FromEventPattern<ItemsViewScrolledEventArgs>(h => CarouselVideos.Scrolled += h, h => CarouselVideos.Scrolled -= h).Select(ep => ep.EventArgs);
			OpeningVideoObservable = ViewModel.OpenVideoCommand.IsExecuting.Where(c => c).Select(_ => Unit.Default);

			return disposables;
		}

		protected override void OnFirstSizeAllocated(double width, double height)
		{
			base.OnFirstSizeAllocated(width, height);

			AdjustCarouselItemsRatio();
		}

		private void AdjustCarouselItemsRatio()
		{
			Size carouselItemSize = VideoCarouselItemView.CalculateVisibleArea(new Size(CarouselVideos.Width, CarouselVideos.Height));			
			CarouselVideos.AdjustCarouselItemsRatio(carouselItemRatio, carouselItemSize);
		}
	}
}