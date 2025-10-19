using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicAppSongContext : DbContext
    {
        public musicAppSongContext (DbContextOptions<musicAppSongContext> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.Song> Song { get; set; } = default!;
    }
}
