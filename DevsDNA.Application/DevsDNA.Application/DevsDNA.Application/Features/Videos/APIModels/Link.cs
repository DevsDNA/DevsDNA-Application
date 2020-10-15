namespace DevsDNA.Application.Features.Videos.APIModels
{
    using System.Xml.Serialization;

    [XmlRoot(Namespace = "http://www.w3.org/2005/Atom")]
    public class Link
    {
        [XmlAttribute(AttributeName = "href")]
        public string Url { get; set; }
    }
}
