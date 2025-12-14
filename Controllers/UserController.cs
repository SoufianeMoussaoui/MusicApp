using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicApp.Data;
using musicApp.Models;
using BCrypt.Net;

namespace musicApp.Controllers;

public class UserController : Controller
{
    private readonly AppDbContext _context;
    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return RedirectToAction("Login", "User");
        }

        var user = await _context.User
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            return RedirectToAction("Login", "User");
        }

        var recentlyPlayed = await _context.UserPlayback
            .Where(up => up.UserId == userId)
            .OrderByDescending(up => up.LastPlayed)
            .Take(10)
            .Join(
                _context.Song,
                playback => playback.SongId,
                song => song.SongId,
                (playback, song) => new SongWithPlaybackInfo
                {
                    SongId = song.SongId,
                    Title = song.Title,
                    CoverPath = song.CoverPath,
                    PlayCounts = song.PlayCounts,
                    DurationSeconds = song.DurationSeconds,
                    LastPlayed = playback.LastPlayed
                }
            )
            .ToListAsync();

        var playlists = await _context.Playlist
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var downloads = await _context.Download
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.DownloadedAt)
            .Take(10)
            .ToListAsync();

        var totalPlaylists = playlists.Count;
        var totalDownloads = await _context.Download
            .CountAsync(d => d.UserId == userId);

        // Calculate total time played
        var totalSeconds = user.TotalSecondsPlayed;
        var totalHours = (int)(totalSeconds / 3600);  // Cast to int
        var totalMinutes = (int)((totalSeconds % 3600) / 60);  // Cast to int
        var remainingSeconds = (int)(totalSeconds % 60);  // Cast to int

        // Format time string
        string formattedTime;
        if (totalHours > 0)
        {
            formattedTime = $"{totalHours}h {totalMinutes}m";
        }
        else if (totalMinutes > 0)
        {
            formattedTime = $"{totalMinutes}m {remainingSeconds}s";
        }
        else
        {
            formattedTime = $"{remainingSeconds}s";
        }
        var viewModel = new ProfileViewModel
        {
            user = user,
            RecentlyPlayed = recentlyPlayed,
            Playlists = playlists,
            Downloads = downloads,
            TotalPlaylists = totalPlaylists,
            TotalDownloads = totalDownloads,
            TotalTimePlayedFormatted = formattedTime,
            TotalHoursPlayed = totalHours,
            TotalMinutesPlayed = totalMinutes
        };

        ViewBag.IsAuthenticated = true;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePlaylist(string name, string description = "")
    {
        try
        {
            Console.WriteLine("=== CreatePlaylist Called ===");

            var userIdString = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"UserId from session: {userIdString}");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                Console.WriteLine("User not authenticated");
                return Json(new { success = false, message = "Please login to create a playlist" });
            }

            Console.WriteLine($"Received name: '{name}', description: '{description}'");

            if (string.IsNullOrEmpty(name?.Trim()))
            {
                return Json(new { success = false, message = "Playlist name is required" });
            }

            var playlist = new Playlist
            {
                UserId = userId,
                Name = name.Trim(),
                Description = description?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            Console.WriteLine($"Creating playlist: UserId={playlist.UserId}, Name={playlist.Name}");

            _context.Playlist.Add(playlist);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Playlist created successfully with ID: {playlist.PlaylistId}");

            return Json(new
            {
                success = true,
                playlistId = playlist.PlaylistId,
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePlaylist([FromBody] DeletePlaylistRequest request)
    {
        try
        {
            Console.WriteLine("=== DeletePlaylist Called ===");

            var userIdString = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"UserId from session: {userIdString}");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Json(new { success = false, message = "Please login to delete playlists" });
            }

            Console.WriteLine($"Deleting playlist ID: {request?.Id}");

            if (request?.Id == null || request.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid playlist ID" });
            }

            // Find the playlist
            var playlist = await _context.Playlist
                .FirstOrDefaultAsync(p => p.PlaylistId == request.Id && p.UserId == userId);

            if (playlist == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Playlist not found or you don't have permission to delete it"
                });
            }

            // First, delete any PlaylistSong entries (if you have this table)
            var playlistSongs = await _context.PlaylistSong
                .Where(ps => ps.PlaylistId == request.Id)
                .ToListAsync();

            if (playlistSongs.Any())
            {
                _context.PlaylistSong.RemoveRange(playlistSongs);
            }

            // Then delete the playlist
            _context.Playlist.Remove(playlist);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Playlist '{playlist.Name}' deleted successfully");

            return Json(new
            {
                success = true,
                message = $"Playlist '{playlist.Name}' deleted successfully!"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting playlist: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Json(new
            {
                success = false,
                message = $"An error occurred: {ex.Message}"
            });
        }
    }

    // Add this class for the request
    public class DeletePlaylistRequest
    {
        public int Id { get; set; }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {

            TempData["ErrorMessage"] = "Please enter both username and password.";
            return View();


        }
        var user = _context.User.FirstOrDefault(u => u.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            TempData["ErrorMessage"] = "Invalid username or password. Please try again.";
            return View();
        }
        
        HttpContext.Session.SetString("UserId", user.UserId.ToString());
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Email", user.Email);
        HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
        
        if (user.IsAdmin)
        {
            return RedirectToAction("Index", "Admin");
        }


        TempData["SuccessMessage"] = $"Login successful! Welcome back {user.Firstname ?? user.Username}.";

        var redirectSongId = HttpContext.Session.GetString("RedirectAfterLogin");
        if (!string.IsNullOrEmpty(redirectSongId))
        {
            HttpContext.Session.Remove("RedirectAfterLogin");
            return RedirectToAction("Details", "Song", new { id = redirectSongId });
        }

        return RedirectToAction("Index", "Home");
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(string username, string email, string password, string firstName = null, string lastName = null, string confirmPassword = null)
    {
        // Basic validation
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            TempData["ErrorMessage"] = "Username, Email, and Password are required.";
            return View();
        }

        if (password.Length < 6)
        {
            TempData["ErrorMessage"] = "Password must be at least 6 characters";
            return View();
        }

        if (password != confirmPassword)
        {
            TempData["ErrorMessage"] = "Passwords do not match";
            return View();
        }

        try
        {
            // Check for existing user
            if (_context.User.Any(u => u.Email == email))
            {
                TempData["ErrorMessage"] = "Email already registered";
                return View();
            }

            if (_context.User.Any(u => u.Username == username))
            {
                TempData["ErrorMessage"] = "Username already taken";
                return View();
            }

            // Create new user
            var user = new User
            {
                Username = username,
                Firstname = firstName,
                Lastname = lastName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow
            };

            _context.User.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration error: {ex.Message}");
            TempData["ErrorMessage"] = "An error occurred during registration. Please try again.";
            return View();
        }
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "User");
    }

}
