using FlagsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FlagsApp
{
    public class FlagsDBContext : DbContext
    {
        public FlagsDBContext(DbContextOptions<FlagsDBContext> options)
            : base(options) { }

        public DbSet<Flag> Flags => Set<Flag>();
        public DbSet<Color> Colors => Set<Color>();
    }
}
