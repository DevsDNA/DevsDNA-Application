namespace DevsDNA.Application.Features.News.NewsDetail
{   
    using Xamarin.Forms;

    public class WebviewNews : WebView
    {
        public static BindableProperty NewsHtmlSettingsProperty = BindableProperty.Create(nameof(NewsHtmlSettings), typeof(NewsHtmlSettings), typeof(WebviewNews), null, propertyChanged: OnNewsHtmlSettingsChanged);

        public NewsHtmlSettings NewsHtmlSettings
        {
            get { return (NewsHtmlSettings)GetValue(NewsHtmlSettingsProperty); }
            set { SetValue(NewsHtmlSettingsProperty, value); }
        }

        private static void OnNewsHtmlSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WebviewNews webviewNews && newValue is NewsHtmlSettings newsHtmlSettings)
            {
                var newsDetailParser = new NewsDetailParser(newsHtmlSettings);
                HtmlWebViewSource htmlWebViewSource = new HtmlWebViewSource()
                {
                    Html = newsDetailParser.BeautifyHtml()
                };

                webviewNews.Source = htmlWebViewSource;
            }
        }
    }
}
