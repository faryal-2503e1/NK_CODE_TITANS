using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;
using FanHubPlus.Models;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMerchandiseController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;

        public AdminMerchandiseController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminMerchandise
        public async Task<IActionResult> Index(string? q, int? categoryId, string? tag, string? status, string? sort, int page = 1)
        {
            var query = _context.MerchandiseItems.Include(m => m.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(m => m.Name.Contains(q));
            }
            if (categoryId.HasValue) query = query.Where(m => m.CategoryId == categoryId);
            if (!string.IsNullOrEmpty(tag)) query = query.Where(m => m.Tag == tag);
            if (status == "upcoming") query = query.Where(m => m.IsUpcoming);
            else if (status == "released") query = query.Where(m => !m.IsUpcoming);

            query = sort switch
            {
                "name" => query.OrderBy(m => m.Name).ThenBy(m => m.ItemId),
                "views" => query.OrderByDescending(m => m.MerchandiseViews.Count).ThenBy(m => m.ItemId),
                _ => query.OrderByDescending(m => m.ItemId)
            };

            var items = await PagedList<MerchandiseItem>.CreateAsync(query, page, PageSize);

            var ids = items.Select(m => m.ItemId).ToList();
            ViewBag.ViewCounts = await _context.MerchandiseViews
                .Where(v => ids.Contains(v.ItemId))
                .GroupBy(v => v.ItemId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);

            return View(items);
        }

        // GET: AdminMerchandise/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var merchandiseItem = await _context.MerchandiseItems
                .Include(m => m.Category)
                .Include(m => m.MerchandiseViews)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (merchandiseItem == null) return NotFound();

            return View(merchandiseItem);
        }

        // GET: AdminMerchandise/Create
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync(null);
            return View(new MerchandiseItem());
        }

        // POST: AdminMerchandise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,ImageUrl,Tag,IsUpcoming")] MerchandiseItem merchandiseItem, IFormFile? imageFile)
        {
            // If a file was uploaded, save it and store the local path
            if (imageFile != null && imageFile.Length > 0)
            {
                merchandiseItem.ImageUrl = await ImageStorage.SaveFileAsync(imageFile, "merchandise");
            }

            await ValidateAsync(merchandiseItem, storedTag: null);

            if (ModelState.IsValid)
            {
                _context.Add(merchandiseItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"\"{merchandiseItem.Name}\" was added.";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoriesAsync(merchandiseItem.CategoryId);
            return View(merchandiseItem);
        }

        // GET: AdminMerchandise/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var merchandiseItem = await _context.MerchandiseItems.FindAsync(id);
            if (merchandiseItem == null) return NotFound();

            await LoadCategoriesAsync(merchandiseItem.CategoryId);
            return View(merchandiseItem);
        }

        // POST: AdminMerchandise/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,CategoryId,Name,ImageUrl,Tag,IsUpcoming")] MerchandiseItem merchandiseItem, IFormFile? imageFile)
        {
            if (id != merchandiseItem.ItemId) return NotFound();

            // If a new file was uploaded, replace the old image
            if (imageFile != null && imageFile.Length > 0)
            {
                merchandiseItem.ImageUrl = await ImageStorage.SaveFileAsync(imageFile, "merchandise");
            }

            var storedTag = await _context.MerchandiseItems.AsNoTracking()
                .Where(m => m.ItemId == id).Select(m => m.Tag).FirstOrDefaultAsync();
            await ValidateAsync(merchandiseItem, storedTag);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchandiseItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MerchandiseItems.Any(e => e.ItemId == merchandiseItem.ItemId)) return NotFound();
                    else throw;
                }

                TempData["SuccessMessage"] = $"\"{merchandiseItem.Name}\" was updated.";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoriesAsync(merchandiseItem.CategoryId);
            return View(merchandiseItem);
        }

        // GET: AdminMerchandise/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var merchandiseItem = await _context.MerchandiseItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (merchandiseItem == null) return NotFound();

            return View(merchandiseItem);
        }

        // POST: AdminMerchandise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var merchandiseItem = await _context.MerchandiseItems.FindAsync(id);
            if (merchandiseItem != null)
            {
                _context.MerchandiseItems.Remove(merchandiseItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"\"{merchandiseItem.Name}\" was deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateAsync(MerchandiseItem item, string? storedTag)
        {
            if (!await _context.Categories.AnyAsync(c => c.CategoryId == item.CategoryId))
            {
                ModelState.AddModelError(nameof(MerchandiseItem.CategoryId), "Choose a category.");
            }

            if (!string.IsNullOrWhiteSpace(item.Tag)
                && !AdminOptions.MerchandiseTags.Contains(item.Tag)
                && !string.Equals(item.Tag, storedTag, StringComparison.Ordinal))
            {
                ModelState.AddModelError(nameof(MerchandiseItem.Tag), "Choose one of the listed tags.");
            }

            // Allow both local uploads (/uploads/...) and valid http/https URLs
            if (!string.IsNullOrWhiteSpace(item.ImageUrl)
                && !item.ImageUrl.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)
                && (!Uri.TryCreate(item.ImageUrl, UriKind.Absolute, out var uri)
                    || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
            {
                ModelState.AddModelError(nameof(MerchandiseItem.ImageUrl), "Enter a full http:// or https:// image link or upload a file.");
            }
        }

        private async Task LoadCategoriesAsync(int? categoryId)
        {
            ViewData["CategoryId"] = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);
        }
    }
}