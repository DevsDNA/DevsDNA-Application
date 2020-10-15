namespace DevsDNA.Application.Tests.TestServices
{
    using DevsDNA.Application.Features.Videos;
    using DevsDNA.Application.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Refit;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reactive.Linq;

    [TestClass]
    public class YouTubeAPIServiceTest
    {
        private IYouTubeAPIService youTubeAPIService;

        [TestInitialize]
        public void SetUp()
        {
            youTubeAPIService = RestService.For<IYouTubeAPIService>(SettingsKeyValues.YouTubeEndPoint, new RefitSettings(new XmlContentSerializer()));
        }

        [TestMethod]
        public async Task GetChannelFeed()
        {
            var feed = await youTubeAPIService.GetChannelFeed(SettingsKeyValues.YouTubeChannelId);

            Assert.IsTrue(feed.Entries.Any());
        }
    }
}
