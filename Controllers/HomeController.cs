using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using musicApp.Models;

namespace musicApp.Controllers;

using Microsoft.EntityFrameworkCore;
using musicApp.Data;


public class HomeController : Controller
{
    private readonly AppDbContext _context;


    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Discover(string search)
    {
        try
        {
            // Get user info from session
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            var userId = isAuthenticated ?
                int.Parse(HttpContext.Session.GetString("UserId")!) : (int?)null;
            var userName = HttpContext.Session.GetString("UserName") ?? "";
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "";
            var sessionId = HttpContext.Session.Id;
            

            // Start with the base query
            var songQuery = _context.Song.Include(s => s.Artist).AsQueryable();
            var albumQuery = _context.Album.AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                
                ViewBag.ShowSearchBar = true;
                await SaveSearchHistory(search, userId, sessionId);

                // Convert search term to lowercase for case-insensitive comparison
                var searchLower = search.ToLower();

                // Case-insensitive search for songs
                songQuery = songQuery.Where(s =>
                    (s.Title != null && s.Title.ToLower().Contains(searchLower)) ||
                    (s.Artist != null && s.Artist.Username != null &&
                     s.Artist.Username.ToLower().Contains(searchLower)) ||
                    (s.Genre != null && s.Genre.ToLower().Contains(searchLower)));

                // Case-insensitive search for albums
                albumQuery = albumQuery.Where(a =>
                    (a.Title != null && a.Title.ToLower().Contains(searchLower)) ||
                    (a.Artist != null && a.Artist.ToLower().Contains(searchLower)));
            }

            var songs = await songQuery
                .OrderByDescending(s => s.PlayCounts)
                .ThenByDescending(s => s.UploadeAt)
                .Take(12)
                .ToListAsync();

            var albums = await albumQuery
                .OrderByDescending(a => a.ReleaseYear)
                .Take(6)
                .ToListAsync();

            // Get recent searches for this user/session
            var recentSearches = await GetRecentSearches(userId, sessionId);

            var viewModel = new DiscoverViewModel
            {
                TrendingSongs = songs,
                TrendingAlbums = albums,
                RecentSearches = recentSearches, // Add recent searches
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                UserEmail = userEmail,
                UnreadNotifications = 0,
                SearchTerm = search ?? ""
            };

            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.UserName = userName;
            ViewBag.UserEmail = userEmail;
            ViewBag.SearchTerm = search ?? "";

            return View(viewModel);
        }
        catch (Exception ex)
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.UserName = HttpContext.Session.GetString("UserName") ?? "";
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail") ?? "";
            Console.WriteLine($"Error loading discover page: {ex.Message}");
            return View(new DiscoverViewModel());
        }
    }

    private async Task SaveSearchHistory(string searchTerm, int? userId, string sessionId)
    {
        try
        {
            // Check if this exact search was done recently (within last 5 minutes)
            var recentSearch = await _context.SearchHistory
                .Where(sh => sh.SearchTerm.ToLower() == searchTerm.ToLower() &&
                             ((userId != null && sh.UserId == userId) ||
                              (userId == null && sh.SessionId == sessionId)) &&
                             sh.CreatedAt > DateTime.UtcNow.AddMinutes(-5))
                .FirstOrDefaultAsync();

            if (recentSearch == null)
            {
                var searchHistory = new SearchHistory
                {
                    UserId = userId,
                    SearchTerm = searchTerm,
                    SearchType = "song", // You can make this dynamic
                    ResultCount = 0, // We'll update this after getting results
                    CreatedAt = DateTime.UtcNow,
                    SessionId = userId == null ? sessionId : null
                };

                _context.SearchHistory.Add(searchHistory);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Update the timestamp of the recent search
                recentSearch.CreatedAt = DateTime.UtcNow;
                _context.SearchHistory.Update(recentSearch);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving search history: {ex.Message}");
            // Don't throw, just log the error
        }
    }
    private async Task<List<SearchHistory>> GetRecentSearches(int? userId, string sessionId)
    {
        try
        {
            var query = _context.SearchHistory
                .Where(sh => (userId != null && sh.UserId == userId) ||
                             (userId == null && sh.SessionId == sessionId))
                .OrderByDescending(sh => sh.CreatedAt)
                .Take(10); // Get last 10 searches

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting search history: {ex.Message}");
            return new List<SearchHistory>();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveSearch(int id)
    {
        var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
        var userId = isAuthenticated ?
            int.Parse(HttpContext.Session.GetString("UserId")!) : (int?)null;
        var sessionId = HttpContext.Session.Id;

        try
        {
            var search = await _context.SearchHistory
                .Where(sh => sh.SearchHistoryId == id &&
                             ((userId != null && sh.UserId == userId) ||
                              (userId == null && sh.SessionId == sessionId)))
                .FirstOrDefaultAsync();

            if (search != null)
            {
                _context.SearchHistory.Remove(search);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Search removed from history.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error removing search: {ex.Message}";
        }

        return RedirectToAction("Discover");
    }

    public IActionResult Index()
    {
        return RedirectToAction("Discover");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
