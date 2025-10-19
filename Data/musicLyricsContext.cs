using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicLyricsContext : DbContext
    {
        public musicLyricsContext (DbContextOptions<musicLyricsContext> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.Lyrics> Lyrics { get; set; } = default!;
    }
}
