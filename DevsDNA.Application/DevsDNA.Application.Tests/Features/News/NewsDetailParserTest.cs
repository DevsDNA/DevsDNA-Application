namespace DevsDNA.Application.Tests.Features.News
{
    using DevsDNA.Application.Features.News.NewsDetail;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NewsDetailParserTest
    {
        [TestMethod]
        public void AddAbsolutePathToRelativeImages()
        {
            var newsDetailParser = GivenANewsDetailParser("<img src=\"/image.png\" /> <img src=\"/other_image.png\" />", "https://www.dummy.com");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "src=\"https://www.dummy.com/image.png\"");
            StringAssert.Contains(result, "src=\"https://www.dummy.com/other_image.png\"");
        }

        [TestMethod]
        public void RemoveSrcsetAttributeInRelativeImages()
        {
            var newsDetailParser = GivenANewsDetailParser("<img src=\"/image.png\" srcset=\"/image-1024.png 1024w, /image-1536.png 1536w, /image-2016.png 2016w\" />");

            string result = newsDetailParser.BeautifyHtml();          

            Assert.IsFalse(result.Contains("srcset"));
        }

        [TestMethod]
        public void AddRelativeImageStyleToRelativeImages()
        {
            var newsDetailParser = GivenANewsDetailParser("<img src=\"/image.png\" />");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "style=\"" + newsDetailParser.RelativeImageStyle + "\"");
        }

        [TestMethod]
        public void NoReplaceImageStyleWhenAddRelativeImageStyleToRelativeImages()
        {
            var newsDetailParser = GivenANewsDetailParser("<img src=\"/image.png\" style=\"height:1em;\" />");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "style=\"height:1em;" + newsDetailParser.RelativeImageStyle + "\"");
        }

        [TestMethod]
        public void NoModifyImageAttributesIfPathIsAbsolute()
        {
            var newsDetailParser = GivenANewsDetailParser("<img src=\"https://www.dummy.com/image.png\" style=\"height:1em;\" />", "https://www.dummy.com");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "<img src=\"https://www.dummy.com/image.png\" style=\"height:1em;\" />");
        }

        [TestMethod]
        public void AddIframeStyleToIframe()
        {
            var newsDetailParser = GivenANewsDetailParser("<iframe />");

            string result = newsDetailParser.BeautifyHtml();           

            StringAssert.Contains(result, "style=\"" + newsDetailParser.IframeStyle + "\"");
        }

        [TestMethod]
        public void NoReplaceIframeStyleWhenAddIframeStyleToIframe()
        {
            var newsDetailParser = GivenANewsDetailParser("<iframe style=\"height:1em;\" />");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "style=\"height:1em;" + newsDetailParser.IframeStyle + "\"");
        }

        [TestMethod]
        public void AddWrapDivWithStyleToIframe()
        {
            var newsDetailParser = GivenANewsDetailParser("<iframe />");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "<div style=\"" + newsDetailParser.WrapDivIframeStyle + "\"");
        }

        [TestMethod]
        public void AddStyleToHtml()
        {
            var newsDetailParser = GivenANewsDetailParser("<div />");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "style");
        }

        [TestMethod]
        public void AddViewportToHtml()
        {
            var newsDetailParser = GivenANewsDetailParser("<div />");

            string result = newsDetailParser.BeautifyHtml();

            StringAssert.Contains(result, "<meta name=\"viewport\"");
        }


        private NewsDetailParser GivenANewsDetailParser(string html, string domain = null)
        {
            return new NewsDetailParser(new NewsHtmlSettings() { Domain = domain, Html = html });
        }
    }
}
