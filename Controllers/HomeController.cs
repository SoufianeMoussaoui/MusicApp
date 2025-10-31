using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using musicApp.Models;

namespace musicApp.Controllers;

public class HomeController : Controller
{
    private readonly ISupabaseService _supabaseService;

    public HomeController(ISupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    [HttpGet]
    public async Task<IActionResult> Discover()
    {
        try
        {
            var songs = await _supabaseService.GetAllSongsAsync();
            
            if (songs == null || !songs.Any())
            {
                ViewBag.Message = "No songs for now. try later";
            }
            else
            {
                ViewBag.Message = $"Found {songs.Count} songs!";
            }

            return View(songs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading songs: {ex.Message}");
            ViewBag.Error = "We're having trouble loading the music library. Please try again in a few moments.";
            return View(new List<Song>());
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
