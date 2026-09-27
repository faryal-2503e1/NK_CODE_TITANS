using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserMediaController : Controller
    {
        private readonly FanHubContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserMediaController(FanHubContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: UserMedia/Index
        public async Task<IActionResult> Index()
        {
            var allMedia = await _context.MediaItems
                .Include(m => m.Category)
                .Include(m => m.MediaTags).ThenInclude(mt => mt.Tag)
                .Include(m => m.MediaRatings)
                .OrderByDescending(m => m.UploadedAt)
                .ToListAsync();

            return View(allMedia);
        }

        // GET: UserMedia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var mediaItem = await _context.MediaItems
                .Include(m => m.Category)
                .Include(m => m.MediaTags).ThenInclude(mt => mt.Tag)
                .Include(m => m.MediaRatings)
                .FirstOrDefaultAsync(m => m.MediaId == id);

            if (mediaItem == null) return NotFound();

            ViewBag.AverageRating = mediaItem.MediaRatings.Any()
                ? mediaItem.MediaRatings.Average(r => r.Rating)
                : 0;

            return View(mediaItem);
        }

        // GET: UserMedia/Videos
        public async Task<IActionResult> Videos()
        {
            var videos = await GetMediaByMediaTypeAsync("Video");
            return View(videos);
        }

        // GET: UserMedia/Audio
        public async Task<IActionResult> Audio()
        {
            var audios = await GetMediaByMediaTypeAsync("Audio");
            return View(audios);
        }

        // GET: UserMedia/Trailers
        public async Task<IActionResult> Trailers()
        {
            var trailers = await GetMediaByMediaTypeAsync("Trailer");
            return View(trailers);
        }

        // GET: UserMedia/Gallery
        public async Task<IActionResult> Gallery()
        {
            var galleryItems = await GetMediaByMediaTypeAsync("Gallery");
            return View(galleryItems);
        }

        // Common Helper Method Media Fetch karne ke liye
        private async Task<List<MediaItem>> GetMediaByMediaTypeAsync(string mediaType)
        {
            return await _context.MediaItems
                .Where(m => m.MediaType == mediaType)
                .Include(m => m.Category)
                .Include(m => m.MediaTags).ThenInclude(mt => mt.Tag)
                .Include(m => m.MediaRatings)
                .OrderByDescending(m => m.UploadedAt)
                .ToListAsync();
        }

        // POST: UserMedia/Rate (Rating submit karne ke liye)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRating(int mediaId, int rating, string returnUrl)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (rating < 1 || rating > 5)
            {
                TempData["Error"] = "Rating 1 se 5 ke beech honi chahiye.";
                return Redirect(returnUrl ?? Url.Action(nameof(Index)));
            }

            var existingRating = await _context.MediaRatings
                .FirstOrDefaultAsync(r => r.MediaId == mediaId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Rating = rating;
                existingRating.CreatedAt = DateTime.Now;
                _context.Update(existingRating);
            }
            else
            {
                var mediaRating = new MediaRating
                {
                    MediaId = mediaId,
                    UserId = userId,
                    Rating = rating,
                    CreatedAt = DateTime.Now
                };
                _context.Add(mediaRating);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Rating successfully submit ho gayi!";

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Details), new { id = mediaId });
        }
    }
}
