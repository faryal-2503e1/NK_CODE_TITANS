using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Models;

namespace FanHubPlus.Controllers
{
    public class HomeController : Controller
    {
        private readonly FanHubContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FanHubContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Homepage - shows a mix of featured content, upcoming events and trending merchandise
        public async Task<IActionResult> Index()
        {
            var featuredArticles = await _context.FeaturedArticles
                .OrderByDescending(a => a.PublishedAt)
                .Take(5)
                .ToListAsync();

            var upcomingEvents = await _context.Events
                .Where(e => e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .Take(4)
                .ToListAsync();

            var popularContent = await _context.Contents
                .OrderByDescending(c => c.PopularityScore)
                .Take(6)
                .ToListAsync();

            var trendingMerch = await _context.MerchandiseItems
                .Include(m => m.MerchandiseViews)
                .OrderByDescending(m => m.MerchandiseViews.Count)
                .Take(4)
                .ToListAsync();

            // "Trending This Week" - last 7 din ke ContentViews/MerchandiseViews logs se,
            // taake yeh list PopularityScore (all-time) se alag, sirf recent activity dikhaye
            var weekAgo = DateTime.Now.AddDays(-7);

            var trendingContentWeekly = await _context.ContentViews
                .Where(v => v.ViewedAt >= weekAgo)
                .GroupBy(v => v.ContentId)
                .Select(g => new { ContentId = g.Key, ViewCount = g.Count() })
                .OrderByDescending(g => g.ViewCount)
                .Take(5)
                .Join(_context.Contents, g => g.ContentId, c => c.ContentId, (g, c) => new { c.ContentId, c.Title, c.Type, ViewCount = g.ViewCount })
                .ToListAsync();

            var trendingMerchWeekly = await _context.MerchandiseViews
                .Where(v => v.ViewedAt >= weekAgo)
                .GroupBy(v => v.ItemId)
                .Select(g => new { ItemId = g.Key, ViewCount = g.Count() })
                .OrderByDescending(g => g.ViewCount)
                .Take(5)
                .Join(_context.MerchandiseItems, g => g.ItemId, m => m.ItemId, (g, m) => new { m.ItemId, m.Name, m.Tag, ViewCount = g.ViewCount })
                .ToListAsync();

            ViewBag.FeaturedArticles = featuredArticles;
            ViewBag.UpcomingEvents = upcomingEvents;
            ViewBag.TrendingMerchandise = trendingMerch;
            ViewBag.TrendingContentWeekly = trendingContentWeekly;
            ViewBag.TrendingMerchWeekly = trendingMerchWeekly;

            return View(popularContent);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Feedback contactMessage)
        {
            // UserId and Status are set server-side, not bound from the form
            ModelState.Remove(nameof(Feedback.UserId));
            ModelState.Remove(nameof(Feedback.Status));

            if (!ModelState.IsValid)
            {
                return View(contactMessage);
            }

            contactMessage.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Guest";
            contactMessage.Status = "Open";

            _context.Feedbacks.Add(contactMessage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thanks for reaching out, we'll get back to you soon.";
            return RedirectToAction(nameof(Contact));
        }

        [Route("/sitemap")]
        public IActionResult Sitemap()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}