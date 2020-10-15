namespace DevsDNA.Application.Features.AboutUs
{
	using DevsDNA.Application.Features.Main;
	using ReactiveUI;
	using System;
	using System.Linq;
	using System.Reactive.Disposables;
	using System.Reactive.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Xamarin.Forms;

	public partial class AboutUsView : ITransitionable
	{
		private CancellationTokenSource cancellationTokenSource;
		private CancellationToken cancellationToken;

		public AboutUsView(double topPadding)
		{
			InitializeComponent();

			LblTitle.Margin = new Thickness(0, topPadding, 0, 0);
		}


		public IObservable<ItemsViewScrolledEventArgs> ScrolledObservable { get; private set; }


		public async Task DoAppearingAnimationAsync()
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = new CancellationTokenSource();
			cancellationToken = cancellationTokenSource.Token;

			await Task.WhenAny(LblTitle.FadeTo(1), LblSubtitle.ShowAsync(cancellationToken, 50, false));
		}

		public async Task DoDissappearingAnimationAsync()
		{
			MainCollection.ScrollTo(0, position: ScrollToPosition.Start);
			await LblTitle.FadeTo(0);
		}

		public Task Reset()
		{
			return Task.CompletedTask;
		}

		public void ScrollToInit()
		{
			MainCollection.ScrollTo(0, position: ScrollToPosition.Start);
		}

		protected override CompositeDisposable CreateBindings(CompositeDisposable disposables)
		{
			base.CreateBindings(disposables);
			ScrolledObservable = Observable.FromEventPattern<ItemsViewScrolledEventArgs>(h => MainCollection.Scrolled += h, h => MainCollection.Scrolled -= h).Select(ep => ep.EventArgs);

			disposables.Add(this.OneWayBind(ViewModel, vm => vm.ThunderMates, v => v.MainCollection.ItemsSource));


			return disposables;
		}
	}
}