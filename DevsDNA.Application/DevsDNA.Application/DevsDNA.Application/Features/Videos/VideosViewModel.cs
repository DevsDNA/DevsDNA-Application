namespace DevsDNA.Application.Features.Videos
{
	using DevsDNA.Application.Base;
    using DevsDNA.Application.Services;
    using Plugin.SharedTransitions;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class VideosViewModel : BaseViewModel
	{
		private IList<VideoModel> videosModel;

		public VideosViewModel(IDependencyService dependencyService) : base(dependencyService)
		{
			OpenVideoCommand = ReactiveCommand.CreateFromTask<VideoModel>(OpenVideoCommandExecuteAsync);
			ShareVideoCommand = ReactiveCommand.CreateFromTask<VideoModel>(ShareVideoCommandExecuteAsync);
			Disposables.Add(dependencyService.Get<IDataService>().YouTubeVideosModel.ObserveOn(RxApp.MainThreadScheduler).Subscribe(v => VideosModel = v));
			Disposables.Add(OpenVideoCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
			Disposables.Add(ShareVideoCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
		}

		public IList<VideoModel> VideosModel
		{
			get => videosModel;
			set => this.RaiseAndSetIfChanged(ref videosModel, value);
		}

		public ReactiveCommand<VideoModel, Unit> OpenVideoCommand { get; }
		public ReactiveCommand<VideoModel, Unit> ShareVideoCommand { get; }


		private async Task OpenVideoCommandExecuteAsync(VideoModel videoModel)
		{
			INavigation navigation = (Application.Current.MainPage as NavigationPage)?.Navigation;
			SharedTransitionNavigationPage.SetTransitionSelectedGroup(navigation.NavigationStack[0], videoModel.Id);
			await navigation.PushAsync(new VideoDetailView() { ViewModel = new VideoDetailViewModel(videoModel) });
		} 

		private Task ShareVideoCommandExecuteAsync(VideoModel videoModel)
		{
			return Share.RequestAsync(new ShareTextRequest
			{
				Uri = videoModel.Url,
				Title = videoModel.Title,
				Text = videoModel.Title
			});
		}
	}
}

