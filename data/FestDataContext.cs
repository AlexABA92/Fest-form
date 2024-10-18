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
        public DbSet <Person>             Persons              { get; set; }
        public DbSet <ParticipantsNumber> ParticipantsNumbers  { get; set; }
        public DbSet <ParticipantsList>   ParticipantsLists    { get; set; }

        public FestDataContext(DbContextOptions<FestDataContext> options) : base(options)
        {
         Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
