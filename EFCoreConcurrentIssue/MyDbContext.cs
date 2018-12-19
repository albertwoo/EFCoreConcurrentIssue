using Microsoft.EntityFrameworkCore;

namespace EFCoreConcurrentIssue
{
    public class MyDbContext: DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasQueryFilter(x => x.Active);
            modelBuilder.Entity<Event>().Property(x => x.Active).HasDefaultValue(true);
            modelBuilder.Entity<Location>().HasQueryFilter(x => x.Active);
            modelBuilder.Entity<Location>().Property(x => x.Active).HasDefaultValue(true);
            base.OnModelCreating(modelBuilder);
        }
    }
}