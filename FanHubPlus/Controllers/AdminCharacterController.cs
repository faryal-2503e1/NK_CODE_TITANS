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
    public class AdminCharacterController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;

        public AdminCharacterController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminCharacter
        public async Task<IActionResult> Index(string? q, int? categoryId, string? sort, int page = 1)
        {
            var query = _context.CharacterProfiles.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(c => c.Name.Contains(q) || (c.Bio != null && c.Bio.Contains(q)));
            }
            if (categoryId.HasValue) query = query.Where(c => c.CategoryId == categoryId);

            query = sort switch
            {
                "newest" => query.OrderByDescending(c => c.CharacterId),
                _ => query.OrderBy(c => c.Name).ThenBy(c => c.CharacterId)
            };

            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);

            return View(await PagedList<CharacterProfile>.CreateAsync(query, page, PageSize));
        }

        // GET: AdminCharacter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var characterProfile = await _context.CharacterProfiles
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CharacterId == id);

            if (characterProfile == null) return NotFound();

            return View(characterProfile);
        }

        // GET: AdminCharacter/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: AdminCharacter/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CharacterId,CategoryId,Name,Bio,ImageUrl")] CharacterProfile characterProfile, IFormFile? imageFile)
        {
            // If the user uploaded a file, save it and set the image path
            if (imageFile != null && imageFile.Length > 0)
            {
                characterProfile.ImageUrl = await ImageStorage.SaveFileAsync (imageFile, "characters");
            }

            if (ModelState.IsValid)
            {
                _context.Add(characterProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", characterProfile.CategoryId);
            return View(characterProfile);
        }

        // GET: AdminCharacter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var characterProfile = await _context.CharacterProfiles.FindAsync(id);
            if (characterProfile == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", characterProfile.CategoryId);
            return View(characterProfile);
        }

        // POST: AdminCharacter/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CharacterId,CategoryId,Name,Bio,ImageUrl")] CharacterProfile characterProfile, IFormFile? imageFile)
        {
            if (id != characterProfile.CharacterId) return NotFound();

            // If a new file was uploaded during edit, replace the old image
            if (imageFile != null && imageFile.Length > 0)
            {
                characterProfile.ImageUrl = await ImageStorage.SaveFileAsync(imageFile, "characters");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterProfileExists(characterProfile.CharacterId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", characterProfile.CategoryId);
            return View(characterProfile);
        }

        // GET: AdminCharacter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var characterProfile = await _context.CharacterProfiles
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CharacterId == id);

            if (characterProfile == null) return NotFound();

            return View(characterProfile);
        }

        // POST: AdminCharacter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterProfile = await _context.CharacterProfiles.FindAsync(id);
            if (characterProfile != null)
            {
                _context.CharacterProfiles.Remove(characterProfile);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterProfileExists(int id)
        {
            return _context.CharacterProfiles.Any(e => e.CharacterId == id);
        }
    }
}