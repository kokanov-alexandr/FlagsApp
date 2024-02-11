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
        public DbSet<FlagColor> FlagColors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlagColor>()
                .HasKey(fc => new { fc.FlagId, fc.ColorId });

            modelBuilder.Entity<FlagColor>()
                .HasOne<Flag>()
                .WithMany()
                .HasForeignKey(fc => fc.FlagId);

            modelBuilder.Entity<FlagColor>()
                .HasOne<Color>()
                .WithMany()
                .HasForeignKey(fc => fc.ColorId);
        }

    }
}
