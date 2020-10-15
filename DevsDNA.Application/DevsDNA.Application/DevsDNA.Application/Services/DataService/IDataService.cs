namespace DevsDNA.Application.Services
{
    using DevsDNA.Application.Features.News;
    using DevsDNA.Application.Features.SocialNetwork;
    using DevsDNA.Application.Features.Videos;
    using System.Collections.Generic;
    using System.Reactive.Subjects;

    public interface IDataService
    {
        public ISubject<IList<PostModel>> FacebookPostsModel { get; }

        public ISubject<IList<VideoModel>> YouTubeVideosModel { get; }

        public ISubject<BlogModel> BlogModel { get; }

        public void Retrieve(DataType dataType);

        public void Clear(DataType dataType);
    }
}
