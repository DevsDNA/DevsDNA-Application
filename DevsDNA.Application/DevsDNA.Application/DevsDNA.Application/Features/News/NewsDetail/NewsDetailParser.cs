namespace DevsDNA.Application.Features.News.NewsDetail
{
    using HtmlAgilityPack;
    using System;

    public class NewsDetailParser
    {
        private readonly HtmlDocument htmlDocument;
        private readonly NewsHtmlSettings newsHtmlSettings;

        public NewsDetailParser(NewsHtmlSettings newsHtmlSettings)
        {
            htmlDocument = new HtmlDocument() { OptionWriteEmptyNodes = true };
            this.newsHtmlSettings = newsHtmlSettings;
        }

        public string RelativeImageStyle { get; set; } = "display:block;margin-left:auto;margin-right:auto;width:100%";
        public string IframeStyle { get; set; } = "width:100%;height:100%;position:absolute;left:0px;top:0px;";
        public string WrapDivIframeStyle { get; set; } = "position:relative;padding-bottom:56.25%;";


        public string BeautifyHtml()
        {
            htmlDocument.LoadHtml(newsHtmlSettings.Html);

            AddViewport();
            AddFontStyles();
            ManageImageTags();
            ManageIframeTags();

            return htmlDocument.DocumentNode.OuterHtml;
        }

        private void ManageImageTags()
        {
            var imgNodes = htmlDocument.DocumentNode.SelectNodes("//img");
            if (imgNodes == null)
                return;

            foreach (var imgNode in imgNodes)
            {
                var src = imgNode.GetAttributeValue("src", null);
                if (Uri.IsWellFormedUriString(src, UriKind.Relative))
                {
                    if (src != null && src.StartsWith("/"))
                        imgNode.SetAttributeValue("src", newsHtmlSettings.Domain + src);

                    imgNode.Attributes.Remove("srcset");

                    var imgStyle = imgNode.GetAttributeValue("style", null);
                    imgNode.SetAttributeValue("style", string.Concat(imgStyle, RelativeImageStyle));
                }
            }
        }

        private void ManageIframeTags()
        {
            var iframeNodes = htmlDocument.DocumentNode.SelectNodes("//iframe");
            if (iframeNodes == null)
                return;

            foreach (var iframeNode in iframeNodes)
            {
                HtmlNode parentIframeNode = iframeNode.ParentNode;
                HtmlDocument wrapDoc = new HtmlDocument();
                HtmlNode wrapDiv = wrapDoc.CreateElement("div");
                wrapDiv.SetAttributeValue("style", WrapDivIframeStyle);
                wrapDiv.AppendChild(iframeNode);
                parentIframeNode.ReplaceChild(wrapDiv, iframeNode);

                var iframeStyle = iframeNode.GetAttributeValue("style", null);
                iframeNode.SetAttributeValue("style", string.Concat(iframeStyle, IframeStyle));
            }
        }

        private void AddFontStyles()
        {
            var style = htmlDocument.CreateElement("style");
            var fontStyle = htmlDocument.CreateTextNode($"@font-face {{  font-family: Regular;  src: url({newsHtmlSettings.RegularFont?.Path});}}" +
                                                        $"@font-face {{  font-family: HeadingFont;  src: url({newsHtmlSettings.HeadingFont?.Path});}}" +
                                                        $"body {{  font-family: Regular; color: {newsHtmlSettings.RegularFont?.Color}}}" +
                                                        $"h1,h2,h3,h4,h5,h6 {{ font-family: HeadingFont; color: {newsHtmlSettings.HeadingFont?.Color}}}");
            style.AppendChild(fontStyle);
            htmlDocument.DocumentNode.AppendChild(style);
        }

        private void AddViewport()
        {
            var meta = htmlDocument.CreateElement("meta");
            meta.SetAttributeValue("name", "viewport");
            meta.SetAttributeValue("content", "width=device-width; initial-scale=1.0;");            
            htmlDocument.DocumentNode.AppendChild(meta);
        }
    }
}
