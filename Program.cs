using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using musicApp.Data;
using Supabase;
using musicApp.Services;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSession();
builder.Services.AddControllersWithViews();

var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];

if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
{
    throw new Exception("Supabase URL and Key must be configured in appsettings.json");
}

// Register Supabase.Client with DI container
builder.Services.AddScoped<Client>(provider =>
{
    var options = new SupabaseOptions
    {
        AutoConnectRealtime = true
    };
    return new Client(supabaseUrl, supabaseKey, options);
});



builder.Services.AddScoped<ISupabaseService, SupabaseServices>();



//builder.Services.AddSingleton<SupabaseService>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
)
    .WithStaticAssets();


app.Run();

