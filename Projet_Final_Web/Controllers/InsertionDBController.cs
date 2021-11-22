using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Projet_Final_Web.Models;
using System.IO;
using System.Text.Json;

namespace Projet_Final_Web.Controllers
{
    public class InsertionDBController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Utilisateurs> userManager;

        public InsertionDBController(IConfiguration configuration, UserManager<Utilisateurs> userManager)
        {
            _configuration = configuration;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("connectionStrings"));

            Server server = new Server(new ServerConnection(conn));

            //Effacer toute les donnees
            string donneesSupprimer = "DELETE FROM [dbo].[FilmsActeurs]            " +
                                        "DELETE FROM [dbo].[FilmsLangues]            " +
                                        "DELETE FROM [dbo].[FilmsSousTitres]         " +
                                        "DELETE FROM [dbo].[FilmsSupplements]        " +
                                        "DELETE FROM [dbo].[Films]                   " +
                                        "DELETE FROM [dbo].[Exemplaires]             " +
                                        "DELETE FROM [dbo].[EmpruntsFilms]           " +
                                        "DELETE FROM [dbo].[Formats]                 " +
                                        "DELETE FROM [dbo].[Langues]                 " +
                                        "DELETE FROM [dbo].[Producteurs]             " +
                                        "DELETE FROM [dbo].[Realisateurs]            " +
                                        "DELETE FROM [dbo].[SousTitres]              " +
                                        "DELETE FROM [dbo].[Supplements]             " +
                                        "DELETE FROM [dbo].[Categories]             " +
                                        "DELETE FROM [dbo].[Acteurs]                 " +
                                        "DELETE FROM [dbo].[UtilisateursPreferences] " +
                                        "DELETE FROM [dbo].[ValeursPreferences]      " +
                                        "DELETE FROM [dbo].[Preference]             " +
                                        "DELETE FROM [dbo].[AspNetUsers]             " +
                                        "DELETE FROM [dbo].[TypesUtilisateur]        ";
            server.ConnectionContext.ExecuteNonQuery(donneesSupprimer);

            // Inserer les type d'utilisateur
            string typeUtilisateur ="INSERT[dbo].[TypesUtilisateur]([TypeUtilisateur], [Description]) VALUES('A', 'Administrateur')"  +
                                    "INSERT[dbo].[TypesUtilisateur]([TypeUtilisateur], [Description]) VALUES('S', 'Super utilisateur')" +
                                    "INSERT[dbo].[TypesUtilisateur]([TypeUtilisateur], [Description]) VALUES('U', 'Utilisateur')";
            
            server.ConnectionContext.ExecuteNonQuery(typeUtilisateur);

            // Inserer les user identity
            using (StreamReader r = new StreamReader(@"FichiersInsertionDB\IdentityUser.json"))
            {
                string json = r.ReadToEnd();
                List<utilisateurJson> utilisateurs = JsonSerializer.Deserialize<List<utilisateurJson>>(json);
                foreach (var item in utilisateurs)
                {
                    var utilisateur = new Utilisateurs
                    {
                        Id = item.NoUtilisateur.ToString(),
                        UserName = item.NomUtilisateur,
                        Email = item.Courriel,
                        TypesUtilisateurID = item.TypeUtilisateur
                    };
                    await userManager.CreateAsync(utilisateur, item.MotPasse.ToString().Trim());
                }
            }

            string script = System.IO.File.ReadAllText(@"FichiersInsertionDB\InsertionDonnes.sql");
            server.ConnectionContext.ExecuteNonQuery(script);

            return View();
        }
    }
    public class utilisateurJson
    {
        public int NoUtilisateur { get; set; }
        public string NomUtilisateur { get; set; }
        public string Courriel { get; set; }
        public string TypeUtilisateur { get; set; }
        public int MotPasse { get; set; }

    }
}
