using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Passion> Passions { get; set; }

        public DbSet<Hate> Hates { get; set; }

        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
                entity.HasCheckConstraint("CK_ValidSexValue", "\"Sex\" in ('Male', 'Female', 'Other')")
            );

            modelBuilder.Entity<User>(entity =>
                entity.HasCheckConstraint(
                    "CK_ValidSexualOrientationValue",
                    "\"SexualOrientation\" in ('Straight', 'Gay', 'Lesbian'," +
                    "'Bisexual', 'Asexual', 'Demisexual', 'Pansexual', 'Queer'," +
                    " 'Bicurious', 'Aromantic')"
                    )
                );
        }
    }
}