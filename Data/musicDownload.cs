using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicApp.Models;

namespace musicApp.Data
{
    public class musicDownload : DbContext
    {
        public musicDownload (DbContextOptions<musicDownload> options)
            : base(options)
        {
        }

        public DbSet<musicApp.Models.Download> Download { get; set; } = default!;
    }
}
