namespace DevsDNA.Application.Tests.TestServices
{
    using DevsDNA.Application.Features.Videos;
    using DevsDNA.Application.Features.Videos.APIModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class YouTubeServiceTest
    {
        [TestMethod]
        public async Task GetVideosFromChannelFeed()
        {
            var youTubeAPIServiceMoq = new Mock<IYouTubeAPIService>();
            youTubeAPIServiceMoq.Setup(y => y.GetChannelFeed(It.IsAny<string>())).Returns(Observable.Return(GivenAFeed()));
            YouTubeService youTubeService = new YouTubeService(youTubeAPIServiceMoq.Object);

            var videos = await youTubeService.GetVideos();

            Assert.AreEqual(1, videos.Count());
        }

        [TestMethod]
        public async Task ReturnsNullIfChannelFeedIsNull()
        {
            var youTubeAPIServiceMoq = new Mock<IYouTubeAPIService>();
            youTubeAPIServiceMoq.Setup(y => y.GetChannelFeed(It.IsAny<string>())).Returns(Observable.Return<Feed>(null));
            YouTubeService youTubeService = new YouTubeService(youTubeAPIServiceMoq.Object);

            var videos = await youTubeService.GetVideos();

            Assert.IsNull(videos);
        }



        private Feed GivenAFeed()
        {
            return new Feed()
            {
                Entries = new List<Entry>()
                {
                    new Entry(){
                        Id = "CLDLDkoJ9fE",
                        Title = "Dummy title",
                        Published= "2019-05-22T09:20:48+00:00",
                        Link = new Link(){ Url = "https://www.youtube.com/watch?v=CLDLDkoJ9fE"},
                        Media = new Media(){ Description = "Dummy description", Thumbnail = new Thumbnail(){ Url ="https://i4.ytimg.com/vi/CLCLDkoJ9fE/hqdefault.jpg" }}
                    }
                }
            };
        }
    }
}
