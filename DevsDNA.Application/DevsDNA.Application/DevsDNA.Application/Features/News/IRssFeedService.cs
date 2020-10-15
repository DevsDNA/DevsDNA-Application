namespace DevsDNA.Application.Features.News
{	
	using System;

	public interface IRssFeedService
	{
		IObservable<BlogModel> GetBlogModel();
	}
}