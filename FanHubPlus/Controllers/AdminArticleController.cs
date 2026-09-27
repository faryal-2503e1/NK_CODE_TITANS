using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;
using FanHubPlus.Helpers;
using FanHubPlus.Models;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminArticleController : Controller
    {
        private readonly FanHubContext _context;

        public AdminArticleController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminArticle
        public async Task<IActionResult> Index(string? q, string? sort, int page = 1)
        {
            var query = _context.FeaturedArticles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(a => a.Title.Contains(q) || a.Content.Contains(q));
            }

            query = sort switch
            {
                "title" => query.OrderBy(a => a.Title).ThenBy(a => a.ArticleId),
                "oldest" => query.OrderBy(a => a.PublishedAt).ThenBy(a => a.ArticleId),
                _ => query.OrderByDescending(a => a.PublishedAt).ThenBy(a => a.ArticleId)
            };

            return View(await PagedList<FeaturedArticle>.CreateAsync(query, page, 10));
        }

        // GET: AdminArticle/Featured
        public async Task<IActionResult> Featured()
        {
            var articles = await _context.FeaturedArticles.OrderByDescending(a => a.PublishedAt).Take(12).ToListAsync();
            return View(articles);
        }

        // GET: AdminArticle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.FeaturedArticles
                .FirstOrDefaultAsync(m => m.ArticleId == id);

            if (article == null) return NotFound();

            return View(article);
        }

        // GET: AdminArticle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminArticle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,ImageUrl,PublishedAt")] FeaturedArticle article, IFormFile? imageFile)
        {
            // If a file was uploaded, save it and store the local path
            if (imageFile != null && imageFile.Length > 0)
            {
                article.ImageUrl = await ImageStorage.SaveFileAsync(imageFile, "articles");
            }

            article.Content = RichText.Sanitize(article.Content);
            ValidateArticle(article);

            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: AdminArticle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.FeaturedArticles.FindAsync(id);
            if (article == null) return NotFound();

            return View(article);
        }

        // POST: AdminArticle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,Content,ImageUrl,PublishedAt")] FeaturedArticle article, IFormFile? imageFile)
        {
            if (id != article.ArticleId) return NotFound();

            // If a new file was uploaded, replace the old image
            if (imageFile != null && imageFile.Length > 0)
            {
                article.ImageUrl = await ImageStorage.SaveFileAsync(imageFile, "articles");
            }

            article.Content = RichText.Sanitize(article.Content);
            ValidateArticle(article);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeaturedArticleExists(article.ArticleId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: AdminArticle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.FeaturedArticles
                .FirstOrDefaultAsync(m => m.ArticleId == id);

            if (article == null) return NotFound();

            return View(article);
        }

        // POST: AdminArticle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.FeaturedArticles.FindAsync(id);
            if (article != null)
            {
                _context.FeaturedArticles.Remove(article);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void ValidateArticle(FeaturedArticle article)
        {
            if (string.IsNullOrWhiteSpace(RichText.ToPlainText(article.Content, 50)) && !article.Content.Contains("<img"))
            {
                ModelState.Remove(nameof(FeaturedArticle.Content));
                ModelState.AddModelError(nameof(FeaturedArticle.Content), "Write the article text.");
            }

            // Allow both local uploads (/uploads/...) and valid http/https URLs
            if (!string.IsNullOrWhiteSpace(article.ImageUrl)
                && !article.ImageUrl.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)
                && (!Uri.TryCreate(article.ImageUrl, UriKind.Absolute, out var uri)
                    || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
            {
                ModelState.AddModelError(nameof(FeaturedArticle.ImageUrl), "Enter a full http:// or https:// image link or upload a file.");
            }
        }

        private bool FeaturedArticleExists(int id)
        {
            return _context.FeaturedArticles.Any(e => e.ArticleId == id);
        }
    }
}