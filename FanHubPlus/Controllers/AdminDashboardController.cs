using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly FanHubContext _context;

        public AdminDashboardController(FanHubContext context)
        {
            _context = context;
        }

        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCategories = await _context.Categories.CountAsync();
            ViewBag.TotalContents = await _context.Contents.CountAsync();
            ViewBag.TotalBookmarks = await _context.Bookmarks.CountAsync();
            ViewBag.TotalViews = await _context.ContentViews.CountAsync();
            ViewBag.PendingFanSubmissions = await _context.FanSubmissions.CountAsync(s => s.Status == "Pending");
            ViewBag.OpenFeedback = await _context.Feedbacks.CountAsync(f => f.Status == "Open");

            return View();
        }

        // GET: AdminDashboard/Overview
        // Breaks the content library down by fandom category so admins can spot
        // categories that are thin on content, characters or merchandise.
        public async Task<IActionResult> Overview()
        {
            var categories = await _context.Categories
                .Include(c => c.Contents)
                .Include(c => c.CharacterProfiles)
                .Include(c => c.MerchandiseItems)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.TotalMediaItems = await _context.MediaItems.CountAsync();
            ViewBag.TotalEvents = await _context.Events.CountAsync();
            ViewBag.UpcomingEvents = await _context.Events.CountAsync(e => e.EventDate >= DateTime.Now);

            return View(categories);
        }

        // GET: AdminDashboard/Statistics
        // Raw platform totals — covers the "usage statistics" requirement from the SRS.
        public async Task<IActionResult> Statistics()
        {
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();
            ViewBag.TotalContents = await _context.Contents.CountAsync();
            ViewBag.TotalCharacterProfiles = await _context.CharacterProfiles.CountAsync();
            ViewBag.TotalFeaturedArticles = await _context.FeaturedArticles.CountAsync();
            ViewBag.TotalMediaItems = await _context.MediaItems.CountAsync();
            ViewBag.TotalEvents = await _context.Events.CountAsync();
            ViewBag.TotalMerchandiseItems = await _context.MerchandiseItems.CountAsync();
            ViewBag.TotalBookmarks = await _context.Bookmarks.CountAsync();
            ViewBag.TotalChatbotQueries = await _context.ChatbotQueries.CountAsync();

            ViewBag.PendingFanSubmissions = await _context.FanSubmissions.CountAsync(s => s.Status == "Pending");
            ViewBag.ApprovedFanSubmissions = await _context.FanSubmissions.CountAsync(s => s.Status == "Approved");
            ViewBag.RejectedFanSubmissions = await _context.FanSubmissions.CountAsync(s => s.Status == "Rejected");

            ViewBag.OpenFeedback = await _context.Feedbacks.CountAsync(f => f.Status == "Open");
            ViewBag.ResolvedFeedback = await _context.Feedbacks.CountAsync(f => f.Status == "Resolved");

            return View();
        }
    }
}
