using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using musicApp.Models;
using Supabase;
namespace musicApp.Controllers;

using Microsoft.EntityFrameworkCore.Infrastructure;
using musicApp.Data;
using musicApp.Services;

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

            var trendingSongs = songs
                .OrderByDescending(s => s.DurationSeconds)
                .Take(8)
                .ToList();

            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));
            var userName = HttpContext.Session.GetString("UserName") ?? "";
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "";
            var nbNotif = 0;
            if (isAuthenticated)
            {
                var UserId = HttpContext.Session.GetString("UserId");
                var user = await _context.User.FindAsync(UserId);
                nbNotif = user.CountAllNotifications();
            }
            var viewModel = new DiscoverViewModel
            {
                TrendingSongs = trendingSongs,
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                UserEmail = userEmail,
                UnreadNotifications = nbNotif
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading discover page: {ex.Message}");
            return View(new DiscoverViewModel());
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
