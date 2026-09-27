namespace FanHubPlus.Helpers
{
    // Fixed value lists used by the admin dropdowns and filters.
    public static class AdminOptions
    {
        public static readonly string[] ContentTypes = { "article", "video", "audio", "image" };
        public static readonly string[] MediaTypes = { "Video", "Audio", "Trailer", "Gallery", "Animated Explainer" };
        public static readonly string[] MerchandiseTags = { "Limited Edition", "Pre-Order", "Collectible" };
        public static readonly string[] FeedbackTypes = { "Bug", "Suggestion", "Query" };
        public static readonly string[] FeedbackStatuses = { "Open", "In Progress", "Resolved", "Closed" };
        public static readonly string[] SubmissionStatuses = { "Pending", "Approved", "Rejected" };

        // Keeps a value that is already stored (older data) selectable even if it is not in the fixed list.
        public static IEnumerable<string> WithCurrent(IEnumerable<string> options, string? current)
        {
            return string.IsNullOrWhiteSpace(current) || options.Contains(current, StringComparer.OrdinalIgnoreCase)
                ? options
                : options.Append(current);
        }
    }
}
