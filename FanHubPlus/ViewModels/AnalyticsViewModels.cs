using FanHubPlus.Helpers;
using FanHubPlus.Models;

namespace FanHubPlus.ViewModels
{
    public class ActiveUserRow
    {
        public string UserName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int Actions { get; set; }
        public DateTime LastActive { get; set; }
    }

    public class CategoryPopularityRow
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ContentCount { get; set; }
        public int MerchandiseCount { get; set; }
        public int ContentViews { get; set; }
        public int MerchandiseViews { get; set; }
        public int Bookmarks { get; set; }
        public int TotalViews => ContentViews + MerchandiseViews;
    }

    // One row per content item or merchandise item in the view-count reports.
    public class ItemViewRow
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public string? Tag { get; set; }
        public int Views { get; set; }
        public int UniqueViewers { get; set; }
        public DateTime? LastViewed { get; set; }
        public int PopularityScore { get; set; }
    }

    public class DailyCount
    {
        public DateTime Day { get; set; }
        public int Count { get; set; }
    }

    public class QueryCount
    {
        public string Message { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ChatbotAnalyticsViewModel
    {
        public int Total { get; set; }
        public int Today { get; set; }
        public int Last7Days { get; set; }
        public int Last30Days { get; set; }
        public int UniqueUsers30Days { get; set; }
        public List<DailyCount> Daily { get; set; } = new();
        public List<QueryCount> TopQuestions { get; set; } = new();
        public PagedList<ChatbotQuery> Recent { get; set; } = default!;
        public Dictionary<string, string> Users { get; set; } = new();
    }
}
