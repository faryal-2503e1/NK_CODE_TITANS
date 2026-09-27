using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserCategoryController : Controller
    {
        private readonly FanHubContext _context;

        public UserCategoryController(FanHubContext context)
        {
            _context = context;
        }

        [Route("/categories")]
        public async Task<IActionResult> Index([FromQuery] string selected = null)
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.SelectedCategory = selected;

            ViewBag.Contents = await _context.Contents
                .Include(c => c.Category)
                .OrderByDescending(c => c.PopularityScore)
                .ToListAsync();

            return View(categories);
        }

        [Route("/categories/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Contents)
                .Include(c => c.CharacterProfiles)
                .Include(c => c.MerchandiseItems)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}