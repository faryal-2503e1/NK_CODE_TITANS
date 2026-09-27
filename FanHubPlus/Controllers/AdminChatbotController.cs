using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminChatbotController : Controller
    {
        private const int PageSize = 10;
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminChatbotController(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AdminChatbot (User Queries)
        public async Task<IActionResult> Index(string? q, int page = 1)
        {
            var query = _context.ChatbotQueries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(c => c.Message.Contains(q) || c.Response.Contains(q));
            }

            var queries = await PagedList<ChatbotQuery>.CreateAsync(
                query.OrderByDescending(c => c.CreatedAt).ThenBy(c => c.QueryId), page, PageSize);
            ViewBag.Users = await _userManager.GetUserNamesAsync(queries.Select(c => c.UserId));

            return View(queries);
        }

        // ==================== FAQs MANAGEMENT ====================

        // GET: AdminChatbot/FAQs
        public async Task<IActionResult> FAQs(string? q, int page = 1)
        {
            var query = _context.ChatbotFaqs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(f => f.Question.Contains(q) || f.Answer.Contains(q));
            }

            return View(await PagedList<ChatbotFaq>.CreateAsync(query.OrderBy(f => f.Question).ThenBy(f => f.FaqId), page, PageSize));
        }

        // GET: AdminChatbot/CreateFaq
        public IActionResult CreateFaq()
        {
            return View();
        }

        // POST: AdminChatbot/CreateFaq
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaq(ChatbotFaq chatbotFaq)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatbotFaq);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FAQs));
            }
            return View(chatbotFaq);
        }

        // GET: AdminChatbot/EditFaq/5
        public async Task<IActionResult> EditFaq(int? id)
        {
            if (id == null) return NotFound();

            var faq = await _context.ChatbotFaqs.FindAsync(id);
            if (faq == null) return NotFound();

            return View(faq);
        }

        // POST: AdminChatbot/EditFaq/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFaq(int id, ChatbotFaq chatbotFaq)
        {
            if (id != chatbotFaq.FaqId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatbotFaq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ChatbotFaqs.Any(e => e.FaqId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(FAQs));
            }
            return View(chatbotFaq);
        }

        // POST: AdminChatbot/DeleteFaq/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            var faq = await _context.ChatbotFaqs.FindAsync(id);
            if (faq != null)
            {
                _context.ChatbotFaqs.Remove(faq);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(FAQs));
        }

        // ==================== KNOWLEDGE BASE MANAGEMENT ====================

        // GET: AdminChatbot/KnowledgeBase
        public async Task<IActionResult> KnowledgeBase(string? q, int page = 1)
        {
            var query = _context.ChatbotKnowledgeBaseEntries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(e => e.Topic.Contains(q) || e.Content.Contains(q));
            }

            var entries = await PagedList<ChatbotKnowledgeBaseEntry>.CreateAsync(query.OrderBy(e => e.Topic).ThenBy(e => e.EntryId), page, PageSize);
            return View(entries);
        }

        // GET: AdminChatbot/CreateKnowledge
        public IActionResult CreateKnowledge()
        {
            return View();
        }

        // POST: AdminChatbot/CreateKnowledge
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateKnowledge(ChatbotKnowledgeBaseEntry entry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(KnowledgeBase));
            }
            return View(entry);
        }

        // GET: AdminChatbot/EditKnowledge/5
        public async Task<IActionResult> EditKnowledge(int? id)
        {
            if (id == null) return NotFound();

            var entry = await _context.ChatbotKnowledgeBaseEntries.FindAsync(id);
            if (entry == null) return NotFound();

            return View(entry);
        }

        // POST: AdminChatbot/EditKnowledge/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKnowledge(int id, ChatbotKnowledgeBaseEntry entry)
        {
            if (id != entry.EntryId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ChatbotKnowledgeBaseEntries.Any(e => e.EntryId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(KnowledgeBase));
            }
            return View(entry);
        }

        // POST: AdminChatbot/DeleteKnowledge/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteKnowledge(int id)
        {
            var entry = await _context.ChatbotKnowledgeBaseEntries.FindAsync(id);
            if (entry != null)
            {
                _context.ChatbotKnowledgeBaseEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(KnowledgeBase));
        }
    }
}