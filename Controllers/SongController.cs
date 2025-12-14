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


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));

            if (!isAuthenticated)
            {
                HttpContext.Session.SetString("RedirectAfterLogin", id.ToString());

                TempData["ErrorMessage"] = "Please login to listen to music";
                return RedirectToAction("Login", "User");
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdString, out int userId))
            {
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

            var user = await _context.User.FindAsync(userId);
            if (user != null)
            {
                user.TotalSecondsPlayed += song.DurationSeconds;
            }

            var existingPlayback = await _context.UserPlayback
                .FirstOrDefaultAsync(up => up.UserId == userId && up.SongId == id);

            if (existingPlayback != null)
            {
                existingPlayback.LastPlayed = DateTime.UtcNow;
            }
            else
            {
                
                var playback = new UserPlayback
                {
                    UserId = userId,
                    SongId = id,
                    LastPlayed = DateTime.UtcNow
                };
                _context.UserPlayback.Add(playback);
            }

            var lyrics = await _context.Lyrics
                .FirstOrDefaultAsync(l => l.SongId == id);

            await _context.SaveChangesAsync();

            ViewBag.IsAuthenticated = true;
            ViewBag.ArtistName = artistName;
            ViewBag.CoverPath = song.CoverPath;
            ViewBag.AudioPath = song.FilePath;
            ViewBag.Lyrics = lyrics?.LyricsText;
            ViewBag.LyricsSource = lyrics?.LyricsSource;
            ViewBag.HasLyrics = lyrics != null && !string.IsNullOrEmpty(lyrics.LyricsText);

            return View(song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Download(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Json(new { success = false, message = "Please login to download songs" });
            }

            var song = await _context.Song.FindAsync(id);
            if (song == null)
            {
                return Json(new { success = false, message = "Song not found" });
            }
            var existingDownload = await _context.Download
                .FirstOrDefaultAsync(d => d.UserId == userId && d.SongId == id);

            try
            {
                if (existingDownload == null)
                {
                    var download = new Download
                    {
                        UserId = userId,
                        SongId = id,
                        DownloadedAt = DateTime.UtcNow
                    };

                    _context.Download.Add(download);
                    await _context.SaveChangesAsync();

                    // Log activity
                    Console.WriteLine($"User {userId} downloaded song {id} at {DateTime.UtcNow}");
                }

                var filePath = song.FilePath?.Replace("/home/soufiane/Music", "/music") ?? "";
                return Json(new
                {
                    success = true,
                    message = "Song ready for download",
                    downloadUrl = Url.Action("GetSongFile", "Song", new { id = id }),
                    fileName = $"{song.Title}.mp3"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Download error: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while processing download" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSongFile(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Please login to download");
            }

            var song = await _context.Song.FindAsync(id);
            if (song == null)
            {
                return NotFound("Song not found");
            }

            var filePath = song.FilePath?.Replace("/music", "/home/soufiane/Music") ?? "";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Audio file not found on server");
            }

            var fileInfo = new FileInfo(filePath);
            var contentType = GetContentType(fileInfo.Extension);
            var fileName = $"{song.Title}{fileInfo.Extension}";

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            if (int.TryParse(userIdString, out int userId))
            {
                var hasDownloaded = await _context.Download
                    .AnyAsync(d => d.UserId == userId && d.SongId == id);

                if (!hasDownloaded)
                {
                    var download = new Download
                    {
                        UserId = userId,
                        SongId = id,
                        DownloadedAt = DateTime.UtcNow
                    };
                    _context.Download.Add(download);
                    await _context.SaveChangesAsync();
                }
            }

            return File(fileBytes, contentType, fileName);
        }

        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".mp3" => "audio/mpeg",
                ".ogg" => "audio/ogg",
                ".wav" => "audio/wav",
                ".flac" => "audio/flac",
                ".m4a" => "audio/mp4",
                _ => "application/octet-stream"
            };
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
