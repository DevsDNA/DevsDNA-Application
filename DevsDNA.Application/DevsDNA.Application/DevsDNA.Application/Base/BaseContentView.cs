namespace DevsDNA.Application.Base
{
	using DevsDNA.Application.Services;
    using Lottie.Forms;
    using ReactiveUI;
	using ReactiveUI.XamForms;
	using System.Reactive.Disposables;
    using Xamarin.Forms;

    public abstract class BaseContentView<TViewModel> : ReactiveContentView<TViewModel> where TViewModel : BaseViewModel
	{
		private bool isAlreadySizeAllocated;

		public BaseContentView()
		{
			DependencyService = CustomDependencyService.Instance;
			LogService = DependencyService.Get<ILogService>();
			TrackService = DependencyService.Get<ITrackService>();
		}

		protected IDependencyService DependencyService { get; }
		protected ILogService LogService { get; }
		protected ITrackService TrackService { get; }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
			if (!(ViewModel is null))
			{
				this.WhenActivated(d => CreateBindings(d));
			}
		}

        protected virtual CompositeDisposable CreateBindings(CompositeDisposable disposables)
		{
			return disposables;
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			if (!isAlreadySizeAllocated && width != -1 && height != -1)
			{
				isAlreadySizeAllocated = true;
				OnFirstSizeAllocated(width, height);
			}
		}

		protected virtual void OnFirstSizeAllocated(double width, double height)
		{
		}

		protected void StartLoadingAnimation(AnimationView animationView)
		{
			animationView.IsVisible = true;
			animationView.IsPlaying = true;
		}

		protected void StopLoadingAnimation(AnimationView animationView)
		{
			animationView.IsVisible = false;
			animationView.IsPlaying = false;
			if (Device.RuntimePlatform == Device.iOS)
			{
				animationView.Progress = 1;
				animationView.Progress = 0;
			}
		}
	}
}
