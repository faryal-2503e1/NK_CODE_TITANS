using System.Net;
using System.Text.RegularExpressions;
using Ganss.Xss;

namespace FanHubPlus.Helpers
{
    public static class RichText
    {
        private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();
        private static readonly object Gate = new object();

        // Article bodies are HTML written in the admin editor, so strip anything executable before storing or showing it.
        public static string Sanitize(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;
            lock (Gate)
            {
                return Sanitizer.Sanitize(html);
            }
        }

        // Plain-text preview for list pages.
        public static string ToPlainText(string? html, int maxLength = 100)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;

            var text = Regex.Replace(html, "<[^>]*>", " ");
            text = WebUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text.Length > maxLength ? text.Substring(0, maxLength - 3) + "..." : text;
        }
    }
}
