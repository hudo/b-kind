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
        public DbSet<News> Newses { get; set; }

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(x => x.Id);

            builder.Entity<Visitor>().HasBaseType<Role>();
            builder.Entity<Administrator>().HasBaseType<Role>();
            builder.Entity<Reviewer>().HasBaseType<Role>();

            builder.Entity<Author>().HasBaseType<Role>()
                .HasMany(x => x.Stories)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);

            builder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.Entity<StoryVotes>()
                .HasOne(x => x.Story)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StoryVotes>()
                .HasOne(x => x.User)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StoryTags>()
                .HasOne(x => x.Story)
                .WithMany(x => x.StoryTags);

            builder.Entity<StoryTags>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.StoryTags);

            builder.Entity<StoryTags>()
                .HasKey(x => new { x.StoryId, x.TagId });

            builder.Entity<User>()
                .Property(x => x.Nickname).IsRequired().HasDefaultValue(string.Empty);

            builder.Entity<Story>()
                .HasIndex(x => x.Slug).IsUnique();

            builder.Entity<Page>().HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy);
            builder.Entity<Page>().HasOne(x => x.ModifiedByUser).WithMany().HasForeignKey(x => x.ModifiedBy);
            builder.Entity<Page>().Property(x => x.Title).IsRequired();
            builder.Entity<Page>().Property(x => x.Slug).IsRequired();
            builder.Entity<Page>().HasIndex(x => x.Slug);

            builder.Entity<News>().Property(x => x.Title).IsRequired();
            builder.Entity<News>().Property(x => x.Content).IsRequired();
            builder.Entity<News>().Property(x => x.Slug).IsRequired();
            builder.Entity<News>().ToTable("News").HasIndex(x => x.Published);
            builder.Entity<News>().HasIndex(x => x.Slug).IsUnique(true);

        }
    }
}