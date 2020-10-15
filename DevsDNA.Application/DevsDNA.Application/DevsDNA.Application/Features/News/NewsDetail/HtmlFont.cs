namespace DevsDNA.Application.Features.News.NewsDetail
{
    using Xamarin.Forms;

    public class HtmlFont
    {
        public HtmlFont(string fontPath, Color color)
        {
            Path = fontPath;
            Color = color.ToHex().Remove(1, 2);
        }

        public string Path { get; }
        public string Color { get; }
    }
}
