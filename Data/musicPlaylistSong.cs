using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicPlaylistSong : DbContext
    {
        public musicPlaylistSong (DbContextOptions<musicPlaylistSong> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.PlaylistSong> PlaylistSong { get; set; } = default!;
    }
}
