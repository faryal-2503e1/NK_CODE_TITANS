using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Models;
using FanHubPlus.Helpers;
using FanHubPlus.ViewModels;

namespace FanHubPlus.Controllers
{
    public class UserEventController : Controller
    {
        private readonly FanHubContext _context;

        public UserEventController(FanHubContext context)
        {
            _context = context;
        }

        // GET: UserEvent/Index
        public async Task<IActionResult> Index(string? search, string? city)
        {
            var query = _context.Events
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Category)
                .Include(e => e.EventTicketLinks)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Title.Contains(search) ||
                                         (e.Description != null && e.Description.Contains(search)));
            }

            if (!string.IsNullOrEmpty(city) && city != "all")
            {
                query = query.Where(e => e.City != null && e.City.ToLower() == city.ToLower());
            }

            ViewBag.Cities = await _context.Events
                .Where(e => !string.IsNullOrEmpty(e.City))
                .Select(e => e.City!)
                .Distinct()
                .ToListAsync();

            var events = await query.OrderByDescending(e => e.EventDate).ToListAsync();
            return View(events);
        }

        // GET: UserEvent/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: UserEvent/Upcoming
        public async Task<IActionResult> Upcoming()
        {
            var upcomingEvents = await _context.Events
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Category)
                .Include(e => e.EventTicketLinks)
                .Where(e => e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(upcomingEvents);
        }

        // GET: UserEvent/Nearby
        // Supports two ways of finding "nearby" events, per the SRS's Map & GPS Integration requirement:
        //  1) Manual city selection (kept for when geolocation isn't available/allowed).
        //  2) Real GPS: the browser sends the visitor's lat/lng (via navigator.geolocation),
        //     and results are sorted by actual distance (km) using Event.LocationCoordinates.
        public async Task<IActionResult> Nearby(string? city, double? lat, double? lng, double? radiusKm)
        {
            var query = _context.Events
                .Include(e => e.EventCategories)
                    .ThenInclude(ec => ec.Category)
                .Include(e => e.EventTicketLinks)
                .Where(e => e.EventDate >= DateTime.Now)
                .AsQueryable();

            var usingGeolocation = lat.HasValue && lng.HasValue;

            if (!usingGeolocation && !string.IsNullOrEmpty(city))
            {
                query = query.Where(e => e.City != null && e.City.ToLower() == city.ToLower());
            }

            ViewBag.SelectedCity = city;
            ViewBag.UsingGeolocation = usingGeolocation;
            ViewBag.UserLat = lat;
            ViewBag.UserLng = lng;
            ViewBag.RadiusKm = radiusKm ?? 100; // default search radius when using GPS
            ViewBag.AvailableCities = await _context.Events
                .Where(e => e.City != null && e.EventDate >= DateTime.Now)
                .Select(e => e.City)
                .Distinct()
                .ToListAsync();

            var events = await query.ToListAsync();

            List<NearbyEventViewModel> result;

            if (usingGeolocation)
            {
                var effectiveRadius = radiusKm ?? 100;

                result = events
                    .Select(e =>
                    {
                        var coords = GeoDistance.TryParse(e.LocationCoordinates);
                        double? distance = coords.HasValue
                            ? GeoDistance.KmBetween(lat!.Value, lng!.Value, coords.Value.Lat, coords.Value.Lng)
                            : null;

                        return new NearbyEventViewModel { Event = e, DistanceKm = distance };
                    })
                    // Events without valid coordinates can't be distance-checked, so keep them out
                    // of a GPS-based search rather than showing a wrong/misleading result.
                    .Where(x => x.DistanceKm.HasValue && x.DistanceKm.Value <= effectiveRadius)
                    .OrderBy(x => x.DistanceKm)
                    .ToList();
            }
            else
            {
                result = events
                    .OrderBy(e => e.EventDate)
                    .Select(e => new NearbyEventViewModel { Event = e, DistanceKm = null })
                    .ToList();
            }

            return View(result);
        }
    }
}