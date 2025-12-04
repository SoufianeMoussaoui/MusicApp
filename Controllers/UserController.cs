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

        var recentlyPlayed = await _context.Song
            .Where(s => _context.UserPlayback
                .Where(up => up.UserId == userId)
                .OrderByDescending(up => up.LastPlayed)
                .Select(up => up.SongId)
                .Contains(s.SongId))
            .Take(10)
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

        var totalPlays = await _context.UserPlayback
            .CountAsync(up => up.UserId == userId);

        var totalPlaylists = playlists.Count;
        var totalDownloads = await _context.Download
            .CountAsync(d => d.UserId == userId);

        var viewModel = new ProfileViewModel
        {
            user = user,
            RecentlyPlayed = recentlyPlayed,
            Playlists = playlists,
            Downloads = downloads,
            TotalPlays = totalPlays,
            TotalPlaylists = totalPlaylists,
            TotalDownloads = totalDownloads,
            IsAuthenticated = true
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlaylist(string name, string description = "")
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Json(new { success = false, message = "Not authenticated" });
        }

        if (string.IsNullOrEmpty(name))
        {
            return Json(new { success = false, message = "Playlist name is required" });
        }

        var playlist = new Playlist
        {
            UserId = userId,
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Playlist.Add(playlist);
        await _context.SaveChangesAsync();

        return Json(new { success = true, playlistId = playlist.PlaylistId });
    }

    [HttpPost]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Json(new { success = false, message = "Not authenticated" });
        }

        var playlist = await _context.Playlist
            .FirstOrDefaultAsync(p => p.PlaylistId == id && p.UserId == userId);

        if (playlist == null)
        {
            return Json(new { success = false, message = "Playlist not found" });
        }

        _context.Playlist.Remove(playlist);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
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
    public IActionResult Register(User model)
    {
        if (ModelState.IsValid)
        {
            if (_context.User.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(model);
            }

            if (_context.User.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already taken");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash),
                CreatedAt = DateTime.UtcNow
            };

            _context.User.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");

        }

        return View(model);
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

}
