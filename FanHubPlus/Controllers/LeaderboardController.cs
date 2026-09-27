using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class LeaderboardController : Controller
    {
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaderboardController(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Leaderboard - top active fans, activity ke points ke hisaab se
        // Points: 1 quiz attempt = 5, 1 poll vote = 2, 1 chat message = 1
        public async Task<IActionResult> Index()
        {
            var quizCounts = await _context.QuizAttempts
                .GroupBy(a => a.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();

            var voteCounts = await _context.PollVotes
                .GroupBy(v => v.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();

            var chatCounts = await _context.EventChatMessages
                .GroupBy(m => m.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();

            var points = new Dictionary<string, int>();

            void AddPoints(List<(string UserId, int Count)> counts, int perItem)
            {
                foreach (var (userId, count) in counts)
                {
                    points[userId] = points.GetValueOrDefault(userId) + (count * perItem);
                }
            }

            AddPoints(quizCounts.Select(x => (x.UserId, x.Count)).ToList(), 5);
            AddPoints(voteCounts.Select(x => (x.UserId, x.Count)).ToList(), 2);
            AddPoints(chatCounts.Select(x => (x.UserId, x.Count)).ToList(), 1);

            // Badge thresholds ke liye har user ka raw activity count bhi rakh lete hain
            var quizCountByUser = quizCounts.ToDictionary(x => x.UserId, x => x.Count);
            var voteCountByUser = voteCounts.ToDictionary(x => x.UserId, x => x.Count);
            var chatCountByUser = chatCounts.ToDictionary(x => x.UserId, x => x.Count);

            var topUserIds = points
                .OrderByDescending(p => p.Value)
                .Take(20)
                .ToList();

            var users = await _userManager.Users
                .Where(u => topUserIds.Select(t => t.Key).Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName ?? u.UserName);

            var leaderboard = topUserIds
                .Select(t =>
                {
                    var badges = new List<string>();

                    if (quizCountByUser.GetValueOrDefault(t.Key) >= 10)
                        badges.Add("Quiz Master");

                    if (voteCountByUser.GetValueOrDefault(t.Key) >= 5)
                        badges.Add("Super Voter");

                    if (chatCountByUser.GetValueOrDefault(t.Key) >= 1)
                        badges.Add("Social Fan");

                    return new LeaderboardEntry
                    {
                        UserId = t.Key,
                        DisplayName = users.ContainsKey(t.Key) ? users[t.Key] : "Fan",
                        Points = t.Value,
                        Badges = badges
                    };
                })
                .ToList();

            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            return View(leaderboard);
        }
    }

    public class LeaderboardEntry
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public int Points { get; set; }
        public List<string> Badges { get; set; } = new();
    }
}
