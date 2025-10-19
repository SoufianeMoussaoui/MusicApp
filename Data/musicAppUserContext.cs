using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicAppUserContext : DbContext
    {
        public musicAppUserContext (DbContextOptions<musicAppUserContext> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.User> User { get; set; } = default!;
    }
}
