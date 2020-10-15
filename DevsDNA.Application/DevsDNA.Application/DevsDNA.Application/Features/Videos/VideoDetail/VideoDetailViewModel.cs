namespace DevsDNA.Application.Features.Videos
{
    using DevsDNA.Application.Base;
    using DevsDNA.Application.Services;
    using ReactiveUI;
    using System;
    using System.Reactive;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class VideoDetailViewModel : BaseViewModel
    {
        public VideoDetailViewModel(IDependencyService dependencyService, VideoModel videoModel) : base(dependencyService)
        {
            VideoModel = videoModel;
            CloseCommand = ReactiveCommand.CreateFromTask(CloseCommandExecuteAsync);
        }

        public VideoDetailViewModel(VideoModel videoModel) : this(CustomDependencyService.Instance, videoModel)
        {
        }


        public VideoModel VideoModel { get; }

        public ReactiveCommand<Unit, Unit> CloseCommand { get; }


        public override async Task AppearingAsync()
        {
            await base.AppearingAsync();
            Disposables.Add(CloseCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));            
        }


        private Task CloseCommandExecuteAsync()
        {
            INavigation navigation = (Application.Current.MainPage as NavigationPage)?.Navigation;
            return navigation.PopAsync();
        }
    }
}
