using Azure;
using BackEndPartWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BackEndPartWeb.Data
{
    public class BestiaryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Clasification> Clasifications { get; set; }
        public BestiaryContext(DbContextOptions<BestiaryContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Monsters)
                .WithMany(m => m.Users)
                .UsingEntity(
            l => l.HasOne(typeof(Monster)).WithMany().OnDelete(DeleteBehavior.Restrict),
            r => r.HasOne(typeof(User)).WithMany().OnDelete(DeleteBehavior.Restrict));
            modelBuilder.Entity<User>()
                .HasOne(u => u.Image)
                .WithMany();
            modelBuilder.Entity<Monster>()
                .HasOne(m => m.Image)
                .WithMany();
        }

    }
}
