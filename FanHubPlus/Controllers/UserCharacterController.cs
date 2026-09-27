using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserCharacterController : Controller
    {
        private readonly FanHubContext _context;

        public UserCharacterController(FanHubContext context)
        {
            _context = context;
        }

        [Route("/characters")]
        public async Task<IActionResult> Index()
        {
            var characters = await _context.CharacterProfiles
                .Include(c => c.Category)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(characters);
        }

        [Route("/characters/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var character = await _context.CharacterProfiles
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.CharacterId == id);

            if (character == null)
            {
                return NotFound();
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.IsBookmarked = await _context.Bookmarks
                    .AnyAsync(b => b.UserId == userId && b.ItemType == "Character" && b.CharacterId == id);
            }

            return View(character);
        }

        [Route("/characters/filter")]
        public async Task<IActionResult> Filter(int? categoryId)
        {
            var characterQuery = _context.CharacterProfiles.Include(c => c.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                characterQuery = characterQuery.Where(c => c.CategoryId == categoryId.Value);
            }

            var filteredCharacters = await characterQuery
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.SelectedCategoryId = categoryId;

            return View(filteredCharacters);
        }
    }
}