using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly FanHubContext _context;

        public AdminCategoryController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminCategory
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.Contents)
                .Include(c => c.CharacterProfiles)
                .Include(c => c.MerchandiseItems)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        // GET: AdminCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .Include(c => c.Contents)
                .Include(c => c.CharacterProfiles)
                .Include(c => c.MerchandiseItems)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return View(category);
        }

        // GET: AdminCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Category category)
        {
            if (await CategoryNameExists(category.Name))
            {
                ModelState.AddModelError(nameof(Category.Name), "A category with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: AdminCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: AdminCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Description")] Category category)
        {
            if (id != category.CategoryId) return NotFound();

            if (await CategoryNameExists(category.Name, excludingId: id))
            {
                ModelState.AddModelError(nameof(Category.Name), "A category with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: AdminCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .Include(c => c.Contents)
                .Include(c => c.CharacterProfiles)
                .Include(c => c.MerchandiseItems)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return View(category);
        }

        // POST: AdminCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Contents)
                .Include(c => c.CharacterProfiles)
                .Include(c => c.MerchandiseItems)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            // A category that still has content, characters or merchandise attached shouldn't
            // vanish silently — the admin needs to move or remove that first.
            var linkedItemCount = (category.Contents?.Count ?? 0)
                + (category.CharacterProfiles?.Count ?? 0)
                + (category.MerchandiseItems?.Count ?? 0);

            if (linkedItemCount > 0)
            {
                TempData["ErrorMessage"] =
                    $"\"{category.Name}\" still has {linkedItemCount} item(s) linked to it (content, characters or merchandise). Move or delete those first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        private async Task<bool> CategoryNameExists(string name, int? excludingId = null)
        {
            return await _context.Categories.AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() && c.CategoryId != excludingId);
        }
    }
}
