namespace DevsDNA.Application.Tests.TestServices
{
	using DevsDNA.Application.Features.News;
	using DevsDNA.Application.Tests.Base;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    [TestClass]
	public class RssFeedTests : BaseTests
	{
		private IRssFeedService rssFeedService;


		[TestInitialize]
		public void SetUp()
		{
			ResetBaseMockServices();

			rssFeedService = new RssFeedService(dependencyService);
		}

		[TestMethod]
		public async Task GetSyndicationFeed()
		{
			BlogModel blog = await rssFeedService.GetBlogModel();

			Assert.IsNotNull(blog);
		}
	}
}
