﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projet_Final_Web.Models;

namespace Projet_Final_Web.Migrations
{
    [DbContext(typeof(DbContextProjetFinal))]
    [Migration("20211110023326_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Acteurs", b =>
                {
                    b.Property<int>("NoActeur")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .HasColumnName("Nom")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Sexe")
                        .HasColumnName("Sexe")
                        .HasColumnType("nchar(1)");

                    b.HasKey("NoActeur");

                    b.ToTable("Acteurs");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Categories", b =>
                {
                    b.Property<int>("NoCategorie")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NoCategorie");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.EmpruntsFilms", b =>
                {
                    b.Property<int>("NoExemplaire")
                        .HasColumnType("int");

                    b.Property<string>("NoUtilisateur")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateEmprunt")
                        .HasColumnType("datetime2");

                    b.HasKey("NoExemplaire", "NoUtilisateur");

                    b.HasIndex("NoUtilisateur");

                    b.ToTable("EmpruntsFilms");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Exemplaires", b =>
                {
                    b.Property<int>("NoExemplaire")
                        .HasColumnType("int");

                    b.Property<string>("NoUtilisateurProprietaire")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NoExemplaire");

                    b.HasIndex("NoUtilisateurProprietaire");

                    b.ToTable("Exemplaires");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Films", b =>
                {
                    b.Property<int>("NoFilm")
                        .HasColumnType("int");

                    b.Property<int?>("AnneeSortie")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateMAJ")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DureeMinutes")
                        .HasColumnType("int");

                    b.Property<bool?>("FilmOriginal")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePochette")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("NbDisques")
                        .HasColumnType("int");

                    b.Property<int?>("NoCategorie")
                        .HasColumnType("int");

                    b.Property<int?>("NoFormat")
                        .HasColumnType("int");

                    b.Property<int?>("NoProducteur")
                        .HasColumnType("int");

                    b.Property<int?>("NoRealisateur")
                        .HasColumnType("int");

                    b.Property<string>("NoUtilisateurMAJ")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Resume")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("TitreFrancais")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TitreOriginal")
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool?>("VersionEtendue")
                        .HasColumnType("bit");

                    b.Property<string>("Xtra")
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("NoFilm");

                    b.HasIndex("NoCategorie");

                    b.HasIndex("NoFormat");

                    b.HasIndex("NoProducteur");

                    b.HasIndex("NoRealisateur");

                    b.HasIndex("NoUtilisateurMAJ");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsActeurs", b =>
                {
                    b.Property<int>("NoFilm")
                        .HasColumnType("int");

                    b.Property<int>("NoActeur")
                        .HasColumnType("int");

                    b.HasKey("NoFilm", "NoActeur");

                    b.HasIndex("NoActeur");

                    b.ToTable("FilmsActeurs");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsLangues", b =>
                {
                    b.Property<int>("NoFilm")
                        .HasColumnType("int");

                    b.Property<int>("NoLangue")
                        .HasColumnType("int");

                    b.HasKey("NoFilm", "NoLangue");

                    b.HasIndex("NoLangue");

                    b.ToTable("FilmsLangues");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsSousTitres", b =>
                {
                    b.Property<int>("NoFilm")
                        .HasColumnType("int");

                    b.Property<int>("NoSousTitre")
                        .HasColumnType("int");

                    b.HasKey("NoFilm", "NoSousTitre");

                    b.HasIndex("NoSousTitre");

                    b.ToTable("FilmsSousTitres");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsSupplements", b =>
                {
                    b.Property<int>("NoFilm")
                        .HasColumnType("int");

                    b.Property<int>("NoSupplement")
                        .HasColumnType("int");

                    b.HasKey("NoFilm", "NoSupplement");

                    b.HasIndex("NoSupplement");

                    b.ToTable("FilmsSupplements");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Formats", b =>
                {
                    b.Property<int>("NoFormat")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NoFormat");

                    b.ToTable("Formats");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Langues", b =>
                {
                    b.Property<int>("NoLangue")
                        .HasColumnType("int");

                    b.Property<string>("Langue")
                        .HasColumnName("Langue")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("NoLangue");

                    b.ToTable("Langues");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Preference", b =>
                {
                    b.Property<int>("NoPreference")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NoPreference");

                    b.ToTable("Preference");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Producteurs", b =>
                {
                    b.Property<int>("NoProducteur")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .HasColumnName("Nom")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NoProducteur");

                    b.ToTable("Producteurs");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Realisateurs", b =>
                {
                    b.Property<int>("NoRealisateur")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .HasColumnName("Nom")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NoRealisateur");

                    b.ToTable("Realisateurs");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.SousTitres", b =>
                {
                    b.Property<int>("NoSousTitre")
                        .HasColumnType("int");

                    b.Property<string>("LangueSousTitre")
                        .HasColumnName("LangueSousTitre")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("NoSousTitre");

                    b.ToTable("SousTitres");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Supplements", b =>
                {
                    b.Property<int>("NoSupplement")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NoSupplement");

                    b.ToTable("Supplements");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.TypesUtilisateur", b =>
                {
                    b.Property<string>("TypeUtilisateur")
                        .HasColumnName("TypeUtilisateur")
                        .HasColumnType("nchar(1)");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("TypeUtilisateur");

                    b.ToTable("TypesUtilisateur");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Utilisateurs", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("TypesUtilisateurID")
                        .HasColumnType("nchar(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("TypesUtilisateurID");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.UtilisateursPreferences", b =>
                {
                    b.Property<string>("NoUtilisateur")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NoPreference")
                        .HasColumnType("int");

                    b.HasKey("NoUtilisateur", "NoPreference");

                    b.HasIndex("NoPreference");

                    b.ToTable("UtilisateursPreferences");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.ValeursPreferences", b =>
                {
                    b.Property<string>("NoUtilisateur")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NoPreference")
                        .HasColumnType("int");

                    b.Property<string>("Valeur")
                        .HasColumnName("Valeur")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NoUtilisateur", "NoPreference");

                    b.HasIndex("NoPreference");

                    b.ToTable("ValeursPreferences");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet_Final_Web.Models.EmpruntsFilms", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Exemplaires", "Exemplaires")
                        .WithMany("EmpruntsFilms")
                        .HasForeignKey("NoExemplaire")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", "Utilisateurs")
                        .WithMany("EmpruntsFilms")
                        .HasForeignKey("NoUtilisateur")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Exemplaires", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", "Utilisateurs")
                        .WithMany("Exemplaires")
                        .HasForeignKey("NoUtilisateurProprietaire");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Films", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Categories", "Categories")
                        .WithMany("Films")
                        .HasForeignKey("NoCategorie");

                    b.HasOne("Projet_Final_Web.Models.Formats", "Formats")
                        .WithMany("Films")
                        .HasForeignKey("NoFormat");

                    b.HasOne("Projet_Final_Web.Models.Producteurs", "Producteurs")
                        .WithMany("Films")
                        .HasForeignKey("NoProducteur");

                    b.HasOne("Projet_Final_Web.Models.Realisateurs", "Realisateurs")
                        .WithMany("Films")
                        .HasForeignKey("NoRealisateur");

                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", "Utilisateurs")
                        .WithMany("Films")
                        .HasForeignKey("NoUtilisateurMAJ");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsActeurs", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Acteurs", "Acteurs")
                        .WithMany("FilmsActeurs")
                        .HasForeignKey("NoActeur")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.Films", "Films")
                        .WithMany("FilmsActeurs")
                        .HasForeignKey("NoFilm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsLangues", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Films", "Films")
                        .WithMany("FilmsLangues")
                        .HasForeignKey("NoFilm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.Langues", "Langues")
                        .WithMany("FilmsLangues")
                        .HasForeignKey("NoLangue")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsSousTitres", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Films", "Films")
                        .WithMany("FilmsSousTitres")
                        .HasForeignKey("NoFilm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.SousTitres", "SousTitres")
                        .WithMany("FilmsSousTitres")
                        .HasForeignKey("NoSousTitre")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet_Final_Web.Models.FilmsSupplements", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Films", "Films")
                        .WithMany("FilmsSupplements")
                        .HasForeignKey("NoFilm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.Supplements", "Supplements")
                        .WithMany("FilmsSupplements")
                        .HasForeignKey("NoSupplement")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet_Final_Web.Models.Utilisateurs", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.TypesUtilisateur", "TypesUtilisateur")
                        .WithMany()
                        .HasForeignKey("TypesUtilisateurID");
                });

            modelBuilder.Entity("Projet_Final_Web.Models.UtilisateursPreferences", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Preference", "Preference")
                        .WithMany()
                        .HasForeignKey("NoPreference")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", "Utilisateurs")
                        .WithMany("UtilisateursPreferences")
                        .HasForeignKey("NoUtilisateur")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet_Final_Web.Models.ValeursPreferences", b =>
                {
                    b.HasOne("Projet_Final_Web.Models.Preference", "Preference")
                        .WithMany()
                        .HasForeignKey("NoPreference")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet_Final_Web.Models.Utilisateurs", "Utilisateurs")
                        .WithMany()
                        .HasForeignKey("NoUtilisateur")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
