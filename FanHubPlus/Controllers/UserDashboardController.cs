using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserDashboardController : Controller
    {
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDashboardController(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string CurrentUserId => _userManager.GetUserId(User) ?? string.Empty;

        [Route("/dashboard")]
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;

            ViewBag.BookmarkCount = await _context.Bookmarks.CountAsync(b => b.UserId == userId);
            ViewBag.NoteCount = await _context.Notes.CountAsync(n => n.UserId == userId);
            ViewBag.FavoriteCategoryCount = await _context.UserFavoriteCategories.CountAsync(f => f.UserId == userId);

            var recentActivity = await _context.UserActivities
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Timestamp)
                .Take(5)
                .ToListAsync();

            var recentNotes = await _context.Notes
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(3)
                .ToListAsync();

            ViewBag.RecentActivity = recentActivity;
            ViewBag.RecentNotes = recentNotes;

            return View();
        }

        [Route("/dashboard/favorites")]
        public async Task<IActionResult> Favorites()
        {
            var favoriteCategories = await _context.UserFavoriteCategories
                .Where(f => f.UserId == CurrentUserId)
                .Include(f => f.Category)
                .ToListAsync();

            return View(favoriteCategories);
        }

        [Route("/dashboard/bookmarks")]
        public async Task<IActionResult> Bookmarks()
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == CurrentUserId)
                .Include(b => b.Content)
                .Include(b => b.Character)
                .Include(b => b.MediaItem)
                .Include(b => b.MerchandiseItem)
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .ToListAsync();

            return View(bookmarks);
        }

        [Route("/dashboard/activity")]
        public async Task<IActionResult> RecentActivity()
        {
            var activities = await _context.UserActivities
                .Where(a => a.UserId == CurrentUserId)
                .OrderByDescending(a => a.Timestamp)
                .Take(50)
                .ToListAsync();

            return View(activities);
        }

        [Route("/dashboard/notes")]
        public async Task<IActionResult> Notes()
        {
            var notes = await _context.Notes
                .Where(n => n.UserId == CurrentUserId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notes);
        }
    }
}