namespace DevsDNA.Application.Features.Videos.APIModels
{
    using System.Xml.Serialization;

    [XmlRoot(Namespace = "http://search.yahoo.com/mrss/")]
    public class Media
    {
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        [XmlElement(ElementName = "content")]
        public Content Content { get; set; }
    }
}
