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
    public async Task<IActionResult> Discover(string search, string filter = "home", string genre = null)
    {
        try
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            var userId = isAuthenticated ?
                int.Parse(HttpContext.Session.GetString("UserId")!) : (int?)null;
            var userName = HttpContext.Session.GetString("UserName") ?? "";
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "";
            var sessionId = HttpContext.Session.Id;

            IQueryable<Song> songQuery = _context.Song.Include(s => s.Artist).AsQueryable();
            IQueryable<Album> albumQuery = _context.Album.AsQueryable();

            // Apply genre filter if provided
            if (!string.IsNullOrEmpty(genre) && genre != "all")
            {
                songQuery = songQuery.Where(s => s.Genre != null && s.Genre.ToLower() == genre.ToLower());
            }

            // Apply filter based on sidebar selection
            switch (filter.ToLower())
            {
                case "trending-songs":
                    songQuery = songQuery.OrderByDescending(s => s.PlayCounts);
                    break;

                case "trending-albums":
                    albumQuery = albumQuery.OrderByDescending(a => a.ReleaseYear);
                    break;

                case "recently-added":
                    songQuery = songQuery.OrderByDescending(s => s.UploadeAt);
                    break;

                case "top-songs":
                    songQuery = songQuery.OrderByDescending(s => s.PlayCounts);
                    break;

                case "top-albums":
                    albumQuery = albumQuery.OrderByDescending(a => a.ReleaseYear);
                    break;

                case "home":
                default:
                    // Default: trending songs + albums
                    songQuery = songQuery.OrderByDescending(s => s.PlayCounts)
                                        .ThenByDescending(s => s.UploadeAt);
                    albumQuery = albumQuery.OrderByDescending(a => a.ReleaseYear);
                    break;
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                await SaveSearchHistory(search, userId, sessionId);
                var searchLower = search.ToLower();

                songQuery = songQuery.Where(s =>
                    (s.Title != null && s.Title.ToLower().Contains(searchLower)) ||
                    (s.Artist != null && s.Artist.Username != null &&
                     s.Artist.Username.ToLower().Contains(searchLower)) ||
                    (s.Genre != null && s.Genre.ToLower().Contains(searchLower)));

                albumQuery = albumQuery.Where(a =>
                    (a.Title != null && a.Title.ToLower().Contains(searchLower)) ||
                    (a.Artist != null && a.Artist.ToLower().Contains(searchLower)));
            }

            var songs = await songQuery.Take(12).ToListAsync();
            var albums = await albumQuery.Take(6).ToListAsync();

            // Get all unique genres for the sidebar
            var allGenres = await _context.Song
                .Where(s => s.Genre != null && s.Genre != "")
                .Select(s => s.Genre!)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            var recentSearches = await GetRecentSearches(userId, sessionId);

            var viewModel = new DiscoverViewModel
            {
                TrendingSongs = songs,
                TrendingAlbums = albums,
                RecentSearches = recentSearches,
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                UserEmail = userEmail,
                SearchTerm = search ?? "",
                CurrentFilter = filter,
                CurrentGenre = genre,
                AvailableGenres = allGenres 
            };

            ViewBag.ShowSearchBar = true;
            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.UserName = userName;
            ViewBag.UserEmail = userEmail;
            ViewBag.SearchTerm = search ?? "";
            ViewBag.CurrentFilter = filter;
            ViewBag.CurrentGenre = genre;
            ViewBag.AvailableGenres = allGenres;

            return View(viewModel);
        }
        catch (Exception ex)
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.UserName = HttpContext.Session.GetString("UserName") ?? "";
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail") ?? "";
            ViewBag.ShowSearchBar = true;
            return View(new DiscoverViewModel());
        }
    }

    // this is a TODO function 
    private async Task SaveSearchHistory(string searchTerm, int? userId, string sessionId)
    {
        try
        {

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
                    SearchType = "song", 
                    ResultCount = 0, 
                    CreatedAt = DateTime.UtcNow,
                    SessionId = userId == null ? sessionId : null
                };

                _context.SearchHistory.Add(searchHistory);
                await _context.SaveChangesAsync();
            }
            else
            {
                recentSearch.CreatedAt = DateTime.UtcNow;
                _context.SearchHistory.Update(recentSearch);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving search history: {ex.Message}");
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
