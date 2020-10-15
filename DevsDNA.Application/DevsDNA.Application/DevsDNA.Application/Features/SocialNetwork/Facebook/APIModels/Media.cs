namespace DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels
{
    using Newtonsoft.Json;

    public class Media
    {
        [JsonProperty("image")]
        public Image Image { get; set; }
    }
}
