using Fest_form.data.Entity;

using Microsoft.EntityFrameworkCore;

namespace Fest_form.data
{
    public class DataContext : DbContext
    {
        public DbSet<DanceTeam>           DanceTeams           { get; set; }
        public DbSet<Performance>         Performances         { get; set; }
        public DbSet <Category>           Categories           { get; set; }
        public DbSet <Genre>              Genres               { get; set; }
        public DbSet <Persоn>             Persоns              { get; set; }
        public DbSet<ParticipiantsNumber> ParticipiantsNumbers { get; set; }

        public DataContext(DbContextOptions options) : base(options){
        Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    CategoryId = -1,
                    CategoryName = "group1",
                    CategoryDescription = "DesciptionGroup1"
                },
                 new Category()
                 {
                     CategoryId = -2,
                     CategoryName = "group2",
                    CategoryDescription = "DesciptionGroup2"
                },
                new Category()
                {
                    CategoryId = -3,
                    CategoryName = "group3",
                    CategoryDescription = "DesciptionGroup3"
                },
                new Category()
                {
                    CategoryId = -4,
                    CategoryName = "group4",
                    CategoryDescription = "DesciptionGroup4"
                },
                new Category()
                {
                    CategoryId = -5,
                    CategoryName = "group5",
                    CategoryDescription = "DesciptionGroup5"
                }
                );
            modelBuilder.Entity<ParticipiantsNumber>().HasData(
                new ParticipiantsNumber() {
                    id = -1,
                    Name = "Solo"
                },
                new ParticipiantsNumber()
                {
                    id = -2,
                    Name = "Duo/Trio"
                },
                new ParticipiantsNumber()
                {
                    id = -3,
                    Name = "Ensemble(Small)"
                },
                new ParticipiantsNumber()
                {
                    id = -4,
                    Name = "Ensemble(Bigg)"
                }
                );
            modelBuilder.Entity<Genre>().HasData(
                   new Genre()
                   {
                       Id = -1,
                       GenreName = "Classic"
                   },
                   new Genre()
                   {
                       Id = -2,
                       GenreName = "Contemporary/Modern"
                   },
                   new Genre()
                   {
                       Id = -3,
                       GenreName = "Folk"
                   }
                );
        }
    }
}
