using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicUserPlaylbackContext : DbContext
    {
        public musicUserPlaylbackContext (DbContextOptions<musicUserPlaylbackContext> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.UserPlayback> UserPlayback { get; set; } = default!;
    }
}
