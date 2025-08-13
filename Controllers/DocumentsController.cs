using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTextEditor.Data;
using OnlineTextEditor.Models;

namespace OnlineTextEditor.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var docs = await _context.Documents.OrderByDescending(d => d.CreatedAt).ToListAsync();
            return View(docs);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return View(new Document());
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return NotFound();
            return View(doc);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Document doc)
        {
            if (!ModelState.IsValid) return View(doc);

            if (doc.Id == 0)
            {
                _context.Documents.Add(doc);
            }
            else
            {
                doc.UpdatedAt = DateTime.Now;
                _context.Documents.Update(doc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc != null)
            {
                _context.Documents.Remove(doc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Documents/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return NotFound();

            var bytes = System.Text.Encoding.UTF8.GetBytes(doc.Content);
            return File(bytes, "text/plain", $"{doc.Title}.txt");
        }
    }
}
