using Microsoft.EntityFrameworkCore;
using CleemyTest.Data;

namespace CleemyTest.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Natures> Natures { get; set; } = null!;
        public DbSet<Devises> Devises { get; set; } = null!;
        public DbSet<Utilisateurs> Utilisateurs { get; set; } = null!;
        public DbSet<Depenses> Depenses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var Natures = modelBuilder.Entity<Natures>();
            var Devises = modelBuilder.Entity<Devises>();
            var Utilisateurs = modelBuilder.Entity<Utilisateurs>();
            var Depenses = modelBuilder.Entity<Depenses>();

            Utilisateurs.HasOne(e => e.Devises)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            Depenses.HasOne(e => e.Devises)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            Depenses.HasOne(e => e.Natures)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            Natures.HasData(
                new Natures { Id = 1, Desc = "Restaurant" },
                new Natures { Id = 2, Desc = "Hotel" },
                new Natures { Id = 3, Desc = "Misc." });

            Devises.HasData(
                new Devises { Id = 1, Desc = "Euro" }, 
                new Devises { Id = 2, Desc = "Dollar américain" },
                new Devises { Id = 3, Desc = "Rouble russe" });

            Utilisateurs.HasData(
                    new Utilisateurs { Id = 1, Nom = "Stark", Prenom = "Anthony", DeviseFK = 2 },
                    new Utilisateurs { Id = 2, Nom = "Romanova", Prenom = "Natasha", DeviseFK = 3 });

        }
    }
}
