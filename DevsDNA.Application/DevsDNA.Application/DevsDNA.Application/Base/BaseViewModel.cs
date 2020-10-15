namespace DevsDNA.Application.Base
{
	using DevsDNA.Application.Services;
	using ReactiveUI;
	using System.Reactive.Disposables;
	using System.Threading.Tasks;

	public class BaseViewModel : ReactiveObject
	{
		private readonly string typeName;
		private bool isLoading;

		public BaseViewModel(IDependencyService dependencyService)
		{
			DependencyService = dependencyService ?? CustomDependencyService.Instance;
			LogService = DependencyService.Get<ILogService>();
			TrackService = DependencyService.Get<ITrackService>();

			Disposables = new CompositeDisposable();
			typeName = GetType().ToString();
			LogService.Log($"Creating {typeName}");
		}


		public virtual bool IsLoading
		{
			get { return isLoading || (IsLoadingPropertyHelper?.Value ?? false); }
			set { this.RaiseAndSetIfChanged(ref isLoading, value); }
		}

		public CompositeDisposable Disposables { get; set; }

		protected IDependencyService DependencyService { get; }
		protected ILogService LogService { get; }
		protected ITrackService TrackService { get; }
		protected virtual ObservableAsPropertyHelper<bool> IsLoadingPropertyHelper { get; set; }


		public virtual Task AppearingAsync()
		{
			LogService.Log($"Appearing {typeName}");
			TrackService.TrackEvent($"Appearing {typeName}");
			Disposables ??= new CompositeDisposable();
			return Task.CompletedTask;
		}

		public virtual Task DisappearingAsync()
		{
			LogService.Log($"Disappearing {typeName}");
			TrackService.TrackEvent($"Disappearing {typeName}");
			Disposables?.Dispose();
			Disposables = null;
			return Task.CompletedTask;
		}
	}
}
