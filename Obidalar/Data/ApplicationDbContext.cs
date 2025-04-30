using Microsoft.EntityFrameworkCore;
using Obidalar.Models;

namespace Obidalar.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Obida> Obidalar { get; set; }
        public DbSet<Sharh> Sharhlar { get; set; }
    }

}
