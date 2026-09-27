using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserShareController : Controller
    {
        private readonly FanHubContext _context;

        public UserShareController(FanHubContext context)
        {
            _context = context;
        }

        // Shows share buttons (Facebook, Twitter/X, WhatsApp, copy link) for a given piece of content[cite: 2]
        [Route("/share")]
        public IActionResult Share(string title, string url)
        {
            ViewBag.Title = title;
            ViewBag.Url = url;

            return View();
        }

        // Called quietly in the background when a share button is clicked, so we don't[cite: 2]
        // slow down the actual share action. Only logged-in users get tracked.[cite: 2]
        [Authorize]
        [HttpPost]
        [Route("/share/log")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogShare(string platform, string sharedUrl)
        {
            var share = new Share
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Platform = platform,
                SharedUrl = sharedUrl,
                SharedAt = DateTime.Now
            };

            _context.Shares.Add(share);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}