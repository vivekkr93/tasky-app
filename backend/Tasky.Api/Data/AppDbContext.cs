using Microsoft.EntityFrameworkCore;
using Tasky.Api.Model;

namespace Tasky.Api.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Title).HasMaxLength(200).IsRequired();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }

    }
}
