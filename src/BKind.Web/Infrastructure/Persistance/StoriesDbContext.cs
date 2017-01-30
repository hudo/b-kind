using BKind.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance
{
    public class StoriesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./stories.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(x => x.Credential)
                .WithOne(x => x.User)
                .HasForeignKey<Credential>(x => x.UserId);

            modelBuilder.Entity<Credential>().ToTable("Credentials");

            modelBuilder.Entity<Visitor>().HasBaseType<Role>();
            modelBuilder.Entity<Administrator>().HasBaseType<Role>();
            modelBuilder.Entity<Author>().HasBaseType<Role>();
            modelBuilder.Entity<Reviewer>().HasBaseType<Role>();

            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

        }
    }
}