namespace DevsDNA.Application.Features.SocialNetwork
{
	using DevsDNA.Application.Base;
	using DevsDNA.Application.Services;
	using ReactiveUI;
	using System.Collections.Generic;
	using System.Reactive.Linq;
	using System;
    using System.Reactive;
    using System.Threading.Tasks;
    using Xamarin.Essentials;

    public class SocialNetworkViewModel : BaseViewModel
	{
		private IList<PostModel> postsModel;
		private PostModel selectedPostModel;

		public SocialNetworkViewModel(IDependencyService dependencyService) : base(dependencyService)
		{
			Disposables.Add(dependencyService.Get<IDataService>().FacebookPostsModel.ObserveOn(RxApp.MainThreadScheduler).Subscribe(pm => PostsModel = pm));
			OpenPostDetailCommand = ReactiveCommand.Create<PostModel>(OpenPostDetailCommandExecute);
			OpenLinkCommand = ReactiveCommand.CreateFromTask<string>(OpenLinkCommandExecuteAsync);
			Disposables.Add(OpenPostDetailCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
			Disposables.Add(OpenLinkCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
		}

        public IList<PostModel> PostsModel
		{
			get => postsModel;
			set => this.RaiseAndSetIfChanged(ref postsModel, value);
		}

		public PostModel SelectedPostModel
        {
			get => selectedPostModel;
			set => this.RaiseAndSetIfChanged(ref selectedPostModel, value);
		}

		public ReactiveCommand<PostModel, Unit> OpenPostDetailCommand { get; }

		public ReactiveCommand<string, Unit> OpenLinkCommand { get; }


		private void OpenPostDetailCommandExecute(PostModel postModel)
		{
			SelectedPostModel = postModel;
		}

		private Task OpenLinkCommandExecuteAsync(string url)
		{
			var uri = new UriBuilder(url).Uri;
			return Browser.OpenAsync(uri, BrowserLaunchMode.External);
		}
	}
}
