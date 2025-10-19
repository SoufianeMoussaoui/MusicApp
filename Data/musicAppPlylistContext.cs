using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicAppPlylistContext : DbContext
    {
        public musicAppPlylistContext (DbContextOptions<musicAppPlylistContext> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.Playlist> Playlist { get; set; } = default!;
    }
}
