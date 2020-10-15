[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Features.News.RssFeedService))]
namespace DevsDNA.Application.Features.News
{
    using DevsDNA.Application.Extensions;
    using DevsDNA.Application.Common;
	using DevsDNA.Application.Services;
    using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel.Syndication;
	using System.Xml;
	using System.Xml.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    public class RssFeedService : IRssFeedService
	{
		private readonly ILogService logService;

		public RssFeedService() : this(CustomDependencyService.Instance)
        {
        }

		public RssFeedService(IDependencyService dependencyService = null)
		{
			dependencyService ??= CustomDependencyService.Instance;
			logService = dependencyService.Get<ILogService>();
		}

		public IObservable<BlogModel> GetBlogModel()
		{
			return Observable.FromAsync(() => Task.Run(GetInternalBlogModel));
		}

		private BlogModel GetInternalBlogModel()
        {
			try
			{
				SyndicationFeed feed;				
				using (XmlReader reader = XmlReader.Create(SettingsKeyValues.UrlDevsDNABlog))
				{
					feed = SyndicationFeed.Load(reader);
				}

				BlogModel blog = BlogModelFromSyndicationFeed(feed);

				return blog;
			}
			catch (UriFormatException ex)
			{
				logService.LogError(ex);
				throw;
			}
			catch (XmlException ex)
			{
				logService.LogError(ex);
				throw;
			}
		}

		private BlogModel BlogModelFromSyndicationFeed(SyndicationFeed feed)
		{
			BlogModel blogModel = new BlogModel
			{
				Title = feed.Title.Text,
				Posts = PostModelsFromSyndicationItem(feed.Items)?.ToList(),
				Description = feed.Description.Text,
				Id = feed.Id,
				LastUpdate = feed.LastUpdatedTime
			};

			return blogModel;
		}

		private IEnumerable<BlogItemModel> PostModelsFromSyndicationItem(IEnumerable<SyndicationItem> items)
		{
			foreach (SyndicationItem item in items)
			{
				BlogItemModel postModel = new BlogItemModel
				{
					Authors = GetAuthors(item) ?? GetCreators(item),
					Categories = item.Categories.Select(c => c.Name)?.ToList(),
					Id = item.Id,
					Created = item.PublishDate,
					Description = item.Summary.Text,
					LastUpdate = item.LastUpdatedTime,
					Title = item.Title.Text,
					Link = item.Links?.FirstOrDefault()?.Uri?.ToString()
				};
				postModel.Content = GetContent(item);
				postModel.Image = string.Concat(SettingsKeyValues.UrlDevsDNAEndPoint, postModel.Content?.GetPathFromFirstImage());
				yield return postModel;
			}
		}

		private string GetContent(SyndicationItem item)
		{
			return item.ElementExtensions?.Where(see => see.OuterName == "encoded")
										  .Select(see => see.GetObject<XElement>()?.Value)
										 ?.FirstOrDefault();
		}

		private List<string> GetCreators(SyndicationItem item)
		{
			return item.ElementExtensions?.Where(see => see.OuterName == "creator")
										  .Select(see => see.GetObject<XElement>()?.Value)
										 ?.ToList();
		}

		/// <returns>Null if there are no Authors</returns>
		private List<string> GetAuthors(SyndicationItem item)
		{
			List<string> result = item.Authors?.Select(a => a.Name)?.ToList();
			return result.Any() ? result : null;
		}
    }
}
