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
    public async Task<IActionResult> Discover()
    {
        try
        {
            var songs = await _context.Song
            .Include(s => s.AlbumId) 
            .OrderByDescending(s => s.PlayCounts) 
            .ThenByDescending(s => s.UploadeAt) 
            .Take(12)
            .ToListAsync();

            var albums = await _context.Album
                .Include(a => a.SongId)
                .OrderByDescending(a => a.ReleaseYear)
                .Take(6)
                .ToListAsync();

            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            var userName = HttpContext.Session.GetString("UserName") ?? "";
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "";
        
            var unreadNotifications = 0;
            /*
            if (isAuthenticated)
            {
                var userid = HttpContext.Session.GetString("UserId");
                var response = await _context
                    .From<Notifications>()
                    
               
            }
            */

            var viewModel = new DiscoverViewModel
            {
                TrendingSongs = songs,
                TrendingAlbums = albums,
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                UserEmail = userEmail,
                UnreadNotifications = unreadNotifications
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading discover page: {ex.Message}");
            return View(new DiscoverViewModel());
        }
    }


    //private DeleTeNotification(){}

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
