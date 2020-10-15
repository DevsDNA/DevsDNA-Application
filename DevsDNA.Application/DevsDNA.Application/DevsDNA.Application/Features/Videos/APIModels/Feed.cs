namespace DevsDNA.Application.Features.Videos.APIModels
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class Feed
    {
        [XmlElement(ElementName = "entry")]
        public List<Entry> Entries { get; set; }

        public IList<VideoModel> ToVideos()
        {
            if (Entries == null)
                return null;

            List<VideoModel> videos = new List<VideoModel>();
            foreach (var entry in Entries)
            {
                videos.Add(new VideoModel()
                {
                    Id = entry.Id,
                    Title = entry.Title,
                    Description = entry.Media?.Description,                    
                    CreationDate = DateTime.Parse(entry.Published, null, System.Globalization.DateTimeStyles.None),
                    Url = entry.Link?.Url,
                    Image = entry.Media?.Thumbnail?.Url,
                    Width = entry?.Media?.Content?.Width ?? 0,
                    Height = entry?.Media?.Content?.Height ?? 0,
                });
            }

            return videos;
        }
    }
}
