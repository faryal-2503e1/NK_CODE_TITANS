using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPollController : Controller
    {
        private readonly FanHubContext _context;

        public AdminPollController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminPoll
        public async Task<IActionResult> Index()
        {
            var polls = await _context.Polls
                .Include(p => p.Category)
                .Include(p => p.Options)
                    .ThenInclude(o => o.Votes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(polls);
        }

        // GET: AdminPoll/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        // POST: AdminPoll/Create - title + comma-separated options ek sath le lete hain, simple rakhne ke liye
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string title, int? categoryId, string optionsCsv)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(optionsCsv))
            {
                ModelState.AddModelError("", "Title aur kam az kam 2 options zaroori hain.");
                ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                return View();
            }

            var optionTexts = optionsCsv
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .ToList();

            if (optionTexts.Count < 2)
            {
                ModelState.AddModelError("", "Kam az kam 2 options comma se separate karke likhein.");
                ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                return View();
            }

            var poll = new Poll
            {
                Title = title,
                CategoryId = categoryId,
                IsActive = true,
                Options = optionTexts.Select(o => new PollOption { OptionText = o }).ToList()
            };

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Poll live ho gayi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: AdminPoll/ToggleActive/5 - poll ko band/chalu karne ke liye
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null) return NotFound();

            poll.IsActive = !poll.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: AdminPoll/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var poll = await _context.Polls
                .Include(p => p.Options)
                .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(p => p.PollId == id);

            if (poll == null) return NotFound();

            foreach (var option in poll.Options)
            {
                _context.PollVotes.RemoveRange(option.Votes);
            }
            _context.PollOptions.RemoveRange(poll.Options);
            _context.Polls.Remove(poll);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
