using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserQuizController : Controller
    {
        private readonly FanHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserQuizController(FanHubContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserQuiz - saari available quizzes category wise
        public async Task<IActionResult> Index()
        {
            var quizzes = await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .OrderBy(q => q.Category.Name)
                .ToListAsync();

            return View(quizzes);
        }

        // GET: UserQuiz/Take/5
        public async Task<IActionResult> Take(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.QuizId == id);

            if (quiz == null || quiz.Questions == null || quiz.Questions.Count == 0)
            {
                return NotFound();
            }

            var viewModel = new QuizTakeViewModel
            {
                QuizId = quiz.QuizId,
                QuizTitle = quiz.Title,
                Questions = quiz.Questions.Select(q => new QuizQuestionAnswerViewModel
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: UserQuiz/Take - submit hone ke baad score calculate hota hai
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Take(QuizTakeViewModel submittedQuiz)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.QuizId == submittedQuiz.QuizId);

            if (quiz == null)
            {
                return NotFound();
            }

            int correctCount = 0;

            foreach (var answered in submittedQuiz.Questions)
            {
                var originalQuestion = quiz.Questions.FirstOrDefault(q => q.QuestionId == answered.QuestionId);

                if (originalQuestion != null &&
                    !string.IsNullOrEmpty(answered.SelectedOption) &&
                    originalQuestion.CorrectOption.Equals(answered.SelectedOption, StringComparison.OrdinalIgnoreCase))
                {
                    correctCount++;
                }
            }

            int totalQuestions = quiz.Questions.Count;
            double percentage = totalQuestions > 0
                ? Math.Round((correctCount * 100.0) / totalQuestions, 2)
                : 0;

            var attempt = new QuizAttempt
            {
                QuizId = quiz.QuizId,
                UserId = _userManager.GetUserId(User),
                Score = correctCount,
                TotalQuestions = totalQuestions,
                PercentageScore = percentage
            };

            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Result), new { id = attempt.AttemptId });
        }

        // GET: UserQuiz/Result/5
        public async Task<IActionResult> Result(int id)
        {
            var attempt = await _context.QuizAttempts
                .Include(a => a.Quiz)
                .FirstOrDefaultAsync(a => a.AttemptId == id);

            if (attempt == null)
            {
                return NotFound();
            }

            return View(attempt);
        }

        // GET: UserQuiz/Leaderboard/5 - top 10 attempts, best percentage pehle
        public async Task<IActionResult> Leaderboard(int id)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            var topAttempts = await _context.QuizAttempts
                .Where(a => a.QuizId == id)
                .OrderByDescending(a => a.PercentageScore)
                .ThenBy(a => a.AttemptedAt)
                .Take(10)
                .ToListAsync();

            var userIds = topAttempts.Select(a => a.UserId).Distinct().ToList();
            var userNames = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName ?? u.UserName);

            ViewBag.QuizTitle = quiz.Title;
            ViewBag.UserNames = userNames;

            return View(topAttempts);
        }
    }
}
