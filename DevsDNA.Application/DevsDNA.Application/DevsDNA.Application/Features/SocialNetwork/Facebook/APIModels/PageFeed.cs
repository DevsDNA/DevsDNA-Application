namespace DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class PageFeed
    {
        [JsonProperty("data")]
        public IEnumerable<Post> Posts { get; set; }

        public IList<PostModel> ToPosts()
        {
            if (Posts == null)
                return null;

            List<PostModel> posts = new List<PostModel>();
            foreach (var post in Posts.Where(p => !string.IsNullOrEmpty(p.Message)))
            {
                posts.Add(new PostModel()
                {
                    Id = post.Id,
                    Message = post.Message,
                    CreationDate = DateTime.Parse(post.CreatedTime, null, System.Globalization.DateTimeStyles.None),
                    Attachments = post.Attachments?.Data?.Where(d => d.Type == "photo" || d.Type == "share")
                                    .Select(d => new SocialNetwork.Attachment()
                                    {
                                        Description = d.Description,
                                        Image = d.Media?.Image?.Src,
                                        Title = d.Title,
                                        Type = d.Type == "photo" ? AttachmentType.Image : AttachmentType.Share,
                                        Url = GetUrl(d)
                                    })?.ToList()
                });

            }

            return posts;
        }

        private string GetUrl(Attachment attachment)
        {
            if (attachment.Type == "share" && Uri.TryCreate(attachment.Url, UriKind.Absolute, out Uri uri))
            {
                return HttpUtility.ParseQueryString(uri.Query).Get("u");
            }
            return attachment.Url;
        }
    }
}
