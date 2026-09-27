using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFeedbackController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminFeedbackController(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminFeedback
        public async Task<IActionResult> Index(string? type, string? status, string? q, string? sort, int page = 1)
        {
            var query = _context.Feedbacks.AsQueryable();

            if (!string.IsNullOrEmpty(type)) query = query.Where(f => f.Type == type);
            if (!string.IsNullOrEmpty(status)) query = query.Where(f => f.Status == status);
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(f => f.Message.Contains(q));
            }

            query = sort == "oldest"
                ? query.OrderBy(f => f.CreatedAt).ThenBy(f => f.FeedbackId)
                : query.OrderByDescending(f => f.CreatedAt).ThenBy(f => f.FeedbackId);

            var feedbacks = await PagedList<Feedback>.CreateAsync(query, page, PageSize);
            ViewBag.Users = await _userManager.GetUserNamesAsync(feedbacks.Select(f => f.UserId));

            return View(feedbacks);
        }

        // GET: AdminFeedback/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.FeedbackId == id);

            if (feedback == null) return NotFound();

            var user = await _userManager.FindByIdAsync(feedback.UserId);
            ViewBag.SubmitterName = user?.UserName ?? "Unknown User";

            return View(feedback);
        }

        // GET: AdminFeedback/UpdateStatus/5
        public async Task<IActionResult> UpdateStatus(int? id)
        {
            if (id == null) return NotFound();

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            return View(feedback);
        }

        // POST: AdminFeedback/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound();

            if (!AdminOptions.FeedbackStatuses.Contains(status))
            {
                TempData["ErrorMessage"] = "Choose a valid status.";
                return RedirectToAction(nameof(UpdateStatus), new { id });
            }

            feedback.Status = status;
            _context.Update(feedback);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Feedback marked as {status}.";
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminFeedback/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.FeedbackId == id);

            if (feedback == null) return NotFound();

            var user = await _userManager.FindByIdAsync(feedback.UserId);
            ViewBag.SubmitterName = user?.UserName ?? "Unknown User";

            return View(feedback);
        }

        // POST: AdminFeedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}