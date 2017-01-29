using BKind.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance
{
    public class StoriesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasOne(x => x.Credential)
            //    .WithOne(x => x.)
                
        }
    }
}