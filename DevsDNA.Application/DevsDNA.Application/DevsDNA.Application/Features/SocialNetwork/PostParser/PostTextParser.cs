namespace DevsDNA.Application.Features.SocialNetwork.PostParser
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class PostTextParser
    {
        private const string urlPattern = @"\b(?:https?://|www\.)\S+\b";
        private const string hashtagPattern = @"(?:#)\w+";
        private static readonly string urlOrHashtagPattern = string.Concat(urlPattern, "|", hashtagPattern);

        public static List<PostSpan> Parse(string text)
        {
            List<PostSpan> postSpans = new List<PostSpan>();

            var lastIndex = 0;
            foreach (Match match in Regex.Matches(text, urlOrHashtagPattern))
            {
                postSpans.Add(new PostSpan(text[lastIndex..match.Index], SpanType.Plain));
                postSpans.Add(new PostSpan(match.Value,match.Value.StartsWith("#")? SpanType.Hashtag: SpanType.Url));
                lastIndex = match.Index + match.Length;
            }

            if(lastIndex < text.Length)
                postSpans.Add(new PostSpan(text.Substring(lastIndex), SpanType.Plain));

            return postSpans;
        }
    }
}
