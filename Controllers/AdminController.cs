// Controllers/AdminController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicApp.Data;
using musicApp.Models;
using BCrypt.Net;

namespace musicApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Middleware to check if user is admin
        private async Task<bool> IsAdminUser()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return false;
            }

            var user = await _context.User.FindAsync(userId);
            return user?.IsAdmin == true;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            var dashboardStats = new AdminDashboardViewModel
            {
                TotalUsers = await _context.User.CountAsync(),
                TotalSongs = await _context.Song.CountAsync(),
                TotalPlaylists = await _context.Playlist.CountAsync(),
                TotalDownloads = await _context.Download.CountAsync(),
                RecentUsers = await _context.User
                    .OrderByDescending(u => u.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                RecentSongs = await _context.Song
                    .OrderByDescending(s => s.UploadeAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardStats);
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users(string search = "", int page = 1, int pageSize = 10)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            var query = _context.User.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u =>
                    u.Username.Contains(search) ||
                    u.Email.Contains(search) ||
                    u.Firstname.Contains(search) ||
                    u.Lastname.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new AdminUsersViewModel
            {
                Users = users,
                SearchTerm = search,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return View(viewModel);
        }

        // GET: Admin/UserDetails/5
        public async Task<IActionResult> UserDetails(int? id)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            // Get user statistics
            var userStats = await GetUserStatistics(id.Value);
            var viewModel = new AdminUserDetailsViewModel
            {
                User = user,
                Statistics = userStats
            };

            return View(viewModel);
        }

        public async Task<IActionResult> EditUser(int? id)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Convert User to EditUserViewModel
            var viewModel = new EditUserViewModel
            {
                UserId = user.UserId,
                Username = user.Username ?? "",
                Email = user.Email ?? "",
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Genre = user.Genre,
                IsAdmin = user.IsAdmin,
                IsActive = user.IsActive
            };

            return View(viewModel);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, EditUserViewModel viewModel)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            if (id != viewModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.User.FindAsync(id);
                    if (existingUser != null)
                    {
                        existingUser.Username = viewModel.Username;
                        existingUser.Email = viewModel.Email;
                        existingUser.Firstname = viewModel.Firstname;
                        existingUser.Lastname = viewModel.Lastname;
                        existingUser.Genre = viewModel.Genre;
                        existingUser.IsAdmin = viewModel.IsAdmin;
                        existingUser.IsActive = viewModel.IsActive;

                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = $"User '{viewModel.Username}' updated successfully!";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(viewModel.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Users));
            }
            return View(viewModel);
        }

        // GET: Admin/DeleteUser/5
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new DeleteConfirmationViewModel
            {
                Id = user.UserId,
                Name = user.Username ?? "Unknown User",
                Type = "User",
                CreatedAt = user.CreatedAt
            };

            return View(viewModel);
        }

        // GET: Admin/DeleteSong/5
        public async Task<IActionResult> DeleteSong(int? id)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .Include(s => s.Artist)
                .FirstOrDefaultAsync(m => m.SongId == id);

            if (song == null)
            {
                return NotFound();
            }

            var viewModel = new DeleteConfirmationViewModel
            {
                Id = song.SongId,
                Name = song.Title ?? "Unknown Song",
                Type = "Song",
                CreatedAt = song.UploadeAt
            };

            return View(viewModel);
        }




        // GET: Admin/Songs
        public async Task<IActionResult> Songs(string search = "", int page = 1, int pageSize = 10)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            var query = _context.Song.Include(s => s.Artist).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.Title.Contains(search) ||
                    (s.Artist != null && s.Artist.Username.Contains(search)));
            }

            var totalCount = await query.CountAsync();
            var songs = await query
                .OrderByDescending(s => s.UploadeAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new AdminSongsViewModel
            {
                Songs = songs,
                SearchTerm = search,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return View(viewModel);
        }

        // GET: Admin/SongDetails/5
        public async Task<IActionResult> SongDetails(int? id)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .Include(s => s.Artist)
                .FirstOrDefaultAsync(s => s.SongId == id);

            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Admin/DeleteSong/5
        // POST: Admin/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            try
            {
                var user = await _context.User.FindAsync(id);
                if (user != null)
                {
                    // Soft delete - just deactivate
                    user.IsActive = false;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"User '{user.Username}' has been deactivated.";
                }
                else
                {
                    TempData["ErrorMessage"] = "User not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Users");
        }

        // POST: Admin/DeleteSong/5
        [HttpPost, ActionName("DeleteSong")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSongConfirmed(int id)
        {
            if (!await IsAdminUser())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Login", "User");
            }

            try
            {
                var song = await _context.Song.FindAsync(id);
                if (song != null)
                {
                    string songTitle = song.Title ?? "Unknown Song";

                    // Delete related records first
                    var lyrics = await _context.Lyrics
                        .Where(l => l.SongId == id)
                        .ToListAsync();
                    if (lyrics.Any())
                    {
                        _context.Lyrics.RemoveRange(lyrics);
                    }

                    var downloads = await _context.Download
                        .Where(d => d.SongId == id)
                        .ToListAsync();
                    if (downloads.Any())
                    {
                        _context.Download.RemoveRange(downloads);
                    }

                    var playbacks = await _context.UserPlayback
                        .Where(up => up.SongId == id)
                        .ToListAsync();
                    if (playbacks.Any())
                    {
                        _context.UserPlayback.RemoveRange(playbacks);
                    }

                    var playlistSongs = await _context.PlaylistSong
                        .Where(ps => ps.SongId == id)
                        .ToListAsync();
                    if (playlistSongs.Any())
                    {
                        _context.PlaylistSong.RemoveRange(playlistSongs);
                    }

                    // Now delete the song
                    _context.Song.Remove(song);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Song '{songTitle}' has been deleted.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Song not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting song: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Songs");
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }


}