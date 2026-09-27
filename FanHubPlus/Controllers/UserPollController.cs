using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Hubs;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserPollController : Controller
    {
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<PollHub> _pollHub;

        public UserPollController(FanHubContext context, UserManager<ApplicationUser> userManager, IHubContext<PollHub> pollHub)
        {
            _context = context;
            _userManager = userManager;
            _pollHub = pollHub;
        }

        // GET: UserPoll - saari active polls, apna vote (agar diya hai) ke sath
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var polls = await _context.Polls
                .Where(p => p.IsActive)
                .Include(p => p.Options)
                    .ThenInclude(o => o.Votes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            // Har poll ke against yeh user pehle kis option ko vote kar chuka hai, agar kiya ho
            var myVotes = await _context.PollVotes
                .Where(v => v.UserId == userId)
                .ToDictionaryAsync(v => v.PollId, v => v.OptionId);

            ViewBag.MyVotes = myVotes;

            return View(polls);
        }

        // POST: UserPoll/Vote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int pollId, int optionId)
        {
            var userId = _userManager.GetUserId(User);

            // Ek user ek poll par sirf ek dafa vote kar sakta hai
            var existingVote = await _context.PollVotes
                .FirstOrDefaultAsync(v => v.PollId == pollId && v.UserId == userId);

            if (existingVote != null)
            {
                // Vote change karne de rahe hain - option switch
                existingVote.OptionId = optionId;
                existingVote.VotedAt = DateTime.Now;
            }
            else
            {
                _context.PollVotes.Add(new PollVote
                {
                    PollId = pollId,
                    OptionId = optionId,
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync();

            // Poll ke naye results calculate karke sab connected clients ko live bhej dete hain
            var results = await _context.PollOptions
                .Where(o => o.PollId == pollId)
                .Select(o => new
                {
                    optionId = o.OptionId,
                    optionText = o.OptionText,
                    voteCount = o.Votes.Count
                })
                .ToListAsync();

            await _pollHub.Clients.Group($"poll-{pollId}").SendAsync("ReceiveResults", pollId, results);

            return RedirectToAction(nameof(Index));
        }
    }
}
