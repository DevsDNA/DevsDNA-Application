namespace DevsDNA.Application.Tests.Features.SocialNetwork
{
    using DevsDNA.Application.Features.SocialNetwork.PostParser;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class PostTextParserTest
    {
        [TestMethod]
        public void ParseTextWithoutHighlights()
        {
            var textWithoutHighlights = "This is a simple dummy text";

            var postSpans = PostTextParser.Parse(textWithoutHighlights);

            Assert.AreEqual(1, postSpans.Count);
            Assert.AreEqual("This is a simple dummy text", postSpans[0].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[0].Type);
        }

        [TestMethod]
        public void ParseTextWithHashtags()
        {
            var textWithHashtags = "This is a #dummy text with #hashtags";

            var postSpans = PostTextParser.Parse(textWithHashtags);

            Assert.AreEqual(4, postSpans.Count);

            Assert.AreEqual("This is a ", postSpans[0].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[0].Type);

            Assert.AreEqual("#dummy", postSpans[1].Text);
            Assert.AreEqual(SpanType.Hashtag, postSpans[1].Type);

            Assert.AreEqual(" text with ", postSpans[2].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[2].Type);

            Assert.AreEqual("#hashtags", postSpans[3].Text);
            Assert.AreEqual(SpanType.Hashtag, postSpans[3].Type);
        }

        [TestMethod]
        public void ParseTextWithUrls()
        {
            var textWithUrls = "This is a dummy text www.devsdna.com with urls https://www.devsdna.com";

            var postSpans = PostTextParser.Parse(textWithUrls);

            Assert.AreEqual(4, postSpans.Count);

            Assert.AreEqual("This is a dummy text ", postSpans[0].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[0].Type);

            Assert.AreEqual("www.devsdna.com", postSpans[1].Text);
            Assert.AreEqual(SpanType.Url, postSpans[1].Type);

            Assert.AreEqual(" with urls ", postSpans[2].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[2].Type);

            Assert.AreEqual("https://www.devsdna.com", postSpans[3].Text);
            Assert.AreEqual(SpanType.Url, postSpans[3].Type);
        }


        [TestMethod]
        public void ParseTextWithUrlsAndHashtags()
        {
            var textWithUrls = "This is a #dummy text www.devsdna.com with urls https://www.devsdna.com and #devsdna hastags";

            var postSpans = PostTextParser.Parse(textWithUrls);

            Assert.AreEqual(9, postSpans.Count);

            Assert.AreEqual("This is a ", postSpans[0].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[0].Type);

            Assert.AreEqual("#dummy", postSpans[1].Text);
            Assert.AreEqual(SpanType.Hashtag, postSpans[1].Type);

            Assert.AreEqual(" text ", postSpans[2].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[2].Type);

            Assert.AreEqual("www.devsdna.com", postSpans[3].Text);
            Assert.AreEqual(SpanType.Url, postSpans[3].Type);

            Assert.AreEqual(" with urls ", postSpans[4].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[4].Type);

            Assert.AreEqual("https://www.devsdna.com", postSpans[5].Text);
            Assert.AreEqual(SpanType.Url, postSpans[5].Type);

            Assert.AreEqual(" and ", postSpans[6].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[6].Type);

            Assert.AreEqual("#devsdna", postSpans[7].Text);
            Assert.AreEqual(SpanType.Hashtag, postSpans[7].Type);

            Assert.AreEqual(" hastags", postSpans[8].Text);
            Assert.AreEqual(SpanType.Plain, postSpans[8].Type);
        }
    }
}
