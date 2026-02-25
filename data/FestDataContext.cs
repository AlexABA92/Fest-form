using Fest_form.data.Entity;

using Microsoft.EntityFrameworkCore;

namespace Fest_form.data
{
    public class FestDataContext : DbContext
    {
        public DbSet<DanceTeam>           DanceTeams           { get; set; }
        public DbSet<Performance>         Performances         { get; set; }
        public DbSet <Category>           Categories           { get; set; }
        public DbSet <Genre>              Genres               { get; set; }
        public DbSet <ParticipantsNumber> ParticipantsNumbers  { get; set; }
        public DbSet <Person>             Persons              { get; set; }
        public DbSet <ParticipantsList>   ParticipantsLists    { get; set; }

        public FestDataContext(DbContextOptions<FestDataContext> options) : base(options)
        {
         Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanceTeam>(e =>
            {
                e.ToTable("DanceTeams");
                e.HasKey(x => x.TeamId);

                e.Property(x => x.TeamName).IsRequired().HasMaxLength(200);
                e.Property(x => x.Mail).IsRequired().HasMaxLength(200);
                e.Property(x => x.TeamPhoneNumber).IsRequired().HasMaxLength(50);
                e.Property(x => x.Organization).HasMaxLength(200);

                // TeamLeader: many teams -> one person
                e.HasOne(x => x.TeamLeader)
                 .WithMany(p => p.LedTeams)
                 .HasForeignKey(x => x.TeamLeaderId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Team -> Performances
                e.HasMany(x => x.Performances)
                 .WithOne(p => p.DanceTeam)
                 .HasForeignKey(p => p.DanceTeamId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Performance>(e =>
            {
                e.ToTable("Performances");
                e.HasKey(x => x.PerformanceId);

                e.Property(x => x.PerformanceName).IsRequired().HasMaxLength(250);
                e.Property(x => x.PerformanceTime).IsRequired().HasMaxLength(50);

                e.Property(x => x.PhonogramFileURL).HasMaxLength(500);
                e.Property(x => x.YouTubeVideoURL).HasMaxLength(500);

                e.HasOne(x => x.ChoreographerDirector)
                 .WithMany(p => p.DirectedPerformances)
                 .HasForeignKey(x => x.ChoreographerDirectorId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Concertmaster)
                 .WithMany(p => p.ConcertmasterPerformances)
                 .HasForeignKey(x => x.ConcertmasterId)
                 .OnDelete(DeleteBehavior.Restrict);

              
                e.HasOne(x => x.Category)
                 .WithMany(c => c.Performances)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Genre)
                 .WithMany(g => g.Performances)
                 .HasForeignKey(x => x.GenreId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.ParticipantsNumber)
                 .WithMany(pn => pn.Performances)
                 .HasForeignKey(x => x.ParticipantsNumberId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.ParticipantsNameList)
                 .WithMany(pl => pl.Performances)
                 .HasForeignKey(x => x.ParticipantsNameListId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(x => x.DanceTeamId);
                e.HasIndex(x => x.GenreId);
                e.HasIndex(x => x.CategoryId);
                e.HasIndex(x => x.ParticipantsNumberId);
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
             
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Description).IsRequired().HasMaxLength(500);
            });
            modelBuilder.Entity<Genre>(e =>
            {
                e.ToTable("Genres");
                e.HasKey(x => x.Id);

                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<ParticipantsNumber>(e =>
            {
                e.ToTable("ParticipantsNumbers");
                e.HasKey(x => x.Id);

                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<Person>(e =>
            {
                e.ToTable("People");
                e.HasKey(x => x.PersonId);

                e.Property(x => x.PersonName).IsRequired().HasMaxLength(120);
                e.Property(x => x.PersonLastName).IsRequired().HasMaxLength(120);
                e.Property(x => x.PersonFatherName).HasMaxLength(120);

                // часто удобно уникальность (по желанию):
                e.HasIndex(x => new { x.PersonName, x.PersonLastName, x.PersonFatherName });
            });
            modelBuilder.Entity<ParticipantsList>(e =>
            {
                e.ToTable("ParticipantsLists");
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Person1)
                 .WithMany()
                 .HasForeignKey(x => x.Person1Id)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(x => x.Person2)
                 .WithMany()
                 .HasForeignKey(x => x.Person2Id)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(x => x.Person3)
                 .WithMany()
                 .HasForeignKey(x => x.Person3Id)
                 .OnDelete(DeleteBehavior.NoAction);

                e.HasIndex(x => x.Person1Id);
                e.HasIndex(x => x.Person2Id);
                e.HasIndex(x => x.Person3Id);
            });
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = -1,
                    Name = "group1",
                    Description = "DescriptionGroup1"
                },
                 new Category()
                 {
                    Id = -2,
                    Name = "group2",
                    Description = "DescriptionGroup2"
                 },
                new Category()
                {
                    Id = -3,
                    Name = "group3",
                    Description = "DescriptionGroup3"
                },
                new Category()
                {
                    Id = -4,
                    Name = "group4",
                    Description = "DescriptionGroup4"
                },
                new Category()
                {
                    Id = -5,
                    Name = "group5",
                    Description = "DescriptionGroup5"
                }
                );
            modelBuilder.Entity<ParticipantsNumber>().HasData(
                new ParticipantsNumber() {
                    Id = -1,
                    Name = "Solo"
                },
                new ParticipantsNumber()
                {
                    Id = -2,
                    Name = "Duo"
                },
                new ParticipantsNumber()
                {
                    Id = -3,
                    Name = "Trio"
                },
                new ParticipantsNumber()
                {
                    Id = -4,
                    Name = "Ensemble(Small)"
                },
                new ParticipantsNumber()
                {
                    Id = -5,
                    Name = "Ensemble(Bigg)"
                }
                );
            modelBuilder.Entity<Genre>().HasData(
                   new Genre()
                   {
                       Id = -1,
                       Name = "Classic"
                   },
                   new Genre()
                   {
                       Id = -2,
                       Name = "Contemporary/Modern"
                   },
                   new Genre()
                   {
                       Id = -3,
                       Name = "Folk"
                   }
                );
        }
    }
}
