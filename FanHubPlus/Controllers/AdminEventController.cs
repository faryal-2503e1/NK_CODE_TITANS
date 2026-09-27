using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminEventController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;

        public AdminEventController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminEvent
        public async Task<IActionResult> Index(string? q, string? city, string? when, int? categoryId, string? sort, int page = 1)
        {
            var query = _context.Events
                .Include(e => e.EventCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.EventTicketLinks)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(e => e.Title.Contains(q) || (e.Description != null && e.Description.Contains(q)));
            }
            if (!string.IsNullOrWhiteSpace(city)) query = query.Where(e => e.City == city);
            if (categoryId.HasValue) query = query.Where(e => e.EventCategories.Any(ec => ec.CategoryId == categoryId));

            var now = DateTime.Now;
            if (when == "upcoming") query = query.Where(e => e.EventDate >= now);
            else if (when == "past") query = query.Where(e => e.EventDate < now);

            query = sort switch
            {
                "title" => query.OrderBy(e => e.Title).ThenBy(e => e.EventId),
                "date_desc" => query.OrderByDescending(e => e.EventDate).ThenBy(e => e.EventId),
                _ => query.OrderBy(e => e.EventDate).ThenBy(e => e.EventId)
            };

            ViewBag.Cities = await _context.Events.Where(e => e.City != null && e.City != "")
                .Select(e => e.City!).Distinct().OrderBy(c => c).ToListAsync();
            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);

            return View(await PagedList<Event>.CreateAsync(query, page, PageSize));
        }

        // GET: AdminEvent/Categories
        public async Task<IActionResult> Categories(int? categoryId, int page = 1)
        {
            var query = _context.EventCategories
                .Include(ec => ec.Event)
                .Include(ec => ec.Category)
                .AsQueryable();

            if (categoryId.HasValue) query = query.Where(ec => ec.CategoryId == categoryId);

            ViewBag.Categories = new SelectList(await _context.Categories.OrderBy(c => c.Name).ToListAsync(), "CategoryId", "Name", categoryId);

            return View(await PagedList<EventCategory>.CreateAsync(
                query.OrderBy(ec => ec.Event.Title).ThenBy(ec => ec.Category.Name), page, 15));
        }

        // POST: AdminEvent/RemoveCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCategory(int eventId, int categoryId)
        {
            var link = await _context.EventCategories.FindAsync(eventId, categoryId);
            if (link != null)
            {
                _context.EventCategories.Remove(link);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category removed from the event.";
            }
            return RedirectToAction(nameof(Categories));
        }

        // GET: AdminEvent/TicketLinks
        public async Task<IActionResult> TicketLinks(string? q, int page = 1)
        {
            var query = _context.EventTicketLinks.Include(t => t.Event).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(t => t.Event.Title.Contains(q) || t.PlatformName.Contains(q));
            }

            return View(await PagedList<EventTicketLink>.CreateAsync(
                query.OrderBy(t => t.Event.Title).ThenBy(t => t.TicketLinkId), page, 15));
        }

        // GET: AdminEvent/AddTicketLink?eventId=5
        public async Task<IActionResult> AddTicketLink(int eventId)
        {
            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null) return NotFound();

            ViewBag.EventTitle = eventItem.Title;
            return View("TicketLinkForm", new EventTicketLink { EventId = eventId });
        }

        // POST: AdminEvent/AddTicketLink
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketLink([Bind("EventId,PlatformName,Url")] EventTicketLink link)
        {
            var eventItem = await _context.Events.FindAsync(link.EventId);
            if (eventItem == null) return NotFound();

            ValidateTicketLink(link);

            if (ModelState.IsValid)
            {
                _context.Add(link);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ticket link added.";
                return RedirectToAction(nameof(Details), new { id = link.EventId });
            }

            ViewBag.EventTitle = eventItem.Title;
            return View("TicketLinkForm", link);
        }

        // GET: AdminEvent/EditTicketLink/5
        public async Task<IActionResult> EditTicketLink(int? id)
        {
            if (id == null) return NotFound();

            var link = await _context.EventTicketLinks.Include(t => t.Event).FirstOrDefaultAsync(t => t.TicketLinkId == id);
            if (link == null) return NotFound();

            ViewBag.EventTitle = link.Event.Title;
            return View("TicketLinkForm", link);
        }

        // POST: AdminEvent/EditTicketLink/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicketLink(int id, [Bind("TicketLinkId,EventId,PlatformName,Url")] EventTicketLink link)
        {
            if (id != link.TicketLinkId) return NotFound();

            var existing = await _context.EventTicketLinks.Include(t => t.Event).FirstOrDefaultAsync(t => t.TicketLinkId == id);
            if (existing == null) return NotFound();

            ValidateTicketLink(link);

            if (ModelState.IsValid)
            {
                // The link stays with its original event
                existing.PlatformName = link.PlatformName;
                existing.Url = link.Url;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ticket link updated.";
                return RedirectToAction(nameof(Details), new { id = existing.EventId });
            }

            link.EventId = existing.EventId;
            ViewBag.EventTitle = existing.Event.Title;
            return View("TicketLinkForm", link);
        }

        // POST: AdminEvent/DeleteTicketLink/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTicketLink(int id)
        {
            var link = await _context.EventTicketLinks.FindAsync(id);
            if (link == null) return NotFound();

            var eventId = link.EventId;
            _context.EventTicketLinks.Remove(link);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ticket link deleted.";
            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        // GET: AdminEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Events
                .Include(e => e.EventCategories).ThenInclude(ec => ec.Category)
                .Include(e => e.EventTicketLinks)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null) return NotFound();

            return View(eventItem);
        }

        // GET: AdminEvent/Create
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync(null);
            return View(new Event { EventDate = DateTime.Today.AddDays(7) });
        }

        // POST: AdminEvent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,EventDate,City,LocationCoordinates")] Event eventItem, int[]? selectedCategories)
        {
            if (ModelState.IsValid)
            {
                var categoryIds = await ExistingCategoryIdsAsync(selectedCategories);
                eventItem.EventCategories = categoryIds.Select(c => new EventCategory { CategoryId = c }).ToList();

                _context.Add(eventItem);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"\"{eventItem.Title}\" was added. You can now add ticket links to it.";
                return RedirectToAction(nameof(Details), new { id = eventItem.EventId });
            }

            await LoadCategoriesAsync(selectedCategories);
            return View(eventItem);
        }

        // GET: AdminEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound();

            var current = await _context.EventCategories.Where(ec => ec.EventId == id).Select(ec => ec.CategoryId).ToArrayAsync();
            await LoadCategoriesAsync(current);
            return View(eventItem);
        }

        // POST: AdminEvent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Title,Description,EventDate,City,LocationCoordinates")] Event eventItem, int[]? selectedCategories)
        {
            if (id != eventItem.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);

                    var wanted = await ExistingCategoryIdsAsync(selectedCategories);
                    var existing = await _context.EventCategories.Where(ec => ec.EventId == id).ToListAsync();
                    _context.EventCategories.RemoveRange(existing.Where(ec => !wanted.Contains(ec.CategoryId)));
                    foreach (var categoryId in wanted.Except(existing.Select(ec => ec.CategoryId)))
                    {
                        _context.EventCategories.Add(new EventCategory { EventId = id, CategoryId = categoryId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.EventId == eventItem.EventId)) return NotFound();
                    else throw;
                }

                TempData["SuccessMessage"] = $"\"{eventItem.Title}\" was updated.";
                return RedirectToAction(nameof(Details), new { id });
            }

            await LoadCategoriesAsync(selectedCategories);
            return View(eventItem);
        }

        // GET: AdminEvent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Events
                .Include(e => e.EventTicketLinks)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null) return NotFound();

            return View(eventItem);
        }

        // POST: AdminEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"\"{eventItem.Title}\" was deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private void ValidateTicketLink(EventTicketLink link)
        {
            if (!string.IsNullOrWhiteSpace(link.Url)
                && (!Uri.TryCreate(link.Url, UriKind.Absolute, out var uri)
                    || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
            {
                ModelState.AddModelError(nameof(EventTicketLink.Url), "Enter a full http:// or https:// link.");
            }
        }

        private async Task<List<int>> ExistingCategoryIdsAsync(int[]? selected)
        {
            if (selected == null || selected.Length == 0) return new List<int>();
            var ids = selected.Distinct().ToList();
            return await _context.Categories.Where(c => ids.Contains(c.CategoryId)).Select(c => c.CategoryId).ToListAsync();
        }

        private async Task LoadCategoriesAsync(int[]? selected)
        {
            ViewBag.AllCategories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.SelectedCategories = new HashSet<int>(selected ?? Array.Empty<int>());
        }
    }
}
