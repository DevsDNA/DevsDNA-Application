namespace DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Attachments
    {
        [JsonProperty("data")]
        public IEnumerable<Attachment> Data { get; set; }
    }
}
