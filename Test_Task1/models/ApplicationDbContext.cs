using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace test_task.models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Founders> Founders { get; set; }
        public DbSet<Clients> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Founders>()
                .HasMany(f => f.Clients)
                .WithMany(c => c.Founders)
                .UsingEntity(j => j.ToTable("FoundersClients"));
        }
    }
}