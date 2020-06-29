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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            EnumToStringConverter<Roles> converter = new EnumToStringConverter<Roles>();

            builder
                .Entity<Role>()
                .Property(u => u.Name)
                .HasConversion(converter);
        }
    }
}