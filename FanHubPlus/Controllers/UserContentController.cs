using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserContentController : Controller
    {
        private readonly FanHubContext _context;

        public UserContentController(FanHubContext context)
        {
            _context = context;
        }

        [Route("/content")]
        public async Task<IActionResult> Index()
        {
            var content = await _context.Contents
                .Include(c => c.Category)
                .OrderByDescending(c => c.ReleaseDate)
                .Take(20)
                .ToListAsync();

            return View(content);
        }

        [Route("/content/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var content = await _context.Contents
                .Include(c => c.Category)
                .Include(c => c.ContentTags)
                    .ThenInclude(ct => ct.Tag)
                .FirstOrDefaultAsync(c => c.ContentId == id);

            if (content == null)
            {
                return NotFound();
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.IsBookmarked = await _context.Bookmarks
                    .AnyAsync(b => b.UserId == userId && b.ItemType == "Content" && b.ContentId == id);
            }

            return View(content);
        }

        [Route("/content/search")]
        public async Task<IActionResult> Search(string query)
        {
            ViewBag.Query = query;

            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<Content>());
            }

            var results = await _context.Contents
                .Include(c => c.Category)
                .Where(c => c.Title.Contains(query) || c.Description.Contains(query))
                .OrderByDescending(c => c.ReleaseDate)
                .ToListAsync();

            return View(results);
        }

        [Route("/content/filter")]
        public async Task<IActionResult> Filter(int? categoryId, string? type)
        {
            var contentQuery = _context.Contents.Include(c => c.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                contentQuery = contentQuery.Where(c => c.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                contentQuery = contentQuery.Where(c => c.Type == type);
            }

            var filteredContent = await contentQuery
                .OrderByDescending(c => c.ReleaseDate)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedType = type;

            return View(filteredContent);
        }

        [Route("/content/popular")]
        public async Task<IActionResult> Popular()
        {
            var popularContent = await _context.Contents
                .Include(c => c.Category)
                .OrderByDescending(c => c.PopularityScore)
                .Take(20)
                .ToListAsync();

            return View(popularContent);
        }
    }
}