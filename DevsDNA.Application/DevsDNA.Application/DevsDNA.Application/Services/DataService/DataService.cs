[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Services.DataService))]
namespace DevsDNA.Application.Services
{
    using DevsDNA.Application.Features.News;
    using DevsDNA.Application.Features.SocialNetwork;
    using DevsDNA.Application.Features.Videos;
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public class DataService : IDataService
    {
        private readonly ILogService logService;
        private readonly IFacebookService facebookService;
        private readonly IRssFeedService rssFeedService;
        private readonly IYouTubeService youTubeService;

        public DataService() : this(CustomDependencyService.Instance)
        {
        }

        public DataService(IDependencyService dependencyService)
        {
            logService = dependencyService.Get<ILogService>();
            facebookService = dependencyService.Get<IFacebookService>();
            rssFeedService = dependencyService.Get<IRssFeedService>();
            youTubeService = dependencyService.Get<IYouTubeService>();
        }


        public ISubject<IList<PostModel>> FacebookPostsModel { get; } = new ReplaySubject<IList<PostModel>>(1);

        public ISubject<IList<VideoModel>> YouTubeVideosModel { get; } = new ReplaySubject<IList<VideoModel>>(1);

        public ISubject<BlogModel> BlogModel { get; } = new ReplaySubject<BlogModel>(1);

      
        public void Retrieve(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.News:
                    rssFeedService.GetBlogModel().Subscribe(BlogModel.OnNext, logService.LogError);
                    break;
                case DataType.Videos:
                    youTubeService.GetVideos().Subscribe(YouTubeVideosModel.OnNext, logService.LogError);
                    break;
                case DataType.SocialNetwork:
                    facebookService.GetPosts().Subscribe(FacebookPostsModel.OnNext, logService.LogError);
                    break;
                case DataType.All:
                    Retrieve(DataType.News); Retrieve(DataType.Videos); Retrieve(DataType.SocialNetwork);
                    break;
            }
        }

        public void Clear(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.News:
                    BlogModel.OnNext(null);
                    break;
                case DataType.Videos:
                    YouTubeVideosModel.OnNext(null);
                    break;
                case DataType.SocialNetwork:
                    FacebookPostsModel.OnNext(null);
                    break;
                case DataType.All:
                    Clear(DataType.News); Clear(DataType.Videos); Clear(DataType.SocialNetwork);
                    break;
            }
        }

    }
}
