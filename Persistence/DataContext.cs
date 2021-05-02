using Domain;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<Date> Dates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasCheckConstraint(
                    "CK_ValidSexValue",
                    "\"Sex\" in ('Male', 'Female', 'Other')"
                )
                .HasCheckConstraint(
                    "CK_ValidSexualOrientationValue",
                    "\"SexualOrientation\" in ('Straight', 'Gay', 'Lesbian'," +
                    "'Bisexual', 'Transgender', 'Queer')"
                ).HasCheckConstraint(
                    "CK_ValidAgeValue",
                    "\"Age\" >= 16 and \"Age\" <= 100"
                );

            modelBuilder.Entity<User>().HasIndex(
                user => user.Sex
            );

            modelBuilder.Entity<User>().HasIndex(
                user => user.SexualOrientation
            );

            modelBuilder.Entity<User>().HasIndex(
                user => user.FieldOfStudy
            );

            modelBuilder.Entity<User>().HasIndex(
                user => user.YearOfStudy
            );
            
            modelBuilder.Entity<User>().HasIndex(
                user => user.Age
            );

            modelBuilder.Entity<Match>()
                .HasAlternateKey(match => new {match.UserId, match.OtherId});
            
            modelBuilder.Entity<Match>()
                .HasIndex(match => new {match.OtherId, match.Status});

            modelBuilder.Entity<Date>()
                .HasAlternateKey(date => new {date.UserId, date.OtherId});
        }
    }
}