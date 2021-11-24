using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_Final_Web.ViewModel;

namespace Projet_Final_Web.Models
{
    public class DbContextProjetFinal : IdentityDbContext<Utilisateurs>
    {
        public DbContextProjetFinal(DbContextOptions<DbContextProjetFinal> options): base()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=tcp:424sql.cgodin.qc.ca,5433;Initial Catalog=BDW56A21_424q;Persist Security Info=True;User ID=W56A21equipe424q;Password=Secret16113");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmpruntsFilms>().HasKey(e => new { e.NoExemplaire, e.NoUtilisateur });
            modelBuilder.Entity<UtilisateursPreferences>().HasKey(e => new { e.NoUtilisateur, e.NoPreference });
            modelBuilder.Entity<ValeursPreferences>().HasKey(e => new { e.NoUtilisateur, e.NoPreference });

            modelBuilder.Entity<FilmsActeurs>().HasKey(e => new { e.NoFilm, e.NoActeur });
            modelBuilder.Entity<FilmsLangues>().HasKey(e => new { e.NoFilm, e.NoLangue });
            modelBuilder.Entity<FilmsSousTitres>().HasKey(e => new { e.NoFilm, e.NoSousTitre });
            modelBuilder.Entity<FilmsSupplements>().HasKey(e => new { e.NoFilm, e.NoSupplement });

            modelBuilder.Entity<FilmsSupplements>();
        }

        public DbSet<Acteurs> Acteurs { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<EmpruntsFilms> EmpruntsFilms { get; set; }
        public DbSet<Exemplaires> Exemplaires { get; set; }
        public DbSet<Films> Films { get; set; }
        public DbSet<Formats> Formats { get; set; }
        public DbSet<Langues> Langues { get; set; }
        public DbSet<Preference> Preference { get; set; }
        public DbSet<Producteurs> Producteurs { get; set; }
        public DbSet<Realisateurs> Realisateurs { get; set; }
        public DbSet<SousTitres> SousTitres { get; set; }
        public DbSet<Supplements> Supplements { get; set; }
        public DbSet<TypesUtilisateur> TypesUtilisateur { get; set; }
        public DbSet<UtilisateursPreferences> UtilisateursPreferences { get; set; }
        public DbSet<ValeursPreferences> ValeursPreferences { get; set; }
        public DbSet<FilmsActeurs> FilmsActeurs { get; set; }
        public DbSet<FilmsLangues> FilmsLangues { get; set; }
        public DbSet<FilmsSousTitres> FilmsSousTitres { get; set; }
        public DbSet<FilmsSupplements> FilmsSupplements { get; set; }
        public DbSet<Projet_Final_Web.ViewModel.UtilisateurViewModel> UtilisateurViewModel { get; set; }


    }
}
