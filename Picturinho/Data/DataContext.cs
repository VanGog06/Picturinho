using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Picturinho.Entities;
using Picturinho.Entities.Enums;

namespace Picturinho.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<UserLike> UserLikes { get; set; }

        public DbSet<UserLove> UserLoves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            EnumToStringConverter<Roles> converter = new EnumToStringConverter<Roles>();

            builder
                .Entity<Role>()
                .Property(u => u.Name)
                .HasConversion(converter);

            builder
                .Entity<UserLike>()
                .HasKey(ul => new { ul.AlbumId, ul.UserId });

            builder
                .Entity<UserLike>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<UserLike>()
                .HasOne(ul => ul.Album)
                .WithMany(i => i.Likes)
                .HasForeignKey(ul => ul.AlbumId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<UserLove>()
                .HasKey(ul => new { ul.UserId, ul.AlbumId });

            builder
                .Entity<UserLove>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.Loves)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<UserLove>()
                .HasOne(ul => ul.Album)
                .WithMany(i => i.Loves)
                .HasForeignKey(ul => ul.AlbumId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}