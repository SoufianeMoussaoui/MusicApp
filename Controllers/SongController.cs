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
                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToPlaylist(int songId, int playlistId)
        {
            try
            {
                Console.WriteLine($"=== AddToPlaylist Called ===");
                Console.WriteLine($"SongId: {songId}, PlaylistId: {playlistId}");

                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    Console.WriteLine("User not authenticated");
                    return Json(new { success = false, message = "Please login to add songs to playlists" });
                }

                // Verify the playlist belongs to the user
                var playlist = await _context.Playlist
                    .FirstOrDefaultAsync(p => p.PlaylistId == playlistId && p.UserId == userId);

                if (playlist == null)
                {
                    Console.WriteLine($"Playlist {playlistId} not found or doesn't belong to user {userId}");
                    return Json(new { success = false, message = "Playlist not found or access denied" });
                }

                // Verify the song exists
                var song = await _context.Song.FindAsync(songId);
                if (song == null)
                {
                    Console.WriteLine($"Song {songId} not found");
                    return Json(new { success = false, message = "Song not found" });
                }

                // Check if song is already in the playlist
                var existingEntry = await _context.PlaylistSong
                    .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

                if (existingEntry != null)
                {
                    Console.WriteLine($"Song {songId} already in playlist {playlistId}");
                    return Json(new { success = false, message = $"'{song.Title}' is already in '{playlist.Name}'" });
                }

                // Add song to playlist - use CreatedAt instead of AddedAt
                var playlistSong = new PlaylistSong
                {
                    PlaylistId = playlistId,
                    SongId = songId,
                    OrderPosition = await GetNextOrderPosition(playlistId),
                    CreatedAt = DateTime.UtcNow  // Changed from AddedAt to CreatedAt
                };

                _context.PlaylistSong.Add(playlistSong);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Successfully added song {songId} to playlist {playlistId}");

                return Json(new
                {
                    success = true,
                    message = $"'{song.Title}' added to '{playlist.Name}' successfully!",
                    playlistName = playlist.Name
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding song to playlist: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        private async Task<int> GetNextOrderPosition(int playlistId)
        {
            var maxPosition = await _context.PlaylistSong
                .Where(ps => ps.PlaylistId == playlistId)
                .OrderByDescending(ps => ps.OrderPosition)
                .Select(ps => ps.OrderPosition)
                .FirstOrDefaultAsync();
            
            return maxPosition + 1;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPlaylistsForSong(int songId)
        {
            try
            {
                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Json(new { success = false, message = "Please login to view playlists" });
                }

                // Get user's playlists
                var playlists = await _context.Playlist
                    .Where(p => p.UserId == userId)
                    .OrderBy(p => p.Name)
                    .Select(p => new
                    {
                        p.PlaylistId,
                        p.Name,
                        p.Description,
                        // Get song count by querying PlaylistSong table directly
                        SongCount = _context.PlaylistSong.Count(ps => ps.PlaylistId == p.PlaylistId),
                        // Check if song exists in playlist by querying PlaylistSong table
                        HasSong = _context.PlaylistSong.Any(ps => ps.PlaylistId == p.PlaylistId && ps.SongId == songId)
                    })
                    .ToListAsync();

                return Json(new { success = true, playlists });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting playlists: {ex.Message}");
                return Json(new { success = false, message = "Error loading playlists" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickCreatePlaylist(string name, string description = "", int? songId = null)
        {
            try
            {
                Console.WriteLine($"=== QuickCreatePlaylist Called ===");
                Console.WriteLine($"Name: '{name}', Description: '{description}', SongId: {songId}");

                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    Console.WriteLine("User not authenticated");
                    return Json(new { success = false, message = "Please login to create playlists" });
                }

                if (string.IsNullOrEmpty(name?.Trim()))
                {
                    return Json(new { success = false, message = "Playlist name is required" });
                }

                // Check if playlist with same name already exists for this user
                var existingPlaylist = await _context.Playlist
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.Name.ToLower() == name.Trim().ToLower());

                if (existingPlaylist != null)
                {
                    // If songId is provided, add song to existing playlist
                    if (songId.HasValue)
                    {
                        var song = await _context.Song.FindAsync(songId.Value);
                        if (song != null)
                        {
                            // Check if song already in playlist
                            var existingEntry = await _context.PlaylistSong
                                .FirstOrDefaultAsync(ps => ps.PlaylistId == existingPlaylist.PlaylistId && ps.SongId == songId.Value);

                            if (existingEntry == null)
                            {
                                var playlistSong = new PlaylistSong
                                {
                                    PlaylistId = existingPlaylist.PlaylistId,
                                    SongId = songId.Value,
                                    OrderPosition = await GetNextOrderPosition(existingPlaylist.PlaylistId),
                                    CreatedAt = DateTime.UtcNow  // Changed from AddedAt to CreatedAt
                                };

                                _context.PlaylistSong.Add(playlistSong);
                                await _context.SaveChangesAsync();

                                return Json(new
                                {
                                    success = true,
                                    playlistId = existingPlaylist.PlaylistId,
                                    playlistName = existingPlaylist.Name,
                                    message = $"Song added to existing playlist '{existingPlaylist.Name}'"
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = false,
                                    message = $"Song already exists in playlist '{existingPlaylist.Name}'"
                                });
                            }
                        }
                    }

                    return Json(new
                    {
                        success = false,
                        message = $"Playlist '{name}' already exists"
                    });
                }

                // Create new playlist
                var playlist = new Playlist
                {
                    UserId = userId,
                    Name = name.Trim(),
                    Description = description?.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Playlist.Add(playlist);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Playlist created with ID: {playlist.PlaylistId}");

                // If songId is provided, add song to the new playlist
                if (songId.HasValue)
                {
                    var song = await _context.Song.FindAsync(songId.Value);
                    if (song != null)
                    {
                        var playlistSong = new PlaylistSong
                        {
                            PlaylistId = playlist.PlaylistId,
                            SongId = songId.Value,
                            OrderPosition = 0, // First song in new playlist
                            CreatedAt = DateTime.UtcNow  // Changed from AddedAt to CreatedAt
                        };

                        _context.PlaylistSong.Add(playlistSong);
                        await _context.SaveChangesAsync();

                        Console.WriteLine($"Song {songId.Value} added to new playlist");
                    }
                }

                return Json(new
                {
                    success = true,
                    playlistId = playlist.PlaylistId,
                    playlistName = playlist.Name,
                    message = $"Playlist '{playlist.Name}' created successfully!"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating playlist: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new
                {
                    success = false,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
