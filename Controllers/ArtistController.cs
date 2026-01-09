using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicApp.Data;
using musicApp.Models;

using musicApp.Controllers;


public class ArtistController : Controller
{
    private readonly AppDbContext _context;

    public ArtistController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Artist
    public async Task<IActionResult> Index()
    {
        return View(await _context.Artist.ToListAsync());
    }

    

}