namespace DevsDNA.Application.Features.Videos.APIModels
{
    using System.Xml.Serialization;

    [XmlRoot(Namespace = "http://search.yahoo.com/mrss/")]
    public class Thumbnail
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
    }
}