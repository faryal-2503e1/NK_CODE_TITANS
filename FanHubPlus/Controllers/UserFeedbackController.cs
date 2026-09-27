using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserFeedbackController : Controller
    {
        private const string DefaultStatus = "Open";

        private readonly FanHubContext _context;

        public UserFeedbackController(FanHubContext context)
        {
            _context = context;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // The user's own feedback history, most recent first
        [Route("/feedback")]
        public async Task<IActionResult> Index()
        {
            var myFeedback = await _context.Feedbacks
                .Where(f => f.UserId == CurrentUserId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(myFeedback);
        }

        [Route("/feedback/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.FeedbackId == id && f.UserId == CurrentUserId);

            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        [Route("/feedback/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/feedback/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            ModelState.Remove(nameof(Feedback.UserId));
            ModelState.Remove(nameof(Feedback.Status));

            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            feedback.UserId = CurrentUserId;
            feedback.Status = DefaultStatus;
            feedback.CreatedAt = DateTime.Now;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thanks for the feedback, we'll take a look.";
            return RedirectToAction(nameof(Index));
        }
    }
}