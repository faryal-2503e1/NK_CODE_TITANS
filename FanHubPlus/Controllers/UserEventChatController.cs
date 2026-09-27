using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserEventChatController : Controller
    {
        private readonly FanHubContext _context;

        public UserEventChatController(FanHubContext context)
        {
            _context = context;
        }

        // GET: UserEventChat/Chat/5 (5 = eventId)
        public async Task<IActionResult> Chat(int id)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);
            if (eventItem == null)
            {
                return NotFound();
            }

            // Last 50 messages history ke tor par load kar dete hain (naye ordered ascending taake chat top-to-bottom sahi dikhe)
            var recentMessages = await _context.EventChatMessages
                .Where(m => m.EventId == id)
                .OrderByDescending(m => m.SentAt)
                .Take(50)
                .ToListAsync();

            recentMessages.Reverse();

            ViewBag.Event = eventItem;
            return View(recentMessages);
        }
    }
}
