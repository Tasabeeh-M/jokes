using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MywebApp.Data;
using MywebApp.Models;

namespace MywebApp.Controllers
{
    // Primary constructor: ApplicationDbContext is injected directly into the class
    public class JokesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Jokes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jokes.ToListAsync());
        }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var jokes = await _context.Jokes
            .Include(j => j.Comments)
            .FirstOrDefaultAsync(m => m.Id == id);      

            if (jokes is null) return NotFound();

            return View(jokes);
        }

        // GET: Jokes/Create
        [Authorize]
        public IActionResult Create() => View();

        // POST: Jokes/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JokesQuestion,JokesAnswer")] Jokes jokes)
        {
            if (!ModelState.IsValid) return View(jokes);

            _context.Add(jokes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Jokes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var jokes = await _context.Jokes.FindAsync(id);
            if (jokes is null) return NotFound();

            return View(jokes);
        }

        // POST: Jokes/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokesQuestion,JokesAnswer")] Jokes jokes)
        {
            if (id != jokes.Id) return NotFound();

            if (!ModelState.IsValid) return View(jokes);

            try
            {
                _context.Update(jokes);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JokesExists(jokes.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Jokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var jokes = await _context.Jokes.FirstOrDefaultAsync(m => m.Id == id);
            if (jokes is null) return NotFound();

            return View(jokes);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jokes = await _context.Jokes.FindAsync(id);
            if (jokes != null) _context.Jokes.Remove(jokes);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Jokes/ShowSearchForm
        public IActionResult ShowSearchForm() => View();

        // POST: Jokes/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            var results = await _context.Jokes
                .Where(j => j.JokesQuestion.Contains(searchPhrase))
                .ToListAsync();

            return View("Index", results);
        }

        private bool JokesExists(int id) => _context.Jokes.Any(e => e.Id == id);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int jokeId, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var comment = new Comment
                {
                    JokeId = jokeId,
                    Content = content,
                    CreatedAt = DateTime.Now
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = jokeId });
        }
    }
}