namespace DevsDNA.Application.Features.Videos.APIModels
{
	using System.Xml.Serialization;

	[XmlRoot(Namespace = "http://www.w3.org/2005/Atom")]
	public class Entry
    {
		[XmlElement(ElementName = "videoId", Namespace = "http://www.youtube.com/xml/schemas/2015")]
		public string Id { get; set; }

		[XmlElement(ElementName = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "link")]
		public Link Link { get; set; }

		[XmlElement(ElementName = "published")]
		public string Published { get; set; }

		[XmlElement(ElementName = "group", Namespace = "http://search.yahoo.com/mrss/")]
		public Media Media { get; set; }

		
	}
}
