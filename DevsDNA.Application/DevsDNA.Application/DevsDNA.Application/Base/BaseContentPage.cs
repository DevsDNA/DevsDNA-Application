namespace DevsDNA.Application.Base
{
	using DevsDNA.Application.Services;
	using ReactiveUI;
	using ReactiveUI.XamForms;
	using System.Reactive.Disposables;
    using System.Threading.Tasks;

    public class BaseContentPage<TViewModel> : ReactiveContentPage<TViewModel> where TViewModel : BaseViewModel
	{
		private bool isAlreadyInitialized;

		public BaseContentPage()
		{
			DependencyService = CustomDependencyService.Instance;
			LogService = DependencyService.Get<ILogService>();
			TrackService = DependencyService.Get<ITrackService>();
			this.WhenActivated(d => CreateBindings(d));
		}

		protected IDependencyService DependencyService { get; }
		protected ILogService LogService { get; }
		protected ITrackService TrackService { get; }


		protected virtual CompositeDisposable CreateBindings(CompositeDisposable disposables)
		{
			disposables.Add(this.OneWayBind(ViewModel, vm => vm.IsLoading, v => v.IsBusy, b => b));
			return disposables;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await ViewModel?.AppearingAsync();

			if (!isAlreadyInitialized)
			{
				isAlreadyInitialized = true;
				await OnFirstAppearingAsync();
			}
		}

		protected override async void OnDisappearing()
		{
			await ViewModel?.DisappearingAsync();
			base.OnDisappearing();
		}

		protected virtual Task OnFirstAppearingAsync()
        {
			return Task.CompletedTask;
        }
	}
}
