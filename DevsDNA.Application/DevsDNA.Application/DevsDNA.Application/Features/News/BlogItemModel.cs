namespace DevsDNA.Application.Features.News
{
	using System;
	using System.Collections.Generic;

	public class BlogItemModel
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public DateTimeOffset? Created { get; set; }
		public DateTimeOffset? LastUpdate { get; set; }
		public List<string> Authors { get; set; }
		public List<string> Categories { get; set; }
		public List<string> Tags { get; set; }
		public IEnumerable<string> Images { get; set; }
        public string Link { get; set; }
    }
}
