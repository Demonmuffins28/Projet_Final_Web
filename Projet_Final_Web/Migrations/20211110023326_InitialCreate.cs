using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projet_Final_Web.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acteurs",
                columns: table => new
                {
                    NoActeur = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Sexe = table.Column<string>(type: "nchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acteurs", x => x.NoActeur);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    NoCategorie = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.NoCategorie);
                });

            migrationBuilder.CreateTable(
                name: "Formats",
                columns: table => new
                {
                    NoFormat = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formats", x => x.NoFormat);
                });

            migrationBuilder.CreateTable(
                name: "Langues",
                columns: table => new
                {
                    NoLangue = table.Column<int>(nullable: false),
                    Langue = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Langues", x => x.NoLangue);
                });

            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    NoPreference = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.NoPreference);
                });

            migrationBuilder.CreateTable(
                name: "Producteurs",
                columns: table => new
                {
                    NoProducteur = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producteurs", x => x.NoProducteur);
                });

            migrationBuilder.CreateTable(
                name: "Realisateurs",
                columns: table => new
                {
                    NoRealisateur = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realisateurs", x => x.NoRealisateur);
                });

            migrationBuilder.CreateTable(
                name: "SousTitres",
                columns: table => new
                {
                    NoSousTitre = table.Column<int>(nullable: false),
                    LangueSousTitre = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SousTitres", x => x.NoSousTitre);
                });

            migrationBuilder.CreateTable(
                name: "Supplements",
                columns: table => new
                {
                    NoSupplement = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplements", x => x.NoSupplement);
                });

            migrationBuilder.CreateTable(
                name: "TypesUtilisateur",
                columns: table => new
                {
                    TypeUtilisateur = table.Column<string>(type: "nchar(1)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(25)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesUtilisateur", x => x.TypeUtilisateur);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    TypesUtilisateurID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_TypesUtilisateur_TypesUtilisateurID",
                        column: x => x.TypesUtilisateurID,
                        principalTable: "TypesUtilisateur",
                        principalColumn: "TypeUtilisateur",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exemplaires",
                columns: table => new
                {
                    NoExemplaire = table.Column<int>(nullable: false),
                    NoUtilisateurProprietaire = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exemplaires", x => x.NoExemplaire);
                    table.ForeignKey(
                        name: "FK_Exemplaires_AspNetUsers_NoUtilisateurProprietaire",
                        column: x => x.NoUtilisateurProprietaire,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    NoFilm = table.Column<int>(nullable: false),
                    AnneeSortie = table.Column<int>(nullable: true),
                    NoCategorie = table.Column<int>(nullable: true),
                    NoFormat = table.Column<int>(nullable: true),
                    DateMAJ = table.Column<DateTime>(nullable: false),
                    NoUtilisateurMAJ = table.Column<string>(nullable: true),
                    Resume = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    DureeMinutes = table.Column<int>(nullable: true),
                    FilmOriginal = table.Column<bool>(nullable: true),
                    ImagePochette = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NbDisques = table.Column<int>(nullable: true),
                    TitreFrancais = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TitreOriginal = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    VersionEtendue = table.Column<bool>(nullable: true),
                    NoRealisateur = table.Column<int>(nullable: true),
                    NoProducteur = table.Column<int>(nullable: true),
                    Xtra = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.NoFilm);
                    table.ForeignKey(
                        name: "FK_Films_Categories_NoCategorie",
                        column: x => x.NoCategorie,
                        principalTable: "Categories",
                        principalColumn: "NoCategorie",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Films_Formats_NoFormat",
                        column: x => x.NoFormat,
                        principalTable: "Formats",
                        principalColumn: "NoFormat",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Films_Producteurs_NoProducteur",
                        column: x => x.NoProducteur,
                        principalTable: "Producteurs",
                        principalColumn: "NoProducteur",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Films_Realisateurs_NoRealisateur",
                        column: x => x.NoRealisateur,
                        principalTable: "Realisateurs",
                        principalColumn: "NoRealisateur",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Films_AspNetUsers_NoUtilisateurMAJ",
                        column: x => x.NoUtilisateurMAJ,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UtilisateursPreferences",
                columns: table => new
                {
                    NoUtilisateur = table.Column<string>(nullable: false),
                    NoPreference = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilisateursPreferences", x => new { x.NoUtilisateur, x.NoPreference });
                    table.ForeignKey(
                        name: "FK_UtilisateursPreferences_Preference_NoPreference",
                        column: x => x.NoPreference,
                        principalTable: "Preference",
                        principalColumn: "NoPreference",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UtilisateursPreferences_AspNetUsers_NoUtilisateur",
                        column: x => x.NoUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValeursPreferences",
                columns: table => new
                {
                    NoUtilisateur = table.Column<string>(nullable: false),
                    NoPreference = table.Column<int>(nullable: false),
                    Valeur = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValeursPreferences", x => new { x.NoUtilisateur, x.NoPreference });
                    table.ForeignKey(
                        name: "FK_ValeursPreferences_Preference_NoPreference",
                        column: x => x.NoPreference,
                        principalTable: "Preference",
                        principalColumn: "NoPreference",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValeursPreferences_AspNetUsers_NoUtilisateur",
                        column: x => x.NoUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpruntsFilms",
                columns: table => new
                {
                    NoExemplaire = table.Column<int>(nullable: false),
                    NoUtilisateur = table.Column<string>(nullable: false),
                    DateEmprunt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpruntsFilms", x => new { x.NoExemplaire, x.NoUtilisateur });
                    table.ForeignKey(
                        name: "FK_EmpruntsFilms_Exemplaires_NoExemplaire",
                        column: x => x.NoExemplaire,
                        principalTable: "Exemplaires",
                        principalColumn: "NoExemplaire",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpruntsFilms_AspNetUsers_NoUtilisateur",
                        column: x => x.NoUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmsActeurs",
                columns: table => new
                {
                    NoFilm = table.Column<int>(nullable: false),
                    NoActeur = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmsActeurs", x => new { x.NoFilm, x.NoActeur });
                    table.ForeignKey(
                        name: "FK_FilmsActeurs_Acteurs_NoActeur",
                        column: x => x.NoActeur,
                        principalTable: "Acteurs",
                        principalColumn: "NoActeur",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmsActeurs_Films_NoFilm",
                        column: x => x.NoFilm,
                        principalTable: "Films",
                        principalColumn: "NoFilm",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmsLangues",
                columns: table => new
                {
                    NoFilm = table.Column<int>(nullable: false),
                    NoLangue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmsLangues", x => new { x.NoFilm, x.NoLangue });
                    table.ForeignKey(
                        name: "FK_FilmsLangues_Films_NoFilm",
                        column: x => x.NoFilm,
                        principalTable: "Films",
                        principalColumn: "NoFilm",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmsLangues_Langues_NoLangue",
                        column: x => x.NoLangue,
                        principalTable: "Langues",
                        principalColumn: "NoLangue",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmsSousTitres",
                columns: table => new
                {
                    NoFilm = table.Column<int>(nullable: false),
                    NoSousTitre = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmsSousTitres", x => new { x.NoFilm, x.NoSousTitre });
                    table.ForeignKey(
                        name: "FK_FilmsSousTitres_Films_NoFilm",
                        column: x => x.NoFilm,
                        principalTable: "Films",
                        principalColumn: "NoFilm",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmsSousTitres_SousTitres_NoSousTitre",
                        column: x => x.NoSousTitre,
                        principalTable: "SousTitres",
                        principalColumn: "NoSousTitre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmsSupplements",
                columns: table => new
                {
                    NoFilm = table.Column<int>(nullable: false),
                    NoSupplement = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmsSupplements", x => new { x.NoFilm, x.NoSupplement });
                    table.ForeignKey(
                        name: "FK_FilmsSupplements_Films_NoFilm",
                        column: x => x.NoFilm,
                        principalTable: "Films",
                        principalColumn: "NoFilm",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmsSupplements_Supplements_NoSupplement",
                        column: x => x.NoSupplement,
                        principalTable: "Supplements",
                        principalColumn: "NoSupplement",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TypesUtilisateurID",
                table: "AspNetUsers",
                column: "TypesUtilisateurID");

            migrationBuilder.CreateIndex(
                name: "IX_EmpruntsFilms_NoUtilisateur",
                table: "EmpruntsFilms",
                column: "NoUtilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Exemplaires_NoUtilisateurProprietaire",
                table: "Exemplaires",
                column: "NoUtilisateurProprietaire");

            migrationBuilder.CreateIndex(
                name: "IX_Films_NoCategorie",
                table: "Films",
                column: "NoCategorie");

            migrationBuilder.CreateIndex(
                name: "IX_Films_NoFormat",
                table: "Films",
                column: "NoFormat");

            migrationBuilder.CreateIndex(
                name: "IX_Films_NoProducteur",
                table: "Films",
                column: "NoProducteur");

            migrationBuilder.CreateIndex(
                name: "IX_Films_NoRealisateur",
                table: "Films",
                column: "NoRealisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Films_NoUtilisateurMAJ",
                table: "Films",
                column: "NoUtilisateurMAJ");

            migrationBuilder.CreateIndex(
                name: "IX_FilmsActeurs_NoActeur",
                table: "FilmsActeurs",
                column: "NoActeur");

            migrationBuilder.CreateIndex(
                name: "IX_FilmsLangues_NoLangue",
                table: "FilmsLangues",
                column: "NoLangue");

            migrationBuilder.CreateIndex(
                name: "IX_FilmsSousTitres_NoSousTitre",
                table: "FilmsSousTitres",
                column: "NoSousTitre");

            migrationBuilder.CreateIndex(
                name: "IX_FilmsSupplements_NoSupplement",
                table: "FilmsSupplements",
                column: "NoSupplement");

            migrationBuilder.CreateIndex(
                name: "IX_UtilisateursPreferences_NoPreference",
                table: "UtilisateursPreferences",
                column: "NoPreference");

            migrationBuilder.CreateIndex(
                name: "IX_ValeursPreferences_NoPreference",
                table: "ValeursPreferences",
                column: "NoPreference");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmpruntsFilms");

            migrationBuilder.DropTable(
                name: "FilmsActeurs");

            migrationBuilder.DropTable(
                name: "FilmsLangues");

            migrationBuilder.DropTable(
                name: "FilmsSousTitres");

            migrationBuilder.DropTable(
                name: "FilmsSupplements");

            migrationBuilder.DropTable(
                name: "UtilisateursPreferences");

            migrationBuilder.DropTable(
                name: "ValeursPreferences");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Exemplaires");

            migrationBuilder.DropTable(
                name: "Acteurs");

            migrationBuilder.DropTable(
                name: "Langues");

            migrationBuilder.DropTable(
                name: "SousTitres");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "Supplements");

            migrationBuilder.DropTable(
                name: "Preference");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Formats");

            migrationBuilder.DropTable(
                name: "Producteurs");

            migrationBuilder.DropTable(
                name: "Realisateurs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TypesUtilisateur");
        }
    }
}
