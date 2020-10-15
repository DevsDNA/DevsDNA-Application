namespace DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels
{
    using Newtonsoft.Json;

    public class Post
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_time")]
        public string CreatedTime { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("attachments")]
        public Attachments Attachments { get; set; }

    }
}
