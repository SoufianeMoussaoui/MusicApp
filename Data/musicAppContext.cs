using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Album> Album { get; set; } = default!;

        public DbSet<Song> Song { get; set; } = default!;
        public DbSet<Artist> Artist { get; set; } = default!;
        public DbSet<Playlist> Playlist { get; set; } = default!;
        public DbSet<Download> Download { get; set; } = default!;
        public DbSet<Lyrics> Lyrics { get; set; } = default!;
        public DbSet<UserPlayback> UserPlayback { get; set; } = default!;
        public DbSet<PlaylistSong> PlaylistSong { get; set; } = default!;
        public DbSet<Notifications> Notifications { get; set; } = default!;
        //public DbSet<DiscoverViewModel> DiscoverViewModels { get; set; } = default!;
        public DbSet<SearchHistory> SearchHistory { get; set; }

        // public DbSet<ProfileViewModel> ProfileViewModel { get; set; } = default!;
        //public DbSet<AdminViewModels> AdminViewModels { get; set; } = default!;


    

    }
}
