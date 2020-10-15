namespace DevsDNA.Application.Features.News
{
	using DevsDNA.Application.Base;
	using DevsDNA.Application.Services;
	using ReactiveUI;
	using System.Reactive.Linq;
	using System.Collections.Generic;
	using System;
	using System.Reactive;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using Plugin.SharedTransitions;

    public class NewsViewModel : BaseViewModel
	{
		private IList<BlogItemModel> blogItemsModel;

		public NewsViewModel(IDependencyService dependencyService) : base(dependencyService)
		{
			OpenDetailCommand = ReactiveCommand.CreateFromTask<BlogItemModel>(OpenDetailCommandExecuteAsync);
			Disposables.Add(dependencyService.Get<IDataService>().BlogModel.ObserveOn(RxApp.MainThreadScheduler).Subscribe(bm => BlogItemsModel = bm?.Posts));
			Disposables.Add(OpenDetailCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
		}

		public IList<BlogItemModel> BlogItemsModel
		{
			get => blogItemsModel;
			set => this.RaiseAndSetIfChanged(ref blogItemsModel, value);
		}

		public ReactiveCommand<BlogItemModel, Unit> OpenDetailCommand { get; }

		private async Task OpenDetailCommandExecuteAsync(BlogItemModel blogItemModel)
		{
 			INavigation navigation = (Application.Current.MainPage as NavigationPage)?.Navigation;
			SharedTransitionNavigationPage.SetTransitionSelectedGroup(navigation.NavigationStack[0], blogItemModel.Id);
			await navigation.PushAsync(new NewsDetailView() { ViewModel = new NewsDetailViewModel(DependencyService, blogItemModel) });
		}
	}
}
