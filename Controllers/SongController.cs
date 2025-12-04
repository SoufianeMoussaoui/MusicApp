using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicApp.Data;
using musicApp.Models;
using NuGet.Protocol.Plugins;

namespace musicApp.Controllers
{
    public class SongController : Controller
    {
        private readonly AppDbContext _context;

        public SongController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Details(int id)
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));

            if (!isAuthenticated)
            {
                HttpContext.Session.SetString("RedirectAfterLogin", id.ToString());

                TempData["ErrorMessage"] = "Please login to listen to music";
                return RedirectToAction("Login", "User");
            }
            var song = await _context.Song
                .FirstOrDefaultAsync(s => s.SongId == id);
            if (song == null)
            {
                return NotFound();
            }

            var artistName = await GetArtistName(song.ArtistId);
            song.PlayCounts++;

            await _context.SaveChangesAsync();


            ViewBag.ArtistName = artistName;
            ViewBag.CoverPath = song.CoverPath;
            ViewBag.AudioPath = song.FilePath;

            return View(song);
        }


        private async Task<string> GetArtistName(int? artistId)
        {
            var artist = await _context.Artist
                .Where(a => a.ArtistId == artistId)
                .FirstOrDefaultAsync();

            return artist.Username ?? "Unknown Artist";
        }



        private bool SongExists(int id)
        {
            return _context.Song.Any(e => e.SongId == id);
        }
    }
}
