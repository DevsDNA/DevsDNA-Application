namespace DevsDNA.Application.Features.Videos.APIModels
{
    using System.Xml.Serialization;

    [XmlRoot(Namespace = "http://search.yahoo.com/mrss/")]
    public class Content
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }

        [XmlAttribute(AttributeName = "width")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = "height")]
        public int Height { get; set; }
    }
}