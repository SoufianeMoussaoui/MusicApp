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
    public class DownloadController : Controller
    {
        private readonly musicDownload _context;

        public DownloadController(musicDownload context)
        {
            _context = context;
        }

        // GET: Download
        public async Task<IActionResult> Index()
        {
            return View(await _context.Download.ToListAsync());
        }

        // GET: Download/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var download = await _context.Download
                .FirstOrDefaultAsync(m => m.DownloadId == id);
            if (download == null)
            {
                return NotFound();
            }

            return View(download);
        }

        // GET: Download/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Download/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DownloadId,UserId,SongId,DownloadedAt")] Download download)
        {
            if (ModelState.IsValid)
            {
                _context.Add(download);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(download);
        }

        // GET: Download/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var download = await _context.Download.FindAsync(id);
            if (download == null)
            {
                return NotFound();
            }
            return View(download);
        }

        // POST: Download/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DownloadId,UserId,SongId,DownloadedAt")] Download download)
        {
            if (id != download.DownloadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(download);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DownloadExists(download.DownloadId))
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
            return View(download);
        }

        // GET: Download/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var download = await _context.Download
                .FirstOrDefaultAsync(m => m.DownloadId == id);
            if (download == null)
            {
                return NotFound();
            }

            return View(download);
        }

        // POST: Download/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var download = await _context.Download.FindAsync(id);
            if (download != null)
            {
                _context.Download.Remove(download);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DownloadExists(int id)
        {
            return _context.Download.Any(e => e.DownloadId == id);
        }
    }
}
