using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserFanSubmissionController : Controller
    {
        private const string ApprovedStatus = "Approved";
        private const string PendingStatus = "Pending";
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        private readonly FanHubContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserFanSubmissionController(FanHubContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Community showcase - only approved submissions from everyone
        [Route("/fan-submissions")]
        public async Task<IActionResult> Index()
        {
            var approvedSubmissions = await _context.FanSubmissions
                .Where(s => s.Status == ApprovedStatus)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();

            return View(approvedSubmissions);
        }

        [Route("/fan-submissions/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var submission = await _context.FanSubmissions
                .FirstOrDefaultAsync(s => s.SubmissionId == id);

            if (submission == null)
            {
                return NotFound();
            }

            // A pending or rejected submission is only visible to the person who submitted it
            var isOwner = submission.UserId == CurrentUserId;
            if (submission.Status != ApprovedStatus && !isOwner)
            {
                return Forbid();
            }

            return View(submission);
        }

        [Route("/fan-submissions/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/fan-submissions/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FanSubmission submission, IFormFile? mediaFile)
        {
            ModelState.Remove(nameof(FanSubmission.UserId));
            ModelState.Remove(nameof(FanSubmission.Status));
            ModelState.Remove(nameof(FanSubmission.MediaUrl));

            if (!ModelState.IsValid)
            {
                return View(submission);
            }

            if (mediaFile != null && mediaFile.Length > 0)
            {
                var extension = Path.GetExtension(mediaFile.FileName).ToLowerInvariant();

                if (!AllowedImageExtensions.Contains(extension))
                {
                    ModelState.AddModelError(string.Empty, "Only JPG, PNG or WEBP images are allowed.");
                    return View(submission);
                }

                if (mediaFile.Length > 2 * 1024 * 1024) // 2 MB limit
                {
                    ModelState.AddModelError(string.Empty, "Image must be smaller than 2 MB.");
                    return View(submission);
                }

                var galleriesFolder = Path.Combine(_environment.WebRootPath, "images", "galleries");
                Directory.CreateDirectory(galleriesFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(galleriesFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await mediaFile.CopyToAsync(stream);
                }

                submission.MediaUrl = $"/images/galleries/{uniqueFileName}";
            }

            submission.UserId = CurrentUserId;
            submission.Status = PendingStatus;
            submission.SubmittedAt = DateTime.Now;

            _context.FanSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your submission has been sent for review.";
            return RedirectToAction(nameof(MySubmissions));
        }

        // Everything the current user has ever submitted, regardless of status
        [Route("/fan-submissions/mine")]
        public async Task<IActionResult> MySubmissions()
        {
            var mySubmissions = await _context.FanSubmissions
                .Where(s => s.UserId == CurrentUserId)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();

            return View(mySubmissions);
        }
    }
}