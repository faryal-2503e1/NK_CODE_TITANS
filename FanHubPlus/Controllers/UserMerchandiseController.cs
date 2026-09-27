using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserMerchandiseController : Controller
    {
        private readonly FanHubContext _context;

        public UserMerchandiseController(FanHubContext context)
        {
            _context = context;
        }

        [Route("/merchandise")]
        public async Task<IActionResult> Index()
        {
            var merchandiseItems = await _context.MerchandiseItems
                .Include(m => m.Category)
                .ToListAsync();

            return View(merchandiseItems);
        }

        [Route("/merchandise/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.MerchandiseItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            // Only track views for logged-in users, since the view log requires a UserId[cite: 2]
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.MerchandiseViews.Add(new MerchandiseViews
                {
                    ItemId = id,
                    UserId = userId,
                    ViewedAt = DateTime.Now
                });
                await _context.SaveChangesAsync();

                ViewBag.IsBookmarked = await _context.Bookmarks
                    .AnyAsync(b => b.UserId == userId && b.ItemType == "Merchandise" && b.MerchandiseItemId == id);
            }

            return View(item);
        }

        [Route("/merchandise/upcoming")]
        public async Task<IActionResult> Upcoming()
        {
            var upcomingItems = await _context.MerchandiseItems
                .Include(m => m.Category)
                .Where(m => m.IsUpcoming)
                .ToListAsync();

            return View(upcomingItems);
        }

        [Route("/merchandise/popular")]
        public async Task<IActionResult> Popular()
        {
            var popularItems = await _context.MerchandiseItems
                .Include(m => m.Category)
                .Include(m => m.MerchandiseViews)
                .OrderByDescending(m => m.MerchandiseViews.Count)
                .Take(20)
                .ToListAsync();

            return View(popularItems);
        }
    }
}