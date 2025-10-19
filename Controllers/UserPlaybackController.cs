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
    public class UserPlaybackController : Controller
    {
        private readonly musicUserPlaylbackContext _context;

        public UserPlaybackController(musicUserPlaylbackContext context)
        {
            _context = context;
        }

        // GET: UserPlayback
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserPlayback.ToListAsync());
        }

        // GET: UserPlayback/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlayback = await _context.UserPlayback
                .FirstOrDefaultAsync(m => m.UserPlaybackId == id);
            if (userPlayback == null)
            {
                return NotFound();
            }

            return View(userPlayback);
        }

        // GET: UserPlayback/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserPlayback/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserPlaybackId,UserId,SongId,CurrentPosition,LastPlayed")] UserPlayback userPlayback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPlayback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userPlayback);
        }

        // GET: UserPlayback/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlayback = await _context.UserPlayback.FindAsync(id);
            if (userPlayback == null)
            {
                return NotFound();
            }
            return View(userPlayback);
        }

        // POST: UserPlayback/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserPlaybackId,UserId,SongId,CurrentPosition,LastPlayed")] UserPlayback userPlayback)
        {
            if (id != userPlayback.UserPlaybackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPlayback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPlaybackExists(userPlayback.UserPlaybackId))
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
            return View(userPlayback);
        }

        // GET: UserPlayback/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlayback = await _context.UserPlayback
                .FirstOrDefaultAsync(m => m.UserPlaybackId == id);
            if (userPlayback == null)
            {
                return NotFound();
            }

            return View(userPlayback);
        }

        // POST: UserPlayback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userPlayback = await _context.UserPlayback.FindAsync(id);
            if (userPlayback != null)
            {
                _context.UserPlayback.Remove(userPlayback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPlaybackExists(int id)
        {
            return _context.UserPlayback.Any(e => e.UserPlaybackId == id);
        }
    }
}
