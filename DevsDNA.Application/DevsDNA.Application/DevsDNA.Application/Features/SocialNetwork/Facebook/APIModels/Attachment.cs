namespace DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels
{
    using Newtonsoft.Json;

    public class Attachment
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }
               
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
