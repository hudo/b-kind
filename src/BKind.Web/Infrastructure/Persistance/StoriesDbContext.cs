using System.Threading.Tasks;
using BKind.Web.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BKind.Web.Infrastructure.Persistance
{
    public class StoriesDbContext : DbContext
    {
        private IDbContextTransaction _tx;

        public StoriesDbContext(DbContextOptions<StoriesDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryTags> StoryTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Page> Pages { get; set; }

        public void BeginTransaction()
        {
            _tx = Database.BeginTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                _tx?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                if (_tx != null)
                {
                    _tx.Dispose();
                    _tx = null;
                }
            }
        }

        public void Rollback()
        {
            try
            {
                _tx?.Rollback();
            }
            finally 
            {
                if (_tx != null)
                {
                    _tx.Dispose();
                    _tx = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<Visitor>().HasBaseType<Role>();
            modelBuilder.Entity<Administrator>().HasBaseType<Role>();
            modelBuilder.Entity<Reviewer>().HasBaseType<Role>();

            modelBuilder.Entity<Author>().HasBaseType<Role>()
                .HasMany(x => x.Stories)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<StoryVotes>()
                .HasOne(x => x.Story)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StoryVotes>()
                .HasOne(x => x.User)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StoryTags>()
                .HasOne(x => x.Story)
                .WithMany(x => x.StoryTags);

            modelBuilder.Entity<StoryTags>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.StoryTags);

            modelBuilder.Entity<StoryTags>()
                .HasKey(x => new { x.StoryId, x.TagId });

            modelBuilder.Entity<User>()
                .Property(x => x.Nickname).IsRequired().HasDefaultValue(string.Empty);

            modelBuilder.Entity<Story>()
                .HasIndex(x => x.Slug).IsUnique();

            modelBuilder.Entity<Page>()
                .HasOne(x => x.CreatedByUser).WithMany()
                .HasForeignKey(x => x.CreatedBy);

            modelBuilder.Entity<Page>()
                .HasOne(x => x.ModifiedByUser).WithMany()
                .HasForeignKey(x => x.ModifiedBy);

            modelBuilder.Entity<Page>().Property(x => x.Title).IsRequired();
            modelBuilder.Entity<Page>().Property(x => x.Slug).IsRequired();
            modelBuilder.Entity<Page>().HasIndex(x => x.Slug);
        }
    }
}