namespace DevsDNA.Application.Tests.TestServices
{
    using DevsDNA.Application.Features.SocialNetwork.Facebook;
    using DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class FacebookServiceTest
    {

        [TestMethod]
        public async Task GetPostsFromPageFeed()
        {
            var facebookAPIServiceMoq = new Mock<IFacebookAPIService>();
            facebookAPIServiceMoq.Setup(f => f.GetPageFeed(It.IsAny<string>(), It.IsAny<string>())).Returns(Observable.Return(GivenAPageFeed()));
            FacebookService facebookService = new FacebookService(facebookAPIServiceMoq.Object);

            var posts = await facebookService.GetPosts();

            Assert.AreEqual(2, posts.Count());
        }

        [TestMethod]
        public async Task ReturnsNullIfPageFeedIsNull()
        {
            var facebookAPIServiceMoq = new Mock<IFacebookAPIService>();
            facebookAPIServiceMoq.Setup(f => f.GetPageFeed(It.IsAny<string>(), It.IsAny<string>())).Returns(Observable.Return<PageFeed>(null));
            FacebookService facebookService = new FacebookService(facebookAPIServiceMoq.Object);

            var posts = await facebookService.GetPosts();

            Assert.IsNull(posts);
        }

        private PageFeed GivenAPageFeed()
        {
            return new PageFeed()
            {
                Posts = new List<Post>() 
                { 
                    new Post() { Id = "112726127146289_112825800469655", CreatedTime = "2020-06-15T11:22:12+0000", Message = "Dummy message" },
                    new Post() { Id = "112222146289_112825800469655", CreatedTime = "2020-06-15T11:20:12+0000", Message = "Other dummy message" }
                }
            };
        }

    }
}
