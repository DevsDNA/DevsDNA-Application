namespace DevsDNA.Application.Features.Videos
{
    using DevsDNA.Application.Features.Videos.APIModels;
    using Refit;
    using System;

    public interface IYouTubeAPIService
    {
        
        [Get("/feeds/videos.xml?channel_id={channel-id}")]
        IObservable<Feed> GetChannelFeed([AliasAs("channel-id")] string channelId);
    }
}
