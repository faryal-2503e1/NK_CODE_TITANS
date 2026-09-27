using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Models;

namespace FanHubPlus.Controllers
{
    public class UserCalendarController : Controller
    {
        private readonly FanHubContext _context;

        public UserCalendarController(FanHubContext context)
        {
            _context = context;
        }

        // GET: UserCalendar/Index
        public async Task<IActionResult> Index()
        {
            // The calendar only ever shows what's still ahead; an event that has already
            // happened has no reason to sit in a "browse events" list.
            var events = await _context.Events
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Category)
                .Where(e => e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        // GET: UserCalendar/Monthly?month=10&year=2026
        public async Task<IActionResult> Monthly(int? month, int? year)
        {
            int selectedMonth = month ?? DateTime.Now.Month;
            int selectedYear = year ?? DateTime.Now.Year;

            var monthlyEvents = await _context.Events
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Category)
                .Where(e => e.EventDate.Month == selectedMonth
                         && e.EventDate.Year == selectedYear
                         && e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;

            return View(monthlyEvents);
        }

        // GET: UserCalendar/EventDetails/5
        public async Task<IActionResult> EventDetails(int? id)
        {
            if (id == null) return NotFound();

            var evt = await _context.Events
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Category)
                .Include(e => e.EventTicketLinks)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (evt == null) return NotFound();

            return View(evt);
        }

        // JSON API endpoint (used by any JavaScript calendar widget on the page)
        [HttpGet]
        public async Task<IActionResult> GetEventsJson()
        {
            var eventData = await _context.Events
                .Where(e => e.EventDate >= DateTime.Now)
                .Select(e => new
                {
                    id = e.EventId,
                    title = e.Title,
                    start = e.EventDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    city = e.City ?? "Online",
                    locationCoordinates = e.LocationCoordinates,
                    url = Url.Action("EventDetails", "UserCalendar", new { id = e.EventId })
                })
                .ToListAsync();

            return Json(eventData);
        }
    }
}