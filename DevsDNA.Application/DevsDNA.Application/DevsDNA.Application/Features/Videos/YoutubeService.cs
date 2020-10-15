[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Features.Videos.YouTubeService))]
namespace DevsDNA.Application.Features.Videos
{
    using DevsDNA.Application.Common;
    using Refit;
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;

    public class YouTubeService : IYouTubeService
    {
        private readonly IYouTubeAPIService youTubeAPIService;

        public YouTubeService() : this(RestService.For<IYouTubeAPIService>(SettingsKeyValues.YouTubeEndPoint, new RefitSettings(new XmlContentSerializer())))
        {
        }

        public YouTubeService(IYouTubeAPIService youTubeAPIService)
        {
            this.youTubeAPIService = youTubeAPIService;
        }

        public IObservable<IList<VideoModel>> GetVideos()
        {
            return youTubeAPIService.GetChannelFeed(SettingsKeyValues.YouTubeChannelId).Select(v => v?.ToVideos());
        }
    }
}
