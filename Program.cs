using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using musicApp.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<musicPlaylistSong>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicPlaylistSong") ?? throw new InvalidOperationException("Connection string 'musicPlaylistSong' not found.")));
builder.Services.AddDbContext<musicUserPlaylbackContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicUserPlaylbackContext") ?? throw new InvalidOperationException("Connection string 'musicUserPlaylbackContext' not found.")));
builder.Services.AddDbContext<musicDownload>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicDownload") ?? throw new InvalidOperationException("Connection string 'musicDownload' not found.")));
builder.Services.AddDbContext<musicLyricsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicLyricsContext") ?? throw new InvalidOperationException("Connection string 'musicLyricsContext' not found.")));
builder.Services.AddDbContext<musicAppPlylistContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicAppPlylistContext") ?? throw new InvalidOperationException("Connection string 'musicAppPlylistContext' not found.")));
builder.Services.AddDbContext<musicAppArtistContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicAppArtistContext") ?? throw new InvalidOperationException("Connection string 'musicAppArtistContext' not found.")));
builder.Services.AddDbContext<musicAppSongContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicAppSongContext") ?? throw new InvalidOperationException("Connection string 'musicAppSongContext' not found.")));
builder.Services.AddDbContext<musicAppUserContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("musicAppUserContext") ?? throw new InvalidOperationException("Connection string 'musicAppUserContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
