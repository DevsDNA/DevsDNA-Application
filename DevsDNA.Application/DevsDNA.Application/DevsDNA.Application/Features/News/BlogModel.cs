namespace DevsDNA.Application.Features.News
{
	using System;
	using System.Collections.Generic;

	public class BlogModel
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public List<BlogItemModel> Posts { get; set; }
		public string Description { get; set; }
		public DateTimeOffset LastUpdate { get; set; }
	}
}
