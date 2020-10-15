namespace DevsDNA.Application.Features.News
{
    using DevsDNA.Application.Base;
    using DevsDNA.Application.Common;
    using DevsDNA.Application.Features.News.NewsDetail;
    using DevsDNA.Application.Services;
    using ReactiveUI;
    using System;
    using System.Reactive;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class NewsDetailViewModel : BaseViewModel
    {
        public NewsDetailViewModel(IDependencyService dependencyService, BlogItemModel blogItemModel) : base(dependencyService)
        {
            BlogItemModel = blogItemModel;
            SetNewsHtmlSettings();
            CloseCommand = ReactiveCommand.CreateFromTask(CloseCommandExecuteAsync);
            ShareBlogItemCommand = ReactiveCommand.CreateFromTask<BlogItemModel>(ShareBlogItemCommandExecuteAsync);
        }
        public NewsDetailViewModel(BlogItemModel blogItemModel) : this(CustomDependencyService.Instance, blogItemModel)
        {
        }

        public NewsHtmlSettings NewsHtmlSettings { get; set; }

        public BlogItemModel BlogItemModel { get; }

        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        public ReactiveCommand<BlogItemModel, Unit> ShareBlogItemCommand { get; }


        public override async Task AppearingAsync()
        {
            await base.AppearingAsync();
            Disposables.Add(CloseCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
            Disposables.Add(ShareBlogItemCommand.ThrownExceptions.Subscribe(LogService.LogError, LogService.LogError));
        }

        private Task CloseCommandExecuteAsync()
        {
            INavigation navigation = (Application.Current.MainPage as NavigationPage)?.Navigation;
            return navigation.PopAsync();
        }

        private Task ShareBlogItemCommandExecuteAsync(BlogItemModel blogItemModel)
        {
            return Share.RequestAsync(new ShareTextRequest
            {
                Uri = blogItemModel.Link,
                Title = blogItemModel.Title,
                Text = blogItemModel.Title, 
            });
        }

        private void SetNewsHtmlSettings()
        {
            string fontPath = Device.RuntimePlatform == Device.Android ? "file:///android_asset/" : string.Empty;
            NewsHtmlSettings = new NewsHtmlSettings()
            {
                Domain = SettingsKeyValues.UrlDevsDNAEndPoint,
                Html = BlogItemModel.Content,
                RegularFont = new HtmlFont(string.Concat(fontPath, "Heebo-Regular.ttf"), (Color)Application.Current.Resources["BrowGreyColor"]),
                HeadingFont = new HtmlFont(string.Concat(fontPath, "Heebo-Black.ttf"), (Color)Application.Current.Resources["DarkBlueColor"])
            };
        }
    }
}
