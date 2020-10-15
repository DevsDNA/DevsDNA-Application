namespace DevsDNA.Application.Features.SocialNetwork
{
    using System;
    using System.Collections.Generic;

    public class PostModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public IList<Attachment> Attachments { get; set; }
    }
}
