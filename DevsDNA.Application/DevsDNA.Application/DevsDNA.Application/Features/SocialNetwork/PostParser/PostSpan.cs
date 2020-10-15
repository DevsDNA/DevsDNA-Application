namespace DevsDNA.Application.Features.SocialNetwork.PostParser
{
    public class PostSpan
    {
        public PostSpan(string text, SpanType type)
        {
            Text = text;
            Type = type;
        }

        public string Text { get; }
        public SpanType Type { get; }
    }
}
