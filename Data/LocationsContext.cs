using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using System.Windows.UI.Xaml; // Was changed from Windows.UI.Xaml;

namespace PizzaMaster
{
    public class LocationsContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }

        public string DbPath { get; }
        public LocationsContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=Pizza.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Location>()
                .HasMany(l => l.Employees)
                .WithOne(e => e.Location)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Location>()
               .HasMany(l => l.Products)
               .WithOne(pr => pr.Location)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
