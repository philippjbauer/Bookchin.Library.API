using System;
using System.Linq;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookchin.Library.API.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        private static ILoggerFactory ContextLoggerFactory
            => LoggerFactory.Create(b => b.AddConsole().AddFilter("", LogLevel.Information));

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=./Data/BookchinLibrary.db")
                .UseLoggerFactory(ContextLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Individual>();
            builder.Entity<Organization>();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            var newEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Added &&
                    x.Entity != null &&
                    x.Entity as ITimeStamped != null
                )
                .Select(x => x.Entity as ITimeStamped);

            var modifiedEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Modified &&
                    x.Entity != null &&
                    x.Entity as ITimeStamped != null
                )
                .Select(x => x.Entity as ITimeStamped);

            foreach (var newEntity in newEntities)
            {
                newEntity.CreatedAt = utcNow;
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                modifiedEntity.UpdatedAt = utcNow;
            }

            return base.SaveChanges();
        }
    }
}