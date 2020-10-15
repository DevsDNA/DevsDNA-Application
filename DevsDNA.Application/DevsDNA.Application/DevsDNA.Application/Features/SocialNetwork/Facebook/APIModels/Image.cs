namespace DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels
{
    using Newtonsoft.Json;

    public class Image
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }
     
    }
}
