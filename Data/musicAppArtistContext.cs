using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicAppArtistContext : DbContext
    {
        public musicAppArtistContext (DbContextOptions<musicAppArtistContext> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.Artist> Artist { get; set; } = default!;
    }
}
