using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using musicApp.Models;
using Supabase;
namespace musicApp.Controllers;

using Microsoft.EntityFrameworkCore.Infrastructure;
using musicApp.Data;
using SQLitePCL;

public class HomeController : Controller
{
    private readonly ISupabaseService _supabaseService;
    private readonly Client _supabaseClient;
    private readonly AppDbContext _context;

    public HomeController(Client supabaseClient, ISupabaseService supabaseService, AppDbContext context)
    {
        _supabaseClient = supabaseClient;
        _supabaseService = supabaseService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Discover()
    {
        try
        {
            var songs = await _supabaseService.GetAllSongsAsync();
            var albums = await _supabaseService.GetAllAlbumsAsync();

            var trendingSongs = songs
                .OrderByDescending(s => s.DurationSeconds)
                .Take(8)
                .ToList();

            var trendingAlbums = albums
                .OrderByDescending(a => a.SongCount)
                .Take(6)
                .ToList();

            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            var userName = HttpContext.Session.GetString("UserName") ?? "";
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "";
            if (isAuthenticated)
            {
                
            }
            var viewModel = new DiscoverViewModel
            {
                TrendingSongs = trendingSongs,
                TrendingAlbums = trendingAlbums,
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                UserEmail = userEmail,
                UnreadNotifications = 0
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading discover page: {ex.Message}");
            return View(new DiscoverViewModel());
        }
    }


    private async Task<List<Song>> GetTrendingSongsAsync()
    {
        try
        {
            var response = await _supabaseClient
                .From<Song>()
                .Order(x => x.SongId, Postgrest.Constants.Ordering.Descending)
                .Limit(10)
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching trending songs: {ex.Message}");
            return new List<Song>();
        }
    }

    private async Task<List<Album>> GetTrendingAlbumsAsync()
    {
        try
        {
            var response = await _supabaseClient
                .From<Album>()
                .Order(x => x.AlbumId, Postgrest.Constants.Ordering.Descending)
                .Limit(6)
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching trending albums: {ex.Message}");
            return new List<Album>();
        }
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
