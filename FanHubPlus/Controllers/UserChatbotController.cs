using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserChatbotController : Controller
    {
        private const string GuestUserId = "Guest";
        private const string FallbackResponse = "Sorry, I don't have an answer for that yet. Try browsing our FAQ or reach out through the Contact page.";

        private readonly FanHubContext _context;

        public UserChatbotController(FanHubContext context)
        {
            _context = context;
        }

        [Route("/chatbot")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/chatbot/ask")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ask(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Please type a question first.");
            }

            var response = await FindBestResponseAsync(message);

            var query = new ChatbotQuery
            {
                UserId = User.Identity?.IsAuthenticated == true
    ? User.FindFirstValue(ClaimTypes.NameIdentifier) ?? GuestUserId
    : GuestUserId,
                Message = message,
                Response = response,
                CreatedAt = DateTime.Now
            };

            _context.ChatbotQueries.Add(query);
            await _context.SaveChangesAsync();

            return Json(new { response });
        }

        [Authorize]
        [Route("/chatbot/history")]
        public async Task<IActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pastQueries = await _context.ChatbotQueries
                .Where(q => q.UserId == userId)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return View(pastQueries);
        }

        [Route("/chatbot/faq")]
        public async Task<IActionResult> FAQ()
        {
            var faqs = await _context.ChatbotFaqs.ToListAsync();
            return View(faqs);
        }

        // Keyword-based matching: the message and each FAQ/knowledge-base entry are
        // both broken into meaningful words, and whichever entry shares the most
        // words with the message wins. A user typing "where is bookmark" now matches
        // the FAQ "How do I bookmark an article, character, video, or merchandise
        // item?" through the shared word "bookmark", instead of needing an
        // (almost impossible) exact-phrase match.
        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "a", "an", "the", "is", "are", "am", "was", "were", "be", "been", "being",
            "i", "you", "your", "yours", "my", "me", "we", "our", "it", "its", "this", "that",
            "do", "does", "did", "doing", "can", "could", "will", "would", "should", "may", "might",
            "to", "of", "in", "on", "at", "for", "and", "or", "but", "with", "about", "from",
            "how", "what", "where", "when", "why", "who", "which",
            "there", "here", "up", "out", "not", "no", "yes", "so", "get", "got"
        };

        private static IReadOnlyCollection<string> ExtractKeywords(string text)
        {
            return Regex.Matches(text, @"[A-Za-z']+")
                .Select(m => m.Value.ToLowerInvariant())
                .Where(w => w.Length > 2 && !StopWords.Contains(w))
                .Distinct()
                .ToList();
        }

        private static int CountSharedKeywords(IReadOnlyCollection<string> messageKeywords, string candidateText)
        {
            var candidateKeywords = ExtractKeywords(candidateText);
            return messageKeywords.Count(w => candidateKeywords.Contains(w));
        }

        private async Task<string> FindBestResponseAsync(string message)
        {
            var messageKeywords = ExtractKeywords(message);

            if (messageKeywords.Count == 0)
            {
                return FallbackResponse;
            }

            var faqs = await _context.ChatbotFaqs.ToListAsync();
            var bestFaq = faqs
                .Select(f => new { Faq = f, Score = CountSharedKeywords(messageKeywords, f.Question) })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (bestFaq != null)
            {
                return bestFaq.Faq.Answer;
            }

            var knowledgeBase = await _context.ChatbotKnowledgeBaseEntries.ToListAsync();
            var bestEntry = knowledgeBase
                .Select(k => new { Entry = k, Score = CountSharedKeywords(messageKeywords, $"{k.Topic} {k.Content}") })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            return bestEntry != null ? bestEntry.Entry.Content : FallbackResponse;
        }
    }
}