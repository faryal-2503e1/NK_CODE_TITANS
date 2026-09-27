using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;
using FanHubPlus.Models;
using FanHubPlus.ViewModels;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAnalyticsController : Controller
    {
        private const int PageSize = 10;
        private static readonly int[] ActiveWindows = { 7, 30, 90 };

        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminAnalyticsController(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminAnalytics
        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;

            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.ActiveUsers = (await GetActivityAsync(now.AddDays(-30))).Count;
            ViewBag.TotalContentViews = await _context.ContentViews.CountAsync();
            ViewBag.TotalMerchViews = await _context.MerchandiseViews.CountAsync();
            ViewBag.TotalChatbotQueries = await _context.ChatbotQueries.CountAsync();
            ViewBag.ChatbotLast7Days = await _context.ChatbotQueries.CountAsync(c => c.CreatedAt >= now.AddDays(-7));
            ViewBag.TopCategories = (await GetCategoryRowsAsync(null)).Take(3).ToList();

            return View();
        }

        // GET: AdminAnalytics/ActiveUsers
        // "Active" means the user did something in the chosen window: viewed content or
        // merchandise, bookmarked, asked the chatbot, or has a logged activity.
        public async Task<IActionResult> ActiveUsers(int days = 30, string? q = null, int page = 1)
        {
            if (!ActiveWindows.Contains(days)) days = 30;

            var activity = await GetActivityAsync(DateTime.Now.AddDays(-days));

            var wanted = activity.Keys.ToList();
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                var activeIds = wanted;
                wanted = await _userManager.Users
                    .Where(u => activeIds.Contains(u.Id) &&
                        ((u.UserName != null && u.UserName.Contains(q)) ||
                         (u.Email != null && u.Email.Contains(q)) ||
                         (u.FullName != null && u.FullName.Contains(q))))
                    .Select(u => u.Id)
                    .ToListAsync();
            }

            var ordered = wanted
                .OrderByDescending(id => activity[id].Last)
                .ToList();

            var paged = PagedList<string>.Create(ordered, page, PageSize);
            var pageIds = paged.ToList();

            var users = await _userManager.Users
                .Where(u => pageIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var rows = pageIds
                .Where(users.ContainsKey)
                .Select(id => new ActiveUserRow
                {
                    UserName = users[id].UserName ?? users[id].Email ?? id,
                    FullName = users[id].FullName,
                    Email = users[id].Email,
                    Actions = activity[id].Count,
                    LastActive = activity[id].Last
                })
                .ToList();

            ViewBag.Days = days;
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            return View(new PagedList<ActiveUserRow>(rows, paged.TotalCount, paged.Page, paged.PageSize));
        }

        // GET: AdminAnalytics/PopularCategories
        public async Task<IActionResult> PopularCategories(int days = 0)
        {
            DateTime? since = days > 0 ? DateTime.Now.AddDays(-days) : null;
            ViewBag.Days = days;
            return View(await GetCategoryRowsAsync(since));
        }

        // GET: AdminAnalytics/ContentViews
        // One row per content item with its total views, so the admin can see what is popular.
        public async Task<IActionResult> ContentViews(string? q, string? sort, int page = 1)
        {
            var query = _context.Contents.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(c => c.Title.Contains(q));
            }

            var rows = query.Select(c => new ItemViewRow
            {
                Id = c.ContentId,
                Title = c.Title,
                CategoryName = c.Category.Name,
                Views = c.ContentViews.Count(),
                UniqueViewers = c.ContentViews.Select(v => v.UserId).Distinct().Count(),
                LastViewed = c.ContentViews.Max(v => (DateTime?)v.ViewedAt),
                PopularityScore = c.PopularityScore
            });

            rows = sort switch
            {
                "score" => rows.OrderByDescending(r => r.PopularityScore).ThenBy(r => r.Id),
                "title" => rows.OrderBy(r => r.Title).ThenBy(r => r.Id),
                "recent" => rows.OrderByDescending(r => r.LastViewed).ThenBy(r => r.Id),
                _ => rows.OrderByDescending(r => r.Views).ThenBy(r => r.Id)
            };

            return View(await PagedList<ItemViewRow>.CreateAsync(rows, page, PageSize));
        }

        // GET: AdminAnalytics/ContentViewLog?contentId=5
        public async Task<IActionResult> ContentViewLog(int? contentId, int page = 1)
        {
            var query = _context.ContentViews.Include(v => v.Content).AsQueryable();
            if (contentId != null) query = query.Where(v => v.ContentId == contentId);

            var views = await PagedList<ContentViews>.CreateAsync(
                query.OrderByDescending(v => v.ViewedAt).ThenBy(v => v.ViewId), page, PageSize);

            ViewBag.Users = await _userManager.GetUserNamesAsync(views.Select(v => v.UserId));
            ViewBag.ItemTitle = contentId == null ? null
                : await _context.Contents.Where(c => c.ContentId == contentId).Select(c => c.Title).FirstOrDefaultAsync();
            return View(views);
        }

        // POST: AdminAnalytics/RecalculatePopularity
        // Popularity score = number of views + 2 x number of bookmarks. It is a stored number,
        // so the admin refreshes it here and the "most popular" sort uses it.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecalculatePopularity()
        {
            var views = await _context.ContentViews
                .GroupBy(v => v.ContentId)
                .Select(g => new { ContentId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ContentId, x => x.Count);

            var bookmarks = await _context.Bookmarks
                .GroupBy(b => b.ContentId)
                .Select(g => new { ContentId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ContentId, x => x.Count);

            var contents = await _context.Contents.ToListAsync();
            foreach (var content in contents)
            {
                views.TryGetValue(content.ContentId, out var v);
                bookmarks.TryGetValue(content.ContentId, out var b);
                content.PopularityScore = v + 2 * b;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Popularity scores refreshed for {contents.Count} content item(s).";
            return RedirectToAction(nameof(ContentViews), new { sort = "score" });
        }

        // GET: AdminAnalytics/MerchandiseViews
        public async Task<IActionResult> MerchandiseViews(string? q, string? sort, int page = 1)
        {
            var query = _context.MerchandiseItems.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(m => m.Name.Contains(q));
            }

            var rows = query.Select(m => new ItemViewRow
            {
                Id = m.ItemId,
                Title = m.Name,
                CategoryName = m.Category.Name,
                Tag = m.Tag,
                Views = m.MerchandiseViews.Count(),
                UniqueViewers = m.MerchandiseViews.Select(v => v.UserId).Distinct().Count(),
                LastViewed = m.MerchandiseViews.Max(v => (DateTime?)v.ViewedAt)
            });

            rows = sort switch
            {
                "title" => rows.OrderBy(r => r.Title).ThenBy(r => r.Id),
                "recent" => rows.OrderByDescending(r => r.LastViewed).ThenBy(r => r.Id),
                _ => rows.OrderByDescending(r => r.Views).ThenBy(r => r.Id)
            };

            return View(await PagedList<ItemViewRow>.CreateAsync(rows, page, PageSize));
        }

        // GET: AdminAnalytics/MerchandiseViewLog?itemId=5
        public async Task<IActionResult> MerchandiseViewLog(int? itemId, int page = 1)
        {
            var query = _context.MerchandiseViews.Include(v => v.MerchandiseItem).AsQueryable();
            if (itemId != null) query = query.Where(v => v.ItemId == itemId);

            var views = await PagedList<MerchandiseViews>.CreateAsync(
                query.OrderByDescending(v => v.ViewedAt).ThenBy(v => v.ViewId), page, PageSize);

            ViewBag.Users = await _userManager.GetUserNamesAsync(views.Select(v => v.UserId));
            ViewBag.ItemTitle = itemId == null ? null
                : await _context.MerchandiseItems.Where(m => m.ItemId == itemId).Select(m => m.Name).FirstOrDefaultAsync();
            return View(views);
        }

        // GET: AdminAnalytics/UserActivity
        public async Task<IActionResult> UserActivity(string? q, int page = 1)
        {
            var query = _context.UserActivities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(a => a.ActionDescription.Contains(q));
            }

            var activities = await PagedList<UserActivity>.CreateAsync(
                query.OrderByDescending(a => a.Timestamp).ThenBy(a => a.ActivityId), page, PageSize);

            ViewBag.Users = await _userManager.GetUserNamesAsync(activities.Select(a => a.UserId));
            return View(activities);
        }

        // GET: AdminAnalytics/ChatbotAnalytics
        public async Task<IActionResult> ChatbotAnalytics(string? q, int page = 1)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var since30 = now.AddDays(-30);
            var since14 = today.AddDays(-13);

            var recentQuery = _context.ChatbotQueries.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                recentQuery = recentQuery.Where(c => c.Message.Contains(q) || c.Response.Contains(q));
            }

            var recent = await PagedList<ChatbotQuery>.CreateAsync(
                recentQuery.OrderByDescending(c => c.CreatedAt).ThenBy(c => c.QueryId), page, PageSize);

            // Per-day volume for the last 14 days (days with no queries are filled in with 0)
            var perDay = await _context.ChatbotQueries
                .Where(c => c.CreatedAt >= since14)
                .GroupBy(c => c.CreatedAt.Date)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .ToListAsync();

            var daily = Enumerable.Range(0, 14)
                .Select(i => since14.AddDays(i))
                .Select(day => new DailyCount { Day = day, Count = perDay.FirstOrDefault(p => p.Day == day)?.Count ?? 0 })
                .ToList();

            var model = new ChatbotAnalyticsViewModel
            {
                Total = await _context.ChatbotQueries.CountAsync(),
                Today = await _context.ChatbotQueries.CountAsync(c => c.CreatedAt >= today),
                Last7Days = await _context.ChatbotQueries.CountAsync(c => c.CreatedAt >= today.AddDays(-6)),
                Last30Days = await _context.ChatbotQueries.CountAsync(c => c.CreatedAt >= since30),
                UniqueUsers30Days = await _context.ChatbotQueries
                    .Where(c => c.CreatedAt >= since30)
                    .Select(c => c.UserId).Distinct().CountAsync(),
                Daily = daily,
                TopQuestions = await _context.ChatbotQueries
                    .GroupBy(c => c.Message)
                    .Select(g => new QueryCount { Message = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count).ThenBy(x => x.Message)
                    .Take(5)
                    .ToListAsync(),
                Recent = recent,
                Users = await _userManager.GetUserNamesAsync(recent.Select(c => c.UserId))
            };

            return View(model);
        }

        // Per-category totals. Views are counted in the database, so nothing is loaded into memory but the 8 result rows.
        private async Task<List<CategoryPopularityRow>> GetCategoryRowsAsync(DateTime? since)
        {
            var rows = await _context.Categories
                .Select(c => new CategoryPopularityRow
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ContentCount = c.Contents.Count(),
                    MerchandiseCount = c.MerchandiseItems.Count(),
                    ContentViews = _context.ContentViews.Count(v =>
                        v.Content.CategoryId == c.CategoryId && (since == null || v.ViewedAt >= since)),
                    MerchandiseViews = _context.MerchandiseViews.Count(v =>
                        v.MerchandiseItem.CategoryId == c.CategoryId && (since == null || v.ViewedAt >= since)),
                    Bookmarks = _context.Bookmarks.Count(b =>
                        b.Content.CategoryId == c.CategoryId && (since == null || b.CreatedAt >= since))
                })
                .ToListAsync();

            return rows.OrderByDescending(r => r.TotalViews).ThenByDescending(r => r.Bookmarks).ThenBy(r => r.Name).ToList();
        }

        // Per-user action count and last action time since the given date, merged from every place a user leaves a trace.
        private async Task<Dictionary<string, (int Count, DateTime Last)>> GetActivityAsync(DateTime since)
        {
            var result = new Dictionary<string, (int Count, DateTime Last)>();

            void Merge(IEnumerable<(string UserId, int Count, DateTime Last)> rows)
            {
                foreach (var (userId, count, last) in rows)
                {
                    if (string.IsNullOrEmpty(userId)) continue;
                    result[userId] = result.TryGetValue(userId, out var cur)
                        ? (cur.Count + count, cur.Last > last ? cur.Last : last)
                        : (count, last);
                }
            }

            Merge((await _context.UserActivities.Where(a => a.Timestamp >= since)
                .GroupBy(a => a.UserId).Select(g => new { g.Key, C = g.Count(), L = g.Max(a => a.Timestamp) }).ToListAsync())
                .Select(x => (x.Key, x.C, x.L)));

            Merge((await _context.ContentViews.Where(v => v.ViewedAt >= since)
                .GroupBy(v => v.UserId).Select(g => new { g.Key, C = g.Count(), L = g.Max(v => v.ViewedAt) }).ToListAsync())
                .Select(x => (x.Key, x.C, x.L)));

            Merge((await _context.MerchandiseViews.Where(v => v.ViewedAt >= since)
                .GroupBy(v => v.UserId).Select(g => new { g.Key, C = g.Count(), L = g.Max(v => v.ViewedAt) }).ToListAsync())
                .Select(x => (x.Key, x.C, x.L)));

            Merge((await _context.ChatbotQueries.Where(c => c.CreatedAt >= since)
                .GroupBy(c => c.UserId).Select(g => new { g.Key, C = g.Count(), L = g.Max(c => c.CreatedAt) }).ToListAsync())
                .Select(x => (x.Key, x.C, x.L)));

            Merge((await _context.Bookmarks.Where(b => b.CreatedAt >= since)
                .GroupBy(b => b.UserId).Select(g => new { g.Key, C = g.Count(), L = g.Max(b => b.CreatedAt) }).ToListAsync())
                .Select(x => (x.Key, x.C, x.L)));

            return result;
        }
    }
}
