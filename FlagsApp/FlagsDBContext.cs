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
        public DbSet<Lines> Lines { get; set; }
        public DbSet<FlagLines> FlagLines { get; set; }
        public DbSet<GraphicElement> GraphicElements {  get; set; } 


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

            modelBuilder.Entity<FlagLines>()
                .HasKey(fl => new { fl.FlagId, fl.LinesId });

            modelBuilder.Entity<FlagLines>()
                .HasOne<Flag>()
                .WithMany()
                .HasForeignKey(fl => fl.FlagId);

            modelBuilder.Entity<FlagLines>()
                .HasOne<Lines>()
                .WithMany()
                .HasForeignKey(fl => fl.LinesId);
        }

    }
}
