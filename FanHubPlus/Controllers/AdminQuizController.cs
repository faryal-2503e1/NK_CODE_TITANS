using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminQuizController : Controller
    {
        private readonly FanHubContext _context;

        public AdminQuizController(FanHubContext context)
        {
            _context = context;
        }

        // GET: AdminQuiz
        public async Task<IActionResult> Index()
        {
            var quizzes = await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .Include(q => q.Attempts)
                .OrderBy(q => q.Category.Name)
                .ToListAsync();

            return View(quizzes);
        }

        // GET: AdminQuiz/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        // POST: AdminQuiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Title,Description")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Quiz create ho gaya. Ab isme questions add karein.";
                return RedirectToAction(nameof(Questions), new { quizId = quiz.QuizId });
            }

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(quiz);
        }

        // GET: AdminQuiz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(quiz);
        }

        // POST: AdminQuiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizId,CategoryId,Title,Description,CreatedAt")] Quiz quiz)
        {
            if (id != quiz.QuizId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.QuizId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(quiz);
        }

        // GET: AdminQuiz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var quiz = await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .Include(q => q.Attempts)
                .FirstOrDefaultAsync(q => q.QuizId == id);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // POST: AdminQuiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .Include(q => q.Attempts)
                .FirstOrDefaultAsync(q => q.QuizId == id);

            if (quiz == null) return NotFound();

            // Pehle questions aur attempts hatane hain warna foreign key error ayega
            _context.QuizQuestions.RemoveRange(quiz.Questions);
            _context.QuizAttempts.RemoveRange(quiz.Attempts);
            _context.Quizzes.Remove(quiz);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ---------------- Questions management ----------------

        // GET: AdminQuiz/Questions/5  (5 = quizId)
        public async Task<IActionResult> Questions(int quizId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // GET: AdminQuiz/CreateQuestion/5 (5 = quizId)
        public async Task<IActionResult> CreateQuestion(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null) return NotFound();

            ViewBag.QuizTitle = quiz.Title;
            var question = new QuizQuestion { QuizId = quizId };
            return View(question);
        }

        // POST: AdminQuiz/CreateQuestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion([Bind("QuizId,QuestionText,OptionA,OptionB,OptionC,OptionD,CorrectOption")] QuizQuestion question)
        {
            if (ModelState.IsValid)
            {
                _context.QuizQuestions.Add(question);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Question add ho gaya.";
                return RedirectToAction(nameof(Questions), new { quizId = question.QuizId });
            }

            var quiz = await _context.Quizzes.FindAsync(question.QuizId);
            ViewBag.QuizTitle = quiz?.Title;
            return View(question);
        }

        // GET: AdminQuiz/EditQuestion/5
        public async Task<IActionResult> EditQuestion(int? id)
        {
            if (id == null) return NotFound();

            var question = await _context.QuizQuestions
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

            if (question == null) return NotFound();

            ViewBag.QuizTitle = question.Quiz?.Title;
            return View(question);
        }

        // POST: AdminQuiz/EditQuestion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(int id, [Bind("QuestionId,QuizId,QuestionText,OptionA,OptionB,OptionC,OptionD,CorrectOption")] QuizQuestion question)
        {
            if (id != question.QuestionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Questions), new { quizId = question.QuizId });
            }

            var quiz = await _context.Quizzes.FindAsync(question.QuizId);
            ViewBag.QuizTitle = quiz?.Title;
            return View(question);
        }

        // POST: AdminQuiz/DeleteQuestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(int id, int quizId)
        {
            var question = await _context.QuizQuestions.FindAsync(id);
            if (question != null)
            {
                _context.QuizQuestions.Remove(question);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Questions), new { quizId });
        }

        private bool QuizExists(int id) => _context.Quizzes.Any(q => q.QuizId == id);
        private bool QuestionExists(int id) => _context.QuizQuestions.Any(q => q.QuestionId == id);
    }
}
