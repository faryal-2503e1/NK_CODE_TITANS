using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Data;

namespace FanHubPlus.Controllers
{
    [Authorize]
    public class UserNoteController : Controller
    {
        private readonly FanHubContext _context;

        public UserNoteController(FanHubContext context)
        {
            _context = context;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [Route("/notes")]
        public async Task<IActionResult> Index()
        {
            var notes = await _context.Notes
                .Where(n => n.UserId == CurrentUserId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notes);
        }

        [Route("/notes/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteId == id && n.UserId == CurrentUserId);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [Route("/notes/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/notes/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Note note)
        {
            ModelState.Remove(nameof(Note.UserId));

            if (!ModelState.IsValid)
            {
                return View(note);
            }

            note.UserId = CurrentUserId;
            note.CreatedAt = DateTime.Now;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Note saved.";
            return RedirectToAction(nameof(Index));
        }

        [Route("/notes/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteId == id && n.UserId == CurrentUserId);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpPost]
        [Route("/notes/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Note updatedNote)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteId == id && n.UserId == CurrentUserId);

            if (note == null)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Note.UserId));

            if (!ModelState.IsValid)
            {
                return View(updatedNote);
            }

            note.Title = updatedNote.Title;
            note.Content = updatedNote.Content;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Note updated.";
            return RedirectToAction(nameof(Index));
        }

        [Route("/notes/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteId == id && n.UserId == CurrentUserId);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpPost]
        [Route("/notes/delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteId == id && n.UserId == CurrentUserId);

            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Note deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}