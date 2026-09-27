using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMediaController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AdminMediaController(FanHubContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: AdminMedia
        public async Task<IActionResult> Index(string? q, int? categoryId, string? mediaType, int? tagId, string? sort, int page = 1)
        {
            var query = _context.MediaItems
                .Include(m => m.Category)
                .Include(m => m.MediaTags).ThenInclude(mt => mt.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(m => m.Title.Contains(q));
            }
            if (categoryId.HasValue) query = query.Where(m => m.CategoryId == categoryId);
            if (!string.IsNullOrEmpty(mediaType)) query = query.Where(m => m.MediaType == mediaType);
            if (tagId.HasValue) query = query.Where(m => m.MediaTags.Any(mt => mt.TagId == tagId));

            query = sort switch
            {
                "title" => query.OrderBy(m => m.Title).ThenBy(m => m.MediaId),
                "oldest" => query.OrderBy(m => m.UploadedAt).ThenBy(m => m.MediaId),
                _ => query.OrderByDescending(m => m.UploadedAt).ThenBy(m => m.MediaId)
            };

            var items = await PagedList<MediaItem>.CreateAsync(query, page, PageSize);

            // Rating summary for only the rows on this page
            var ids = items.Select(m => m.MediaId).ToList();
            var stats = await _context.MediaRatings
                .Where(r => ids.Contains(r.MediaId))
                .GroupBy(r => r.MediaId)
                .Select(g => new { g.Key, Average = g.Average(r => r.Rating), Count = g.Count() })
                .ToListAsync();
            ViewBag.RatingStats = stats.ToDictionary(s => s.Key, s => (s.Average, s.Count));

            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);
            ViewBag.Tags = new SelectList(await _context.Tags.OrderBy(t => t.Name).ToListAsync(), "TagId", "Name", tagId);

            return View(items);
        }

        // GET: AdminMedia/Ratings
        public async Task<IActionResult> Ratings(int? mediaId, int? stars, int page = 1)
        {
            var query = _context.MediaRatings.Include(r => r.MediaItem).AsQueryable();

            if (mediaId.HasValue) query = query.Where(r => r.MediaId == mediaId);
            if (stars.HasValue) query = query.Where(r => r.Rating == stars);

            var ratings = await PagedList<MediaRating>.CreateAsync(query.OrderByDescending(r => r.CreatedAt).ThenBy(r => r.RatingId), page, 15);

            ViewBag.Users = await _userManager.GetUserNamesAsync(ratings.Select(r => r.UserId));
            ViewBag.MediaList = new SelectList(await _context.MediaItems.OrderBy(m => m.Title).ToListAsync(), "MediaId", "Title", mediaId);

            return View(ratings);
        }

        // GET: AdminMedia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var mediaItem = await _context.MediaItems
                .Include(m => m.Category)
                .Include(m => m.MediaTags).ThenInclude(mt => mt.Tag)
                .Include(m => m.MediaRatings)
                .FirstOrDefaultAsync(m => m.MediaId == id);

            if (mediaItem == null) return NotFound();

            ViewBag.Users = await _userManager.GetUserNamesAsync(mediaItem.MediaRatings.Select(r => r.UserId));
            return View(mediaItem);
        }

        // GET: AdminMedia/Create
        public async Task<IActionResult> Create()
        {
            await LoadFormListsAsync(null, null);
            return View(new MediaItem());
        }

        // POST: AdminMedia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(MediaPlayback.MaxRequestBytes)]
        [RequestFormLimits(MultipartBodyLengthLimit = MediaPlayback.MaxRequestBytes)]
        public async Task<IActionResult> Create([Bind("Title,FileUrl,MediaType,CategoryId")] MediaItem mediaItem, int[]? selectedTags, IFormFile? mediaFile)
        {
            await ValidateAsync(mediaItem, mediaFile);

            if (ModelState.IsValid)
            {
                var tagIds = await ExistingTagIdsAsync(selectedTags);
                mediaItem.MediaTags = tagIds.Select(t => new MediaTag { TagId = t }).ToList();

                // An uploaded file is stored on this website and replaces any typed link
                if (HasFile(mediaFile)) mediaItem.FileUrl = await SaveUploadAsync(mediaFile!);

                try
                {
                    _context.Add(mediaItem);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    DeleteUpload(mediaItem.FileUrl);
                    throw;
                }

                TempData["SuccessMessage"] = $"\"{mediaItem.Title}\" was added.";
                return RedirectToAction(nameof(Index));
            }

            await LoadFormListsAsync(mediaItem.CategoryId, selectedTags);
            return View(mediaItem);
        }

        // GET: AdminMedia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mediaItem = await _context.MediaItems.FindAsync(id);
            if (mediaItem == null) return NotFound();

            var current = await _context.MediaTags.Where(mt => mt.MediaId == id).Select(mt => mt.TagId).ToArrayAsync();
            await LoadFormListsAsync(mediaItem.CategoryId, current);
            return View(mediaItem);
        }

        // POST: AdminMedia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(MediaPlayback.MaxRequestBytes)]
        [RequestFormLimits(MultipartBodyLengthLimit = MediaPlayback.MaxRequestBytes)]
        public async Task<IActionResult> Edit(int id, [Bind("MediaId,Title,FileUrl,MediaType,UploadedAt,CategoryId")] MediaItem mediaItem, int[]? selectedTags, IFormFile? mediaFile)
        {
            if (id != mediaItem.MediaId) return NotFound();

            await ValidateAsync(mediaItem, mediaFile);

            if (ModelState.IsValid)
            {
                var oldUrl = await _context.MediaItems.AsNoTracking()
                    .Where(m => m.MediaId == id).Select(m => m.FileUrl).FirstOrDefaultAsync();

                if (HasFile(mediaFile)) mediaItem.FileUrl = await SaveUploadAsync(mediaFile!);

                try
                {
                    _context.Update(mediaItem);

                    var wanted = await ExistingTagIdsAsync(selectedTags);
                    var existing = await _context.MediaTags.Where(mt => mt.MediaId == id).ToListAsync();
                    _context.MediaTags.RemoveRange(existing.Where(mt => !wanted.Contains(mt.TagId)));
                    foreach (var tagId in wanted.Except(existing.Select(mt => mt.TagId)))
                    {
                        _context.MediaTags.Add(new MediaTag { MediaId = id, TagId = tagId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (mediaItem.FileUrl != oldUrl) DeleteUpload(mediaItem.FileUrl);
                    if (!_context.MediaItems.Any(e => e.MediaId == mediaItem.MediaId)) return NotFound();
                    else throw;
                }

                // The old file is no longer used once it has been replaced
                if (oldUrl != mediaItem.FileUrl) DeleteUpload(oldUrl);

                TempData["SuccessMessage"] = $"\"{mediaItem.Title}\" was updated.";
                return RedirectToAction(nameof(Index));
            }

            await LoadFormListsAsync(mediaItem.CategoryId, selectedTags);
            return View(mediaItem);
        }

        // GET: AdminMedia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var mediaItem = await _context.MediaItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MediaId == id);

            if (mediaItem == null) return NotFound();

            return View(mediaItem);
        }

        // POST: AdminMedia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mediaItem = await _context.MediaItems.FindAsync(id);
            if (mediaItem != null)
            {
                _context.MediaItems.Remove(mediaItem);
                await _context.SaveChangesAsync();
                DeleteUpload(mediaItem.FileUrl);
                TempData["SuccessMessage"] = $"\"{mediaItem.Title}\" was deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private static bool HasFile(IFormFile? file) => file != null && file.Length > 0;

        // Every media item has to be playable inside the site: an uploaded file, a direct file
        // link, or a YouTube/Vimeo link that is shown through the embedded player.
        private async Task ValidateAsync(MediaItem item, IFormFile? file)
        {
            var allowed = MediaPlayback.AllowedExtensions(item.MediaType);

            if (string.IsNullOrWhiteSpace(item.MediaType))
            {
                ModelState.AddModelError(nameof(MediaItem.MediaType), "Choose a media type.");
            }
            else if (allowed.Length == 0)
            {
                ModelState.AddModelError(nameof(MediaItem.MediaType), "Choose a media type from the list.");
            }
            else if (HasFile(file))
            {
                var ext = Path.GetExtension(file!.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("mediaFile", $"A {item.MediaType} file must be one of: {string.Join(", ", allowed)}.");
                }
                else if (file.Length > MediaPlayback.MaxUploadBytes)
                {
                    ModelState.AddModelError("mediaFile", "The file is larger than 100 MB.");
                }
            }
            else if (string.IsNullOrWhiteSpace(item.FileUrl))
            {
                ModelState.AddModelError(nameof(MediaItem.FileUrl), "Upload a file or enter a media link.");
            }
            else if (MediaPlayback.IsLocalUpload(item.FileUrl))
            {
                // Keeping the file that was uploaded earlier (edit form): it must still exist and match the type
                if (!System.IO.File.Exists(UploadPath(item.FileUrl)) || !allowed.Contains(MediaPlayback.GetExtension(item.FileUrl)))
                {
                    ModelState.AddModelError(nameof(MediaItem.FileUrl), "The stored file does not match this media type. Upload the file again.");
                }
            }
            else if (!Uri.TryCreate(item.FileUrl, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                ModelState.AddModelError(nameof(MediaItem.FileUrl), "Enter a full http:// or https:// link.");
            }
            else if (!MediaPlayback.CanPlay(item.FileUrl, item.MediaType))
            {
                ModelState.AddModelError(nameof(MediaItem.FileUrl),
                    MediaPlayback.AllowsEmbed(item.MediaType)
                        ? $"This link cannot be played inside the site. Use a YouTube or Vimeo link, a direct file link ({string.Join(", ", allowed)}), or upload the file."
                        : $"This link cannot be played inside the site. Use a direct file link ({string.Join(", ", allowed)}) or upload the file.");
            }

            if (item.CategoryId.HasValue && !await _context.Categories.AnyAsync(c => c.CategoryId == item.CategoryId))
            {
                ModelState.AddModelError(nameof(MediaItem.CategoryId), "Choose a valid category.");
            }
        }

        // Path.GetFileName keeps the result inside the uploads folder whatever the stored value is
        private string UploadPath(string url) =>
            Path.Combine(_env.WebRootPath, "uploads", "media", Path.GetFileName(url));

        // Saves under a random name, so uploaded names can never overwrite or guess other files
        private async Task<string> SaveUploadAsync(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "media");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName).ToLowerInvariant();
            await using var stream = System.IO.File.Create(Path.Combine(folder, fileName));
            await file.CopyToAsync(stream);

            return MediaPlayback.UploadPrefix + fileName;
        }

        // Only files this controller saved (under /uploads/media/) are ever deleted; external links are left alone
        private void DeleteUpload(string? url)
        {
            if (!MediaPlayback.IsLocalUpload(url)) return;

            var path = UploadPath(url!);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }

        private async Task<List<int>> ExistingTagIdsAsync(int[]? selected)
        {
            if (selected == null || selected.Length == 0) return new List<int>();
            var ids = selected.Distinct().ToList();
            return await _context.Tags.Where(t => ids.Contains(t.TagId)).Select(t => t.TagId).ToListAsync();
        }

        private async Task LoadFormListsAsync(int? categoryId, int[]? selectedTags)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);
            ViewBag.AllTags = await _context.Tags.OrderBy(t => t.Name).ToListAsync();
            ViewBag.SelectedTags = new HashSet<int>(selectedTags ?? Array.Empty<int>());
        }
    }
}
