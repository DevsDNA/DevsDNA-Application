namespace DevsDNA.Application.Features.Videos
{
    using System;
    using System.Collections.Generic;

    public interface IYouTubeService
    {
        IObservable<IList<VideoModel>> GetVideos();
    }
}
