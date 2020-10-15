namespace DevsDNA.Application.Features.Videos
{
    using System;

    public class VideoModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
