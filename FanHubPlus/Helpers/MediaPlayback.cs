using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace FanHubPlus.Helpers
{
    public enum PlayerKind { Unsupported, Embed, Video, Audio, Image }

    public class PlayerInfo
    {
        public PlayerKind Kind { get; init; }
        public string Url { get; init; } = string.Empty;
        public string? MimeType { get; init; }
    }

    // Decides how a stored media link is shown, so every video, audio clip and image
    // plays inside this website (own <video>/<audio>/<img> tags or an embedded player)
    // and the visitor never has to leave the site.
    public static class MediaPlayback
    {
        // Files uploaded by the admin are saved here, under wwwroot
        public const string UploadPrefix = "/uploads/media/";
        public const long MaxUploadBytes = 100L * 1024 * 1024;
        public const long MaxRequestBytes = MaxUploadBytes + 1024 * 1024;

        private static readonly Dictionary<string, string> VideoTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".mp4"] = "video/mp4", [".webm"] = "video/webm", [".ogv"] = "video/ogg"
        };

        private static readonly Dictionary<string, string> AudioTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".mp3"] = "audio/mpeg", [".wav"] = "audio/wav", [".ogg"] = "audio/ogg", [".m4a"] = "audio/mp4"
        };

        private static readonly Dictionary<string, string> ImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = "image/jpeg", [".jpeg"] = "image/jpeg", [".png"] = "image/png",
            [".gif"] = "image/gif", [".webp"] = "image/webp"
        };

        private static readonly Regex YouTubeId = new("^[A-Za-z0-9_-]{11}$", RegexOptions.Compiled);
        private static readonly Regex Digits = new("^[0-9]+$", RegexOptions.Compiled);

        // YouTube / Vimeo pages cannot be played directly, only through their embed player
        public static bool AllowsEmbed(string? mediaType) =>
            mediaType is "Video" or "Trailer" or "Animated Explainer";

        // File extensions the admin may upload (or link to) for each media type
        public static string[] AllowedExtensions(string? mediaType) => mediaType switch
        {
            "Video" or "Trailer" => VideoTypes.Keys.ToArray(),
            "Animated Explainer" => VideoTypes.Keys.Concat(new[] { ".gif", ".webp" }).ToArray(),
            "Audio" => AudioTypes.Keys.ToArray(),
            "Gallery" => ImageTypes.Keys.ToArray(),
            _ => Array.Empty<string>()
        };

        public static string GetExtension(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            var path = Uri.TryCreate(url, UriKind.Absolute, out var uri) ? uri.AbsolutePath : url;
            return Path.GetExtension(path).ToLowerInvariant();
        }

        public static bool IsLocalUpload(string? url) =>
            !string.IsNullOrEmpty(url) && url.StartsWith(UploadPrefix, StringComparison.Ordinal);

        // True when the link can be played inside the site for this media type
        public static bool CanPlay(string? url, string? mediaType)
        {
            if (TryGetEmbedUrl(url, out _)) return AllowsEmbed(mediaType);
            return AllowedExtensions(mediaType).Contains(GetExtension(url));
        }

        public static bool TryGetEmbedUrl(string? url, out string embedUrl)
        {
            embedUrl = string.Empty;
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return false;
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) return false;

            var host = uri.Host.ToLowerInvariant();
            if (host.StartsWith("www.")) host = host.Substring(4);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            string? youTubeId = null;
            if (host == "youtu.be" && segments.Length >= 1)
            {
                youTubeId = segments[0];
            }
            else if (host is "youtube.com" or "m.youtube.com" or "youtube-nocookie.com")
            {
                if (segments.Length >= 1 && segments[0] == "watch")
                {
                    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("v", out var v)) youTubeId = v.ToString();
                }
                else if (segments.Length >= 2 && segments[0] is "embed" or "shorts" or "live")
                {
                    youTubeId = segments[1];
                }
            }

            if (youTubeId != null && YouTubeId.IsMatch(youTubeId))
            {
                // youtube-nocookie.com is YouTube's privacy-enhanced embed domain
                embedUrl = $"https://www.youtube-nocookie.com/embed/{youTubeId}";
                return true;
            }

            string? vimeoId = null;
            if (host == "vimeo.com" && segments.Length >= 1) vimeoId = segments[^1];
            else if (host == "player.vimeo.com" && segments.Length >= 2 && segments[0] == "video") vimeoId = segments[1];

            if (vimeoId != null && Digits.IsMatch(vimeoId))
            {
                embedUrl = $"https://player.vimeo.com/video/{vimeoId}";
                return true;
            }

            return false;
        }

        public static PlayerInfo Resolve(string? fileUrl, string? mediaType)
        {
            var unsupported = new PlayerInfo { Kind = PlayerKind.Unsupported };
            if (string.IsNullOrWhiteSpace(fileUrl)) return unsupported;

            var isWebLink = Uri.TryCreate(fileUrl, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            if (!isWebLink && !IsLocalUpload(fileUrl)) return unsupported;

            if (TryGetEmbedUrl(fileUrl, out var embed)) return new PlayerInfo { Kind = PlayerKind.Embed, Url = embed };

            var ext = GetExtension(fileUrl);
            if (VideoTypes.TryGetValue(ext, out var video)) return new PlayerInfo { Kind = PlayerKind.Video, Url = fileUrl, MimeType = video };
            if (AudioTypes.TryGetValue(ext, out var audio)) return new PlayerInfo { Kind = PlayerKind.Audio, Url = fileUrl, MimeType = audio };
            if (ImageTypes.TryGetValue(ext, out var image)) return new PlayerInfo { Kind = PlayerKind.Image, Url = fileUrl, MimeType = image };

            return unsupported;
        }
    }
}
