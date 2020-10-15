namespace DevsDNA.Application.Extensions
{
    using System.Text.RegularExpressions;

    public static class ExtensionMethodsString
    {
        public static string GetPathFromFirstImage(this string html)
        {
            Match match = Regex.Match(html, "<img.+?src=[\"'](.+?)[\"'].*?>");
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[1].Value;

            return null;
        }
    }
}
