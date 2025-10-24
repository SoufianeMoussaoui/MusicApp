using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicApp.Data;
using musicApp.Models;

namespace musicApp.Controllers
{
    public class LyricsController : Controller
    {
        private readonly AppDbContext _context;

        public LyricsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lyrics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lyrics.ToListAsync());
        }

        // GET: Lyrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lyrics = await _context.Lyrics
                .FirstOrDefaultAsync(m => m.LyricsId == id);
            if (lyrics == null)
            {
                return NotFound();
            }

            return View(lyrics);
        }

        // GET: Lyrics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lyrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LyricsId,SongId,LyricsText,LyricsSource,AddedAt")] Lyrics lyrics)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lyrics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lyrics);
        }

        // GET: Lyrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lyrics = await _context.Lyrics.FindAsync(id);
            if (lyrics == null)
            {
                return NotFound();
            }
            return View(lyrics);
        }

        // POST: Lyrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LyricsId,SongId,LyricsText,LyricsSource,AddedAt")] Lyrics lyrics)
        {
            if (id != lyrics.LyricsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lyrics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LyricsExists(lyrics.LyricsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lyrics);
        }

        // GET: Lyrics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lyrics = await _context.Lyrics
                .FirstOrDefaultAsync(m => m.LyricsId == id);
            if (lyrics == null)
            {
                return NotFound();
            }

            return View(lyrics);
        }

        // POST: Lyrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lyrics = await _context.Lyrics.FindAsync(id);
            if (lyrics != null)
            {
                _context.Lyrics.Remove(lyrics);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LyricsExists(int id)
        {
            return _context.Lyrics.Any(e => e.LyricsId == id);
        }
    }
}
