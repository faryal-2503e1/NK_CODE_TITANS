using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFanSubmissionController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminFanSubmissionController(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminFanSubmission
        public async Task<IActionResult> Index(string? status, string? q, string? sort, int page = 1)
        {
            var query = _context.FanSubmissions.AsQueryable();

            if (!string.IsNullOrEmpty(status)) query = query.Where(s => s.Status == status);
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(s => s.Title.Contains(q) || s.Content.Contains(q));
            }

            query = sort == "oldest"
                ? query.OrderBy(s => s.SubmittedAt).ThenBy(s => s.SubmissionId)
                : query.OrderByDescending(s => s.SubmittedAt).ThenBy(s => s.SubmissionId);

            var submissions = await PagedList<FanSubmission>.CreateAsync(query, page, PageSize);
            ViewBag.Users = await _userManager.GetUserNamesAsync(submissions.Select(s => s.UserId));

            return View(submissions);
        }

        // GET: AdminFanSubmission/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var submission = await _context.FanSubmissions
                .FirstOrDefaultAsync(m => m.SubmissionId == id);

            if (submission == null) return NotFound();

            var user = await _userManager.FindByIdAsync(submission.UserId);
            ViewBag.SubmitterName = user?.UserName ?? "Unknown User";

            return View(submission);
        }

        // GET: AdminFanSubmission/Review/5
        public async Task<IActionResult> Review(int? id)
        {
            if (id == null) return NotFound();

            var submission = await _context.FanSubmissions.FindAsync(id);
            if (submission == null) return NotFound();

            return View(submission);
        }

        // GET: AdminFanSubmission/Approve/5
        public async Task<IActionResult> Approve(int? id)
        {
            return await ConfirmAsync(id);
        }

        // GET: AdminFanSubmission/Reject/5
        public async Task<IActionResult> Reject(int? id)
        {
            return await ConfirmAsync(id);
        }

        // POST: AdminFanSubmission/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            return await SetStatusAsync(id, "Approved");
        }

        // POST: AdminFanSubmission/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            return await SetStatusAsync(id, "Rejected");
        }

        // Shows the confirmation page (Approve.cshtml / Reject.cshtml) for a submission that is still pending
        private async Task<IActionResult> ConfirmAsync(int? id)
        {
            if (id == null) return NotFound();

            var submission = await _context.FanSubmissions.FindAsync(id);
            if (submission == null) return NotFound();

            if (submission.Status != "Pending")
            {
                TempData["ErrorMessage"] = $"\"{submission.Title}\" has already been {submission.Status.ToLower()}.";
                return RedirectToAction(nameof(Index));
            }

            return View(submission);
        }

        // Only a pending submission can be decided, so a double click or a stale tab cannot flip an earlier decision
        private async Task<IActionResult> SetStatusAsync(int id, string newStatus)
        {
            var submission = await _context.FanSubmissions.FindAsync(id);
            if (submission == null) return NotFound();

            if (submission.Status != "Pending")
            {
                TempData["ErrorMessage"] = $"\"{submission.Title}\" has already been {submission.Status.ToLower()}.";
                return RedirectToAction(nameof(Index));
            }

            submission.Status = newStatus;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"\"{submission.Title}\" was {newStatus.ToLower()}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
