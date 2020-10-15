namespace DevsDNA.Application.Tests.TestServices
{
    using DevsDNA.Application.Features.SocialNetwork.Facebook;
    using DevsDNA.Application.Helpers;
    using DevsDNA.Application.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Refit;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reactive.Linq;

    [TestClass]
    public class FacebookAPIServiceTest
    {
        private IFacebookAPIService facebookAPIService;

        [TestInitialize]
        public void SetUp()
        {
            facebookAPIService = RestService.For<IFacebookAPIService>(SettingsKeyValues.FacebookEndPoint);
        }

        [TestMethod]
        public async Task GetPageFeed()
        {
            var pageFeed = await facebookAPIService.GetPageFeed(SettingsKeyValues.FacebookPageId, Secrets.FacebookAccessToken);

            Assert.IsTrue(pageFeed.Posts.Any());
        }
    }
}
