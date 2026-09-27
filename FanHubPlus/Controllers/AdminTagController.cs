using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;

namespace FanHubPlus.Controllers
{
    // Tags are shared by fandom content and multimedia items.
    [Authorize(Roles = "Admin")]
    public class AdminTagController : Controller
    {
        private const int PageSize = 15;
        private readonly FanHubContext _context;

        public AdminTagController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminTag
        public async Task<IActionResult> Index(string? q, int page = 1)
        {
            var query = _context.Tags.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(t => t.Name.Contains(q));
            }

            var tags = await PagedList<Tag>.CreateAsync(query.OrderBy(t => t.Name), page, PageSize);

            // Usage counts for only the tags on this page
            var ids = tags.Select(t => t.TagId).ToList();
            ViewBag.ContentCounts = await _context.ContentTags.Where(ct => ids.Contains(ct.TagId))
                .GroupBy(ct => ct.TagId).ToDictionaryAsync(g => g.Key, g => g.Count());
            ViewBag.MediaCounts = await _context.MediaTags.Where(mt => ids.Contains(mt.TagId))
                .GroupBy(mt => mt.TagId).ToDictionaryAsync(g => g.Key, g => g.Count());

            return View(tags);
        }

        // GET: AdminTag/Create
        public IActionResult Create()
        {
            return View(new Tag());
        }

        // POST: AdminTag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Tag tag)
        {
            tag.Name = (tag.Name ?? string.Empty).Trim();
            await CheckDuplicateAsync(tag);

            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Tag \"{tag.Name}\" was added.";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: AdminTag/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            return View(tag);
        }

        // POST: AdminTag/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagId,Name")] Tag tag)
        {
            if (id != tag.TagId) return NotFound();

            tag.Name = (tag.Name ?? string.Empty).Trim();
            await CheckDuplicateAsync(tag);

            if (ModelState.IsValid)
            {
                var existing = await _context.Tags.FindAsync(id);
                if (existing == null) return NotFound();

                existing.Name = tag.Name;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Tag \"{tag.Name}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: AdminTag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            ViewBag.ContentCount = await _context.ContentTags.CountAsync(ct => ct.TagId == id);
            ViewBag.MediaCount = await _context.MediaTags.CountAsync(mt => mt.TagId == id);
            return View(tag);
        }

        // POST: AdminTag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                // Remove the links first so no content or media keeps a dangling tag
                _context.ContentTags.RemoveRange(_context.ContentTags.Where(ct => ct.TagId == id));
                _context.MediaTags.RemoveRange(_context.MediaTags.Where(mt => mt.TagId == id));
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Tag \"{tag.Name}\" was deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task CheckDuplicateAsync(Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                ModelState.AddModelError(nameof(Tag.Name), "Tag name is required.");
                return;
            }

            var lower = tag.Name.ToLower();
            if (await _context.Tags.AnyAsync(t => t.Name.ToLower() == lower && t.TagId != tag.TagId))
            {
                ModelState.AddModelError(nameof(Tag.Name), "A tag with this name already exists.");
            }
        }
    }
}
