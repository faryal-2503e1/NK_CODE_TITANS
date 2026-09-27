using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    public class UserArticleController : Controller
    {
        private readonly FanHubContext _context;

        public UserArticleController(FanHubContext context)
        {
            _context = context;
        }

        [Route("/articles")]
        public async Task<IActionResult> Index()
        {
            var articles = await _context.FeaturedArticles
                .OrderByDescending(a => a.PublishedAt)
                .ToListAsync();

            return View(articles);
        }

        [Route("/articles/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _context.FeaturedArticles
                .FirstOrDefaultAsync(a => a.ArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // Spotlight on the handful of most recent articles, shown in a hero-style layout
        [Route("/articles/featured")]
        public async Task<IActionResult> Featured()
        {
            var featuredArticles = await _context.FeaturedArticles
                .OrderByDescending(a => a.PublishedAt)
                .Take(3)
                .ToListAsync();

            return View(featuredArticles);
        }

        // All articles grouped by month for a chronological, news-style timeline
        [Route("/articles/timeline")]
        public async Task<IActionResult> Timeline()
        {
            var articles = await _context.FeaturedArticles
                .OrderByDescending(a => a.PublishedAt)
                .ToListAsync();

            var groupedByMonth = articles
                .GroupBy(a => new DateTime(a.PublishedAt.Year, a.PublishedAt.Month, 1))
                .OrderByDescending(group => group.Key)
                .Select(group => (MonthLabel: group.Key.ToString("MMMM yyyy"), Articles: group.ToList()))
                .ToList();

            return View(groupedByMonth);
        }
    }
}