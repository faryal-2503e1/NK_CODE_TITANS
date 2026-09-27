using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserBookmarkController : Controller
    {
        private readonly FanHubContext _context;

        public UserBookmarkController(FanHubContext context)
        {
            _context = context;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [Route("/bookmarks")]
        public async Task<IActionResult> Index()
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == CurrentUserId)
                .Include(b => b.Content)
                .Include(b => b.Character)
                .Include(b => b.MediaItem)
                .Include(b => b.MerchandiseItem)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookmarks);
        }

        [Route("/bookmarks/content")]
        public async Task<IActionResult> Content()
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == CurrentUserId && b.ItemType == "Content")
                .Include(b => b.Content)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookmarks);
        }

        [Route("/bookmarks/characters")]
        public async Task<IActionResult> Characters()
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == CurrentUserId && b.ItemType == "Character")
                .Include(b => b.Character)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookmarks);
        }

        [Route("/bookmarks/media")]
        public async Task<IActionResult> Media()
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == CurrentUserId && b.ItemType == "Media")
                .Include(b => b.MediaItem)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookmarks);
        }

        [Route("/bookmarks/merchandise")]
        public async Task<IActionResult> Merchandise()
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == CurrentUserId && b.ItemType == "Merchandise")
                .Include(b => b.MerchandiseItem)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookmarks);
        }

        // Called from Details pages (Content, Character, Media, Merchandise) via a small form/button
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/bookmarks/add")]
        public async Task<IActionResult> Add(string itemType, int itemId, string returnUrl)
        {
            var alreadyBookmarked = await IsAlreadyBookmarked(itemType, itemId);
            if (alreadyBookmarked)
            {
                TempData["InfoMessage"] = "This is already in your bookmarks.";
                return LocalRedirect(returnUrl ?? Url.Action(nameof(Index)));
            }

            var bookmark = new Bookmark
            {
                UserId = CurrentUserId,
                ItemType = itemType,
                NoteText = string.Empty, // FIXED: Satisfies the NOT NULL database requirement
                CreatedAt = DateTime.Now
            };

            switch (itemType)
            {
                case "Content":
                    bookmark.ContentId = itemId;
                    break;
                case "Character":
                    bookmark.CharacterId = itemId;
                    break;
                case "Media":
                    bookmark.MediaItemId = itemId;
                    break;
                case "Merchandise":
                    bookmark.MerchandiseItemId = itemId;
                    break;
                default:
                    return BadRequest("Unknown bookmark type.");
            }

            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Added to your bookmarks.";
            return LocalRedirect(returnUrl ?? Url.Action(nameof(Index)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/bookmarks/remove/{id:int}")]
        public async Task<IActionResult> Remove(int id, string returnUrl)
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.BookmarkId == id && b.UserId == CurrentUserId);

            if (bookmark == null)
            {
                return NotFound();
            }

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Removed from your bookmarks.";
            return LocalRedirect(returnUrl ?? Url.Action(nameof(Index)));
        }

        private async Task<bool> IsAlreadyBookmarked(string itemType, int itemId)
        {
            return itemType switch
            {
                "Content" => await _context.Bookmarks.AnyAsync(b => b.UserId == CurrentUserId && b.ItemType == "Content" && b.ContentId == itemId),
                "Character" => await _context.Bookmarks.AnyAsync(b => b.UserId == CurrentUserId && b.ItemType == "Character" && b.CharacterId == itemId),
                "Media" => await _context.Bookmarks.AnyAsync(b => b.UserId == CurrentUserId && b.ItemType == "Media" && b.MediaItemId == itemId),
                "Merchandise" => await _context.Bookmarks.AnyAsync(b => b.UserId == CurrentUserId && b.ItemType == "Merchandise" && b.MerchandiseItemId == itemId),
                _ => false
            };
        }
    }
}