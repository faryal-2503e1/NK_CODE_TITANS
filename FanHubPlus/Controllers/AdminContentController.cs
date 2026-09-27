using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;
using FanHubPlus.Hubs;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminContentController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public AdminContentController(FanHubContext context, IHubContext<NotificationHub> notificationHub)
        {
            _context = context;
            _notificationHub = notificationHub;
        }

        // GET: AdminContent
        public async Task<IActionResult> Index(string? q, int? categoryId, string? type, int? tagId, string? sort, int page = 1)
        {
            var query = _context.Contents.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(c => c.Title.Contains(q) || (c.Description != null && c.Description.Contains(q)));
            }
            if (categoryId.HasValue) query = query.Where(c => c.CategoryId == categoryId);
            if (!string.IsNullOrEmpty(type)) query = query.Where(c => c.Type == type);
            if (tagId.HasValue) query = query.Where(c => c.ContentTags.Any(ct => ct.TagId == tagId));

            query = sort switch
            {
                "title" => query.OrderBy(c => c.Title).ThenBy(c => c.ContentId),
                "popular" => query.OrderByDescending(c => c.PopularityScore).ThenBy(c => c.ContentId),
                "oldest" => query.OrderBy(c => c.ReleaseDate).ThenBy(c => c.ContentId),
                _ => query.OrderByDescending(c => c.ReleaseDate).ThenBy(c => c.ContentId)
            };

            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);
            ViewBag.Tags = new SelectList(await _context.Tags.OrderBy(t => t.Name).ToListAsync(), "TagId", "Name", tagId);

            return View(await PagedList<Content>.CreateAsync(query, page, PageSize));
        }

        // GET: AdminContent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var content = await _context.Contents
                .Include(c => c.Category)
                .Include(c => c.ContentTags).ThenInclude(ct => ct.Tag)
                .FirstOrDefaultAsync(m => m.ContentId == id);

            if (content == null) return NotFound();

            ViewBag.ViewCount = await _context.ContentViews.CountAsync(v => v.ContentId == id);
            ViewBag.BookmarkCount = await _context.Bookmarks.CountAsync(b => b.ContentId == id);

            return View(content);
        }

        // GET: AdminContent/Create
        public async Task<IActionResult> Create()
        {
            await LoadFormListsAsync(null, null);
            return View(new Content { ReleaseDate = DateTime.Today });
        }

        // POST: AdminContent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Title,Type,Description,ReleaseDate,PopularityScore")] Content content, int[]? selectedTags)
        {
            await ValidateAsync(content);

            if (ModelState.IsValid)
            {
                var tagIds = await ExistingTagIdsAsync(selectedTags);
                content.ContentTags = tagIds.Select(t => new ContentTag { TagId = t }).ToList();

                _context.Add(content);
                await _context.SaveChangesAsync();

                await NotifyFavoriteCategoryFansAsync(content);

                TempData["SuccessMessage"] = $"\"{content.Title}\" was added.";
                return RedirectToAction(nameof(Index));
            }

            await LoadFormListsAsync(content.CategoryId, selectedTags);
            return View(content);
        }

        // GET: AdminContent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var content = await _context.Contents.FindAsync(id);
            if (content == null) return NotFound();

            var current = await _context.ContentTags.Where(ct => ct.ContentId == id).Select(ct => ct.TagId).ToArrayAsync();
            await LoadFormListsAsync(content.CategoryId, current);
            return View(content);
        }

        // POST: AdminContent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentId,CategoryId,Title,Type,Description,ReleaseDate,PopularityScore")] Content content, int[]? selectedTags)
        {
            if (id != content.ContentId) return NotFound();

            await ValidateAsync(content);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(content);

                    // Sync the tag links with what was ticked on the form
                    var wanted = await ExistingTagIdsAsync(selectedTags);
                    var existing = await _context.ContentTags.Where(ct => ct.ContentId == id).ToListAsync();
                    _context.ContentTags.RemoveRange(existing.Where(ct => !wanted.Contains(ct.TagId)));
                    foreach (var tagId in wanted.Except(existing.Select(ct => ct.TagId)))
                    {
                        _context.ContentTags.Add(new ContentTag { ContentId = id, TagId = tagId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Contents.Any(e => e.ContentId == content.ContentId)) return NotFound();
                    else throw;
                }

                TempData["SuccessMessage"] = $"\"{content.Title}\" was updated.";
                return RedirectToAction(nameof(Index));
            }

            await LoadFormListsAsync(content.CategoryId, selectedTags);
            return View(content);
        }

        // GET: AdminContent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var content = await _context.Contents
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.ContentId == id);

            if (content == null) return NotFound();

            return View(content);
        }

        // POST: AdminContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                _context.Contents.Remove(content);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"\"{content.Title}\" was deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Smart Category Notifications: sirf usi category ke group ko toast bhejte hain
        // jis category mein yeh naya content publish hua hai, taake baaki fans ko
        // irrelevant notifications na milein.
        private async Task NotifyFavoriteCategoryFansAsync(Content content)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == content.CategoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            if (category == null) return;

            await _notificationHub.Clients
                .Group($"category-{content.CategoryId}")
                .SendAsync("NewContentPublished", new
                {
                    contentId = content.ContentId,
                    title = content.Title,
                    type = content.Type,
                    categoryName = category,
                    url = Url.Action("Details", "UserContent", new { id = content.ContentId })
                });
        }

        private async Task ValidateAsync(Content content)
        {
            if (!AdminOptions.ContentTypes.Contains(content.Type))
            {
                ModelState.AddModelError("Type", "Choose a valid content type.");
            }
            if (!await _context.Categories.AnyAsync(c => c.CategoryId == content.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Choose a category.");
            }
        }

        // Ignores tampered ids by keeping only tags that really exist
        private async Task<List<int>> ExistingTagIdsAsync(int[]? selected)
        {
            if (selected == null || selected.Length == 0) return new List<int>();
            var ids = selected.Distinct().ToList();
            return await _context.Tags.Where(t => ids.Contains(t.TagId)).Select(t => t.TagId).ToListAsync();
        }

        private async Task LoadFormListsAsync(int? categoryId, int[]? selectedTags)
        {
            ViewData["CategoryId"] = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);
            ViewBag.AllTags = await _context.Tags.OrderBy(t => t.Name).ToListAsync();
            ViewBag.SelectedTags = new HashSet<int>(selectedTags ?? Array.Empty<int>());
        }
    }
}
