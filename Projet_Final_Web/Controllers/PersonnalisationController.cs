using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projet_Final_Web.Models;
using Projet_Final_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Controllers
{
    public class PersonnalisationController : Controller
    {

        private readonly DbContextProjetFinal _context;
        private readonly UserManager<Utilisateurs> _userManager;
        private IWebHostEnvironment _env;

        public PersonnalisationController(DbContextProjetFinal context, UserManager<Utilisateurs> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            const int DVD_PAR_PAGE = 12;

            //Rechercher l'utilisateur
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<UtilisateursPreferences> lstPreference = _context.UtilisateursPreferences.Where(u => u.NoUtilisateur == currentUser.Id).ToList();
            //Rechercher les préférences

            //Remplir model
            PersonnalisationViewModel model = new PersonnalisationViewModel();
            model.Prenom = currentUser.UserName;
            model.Courriel_Sur_Ajout = lstPreference.Exists(p => p.NoPreference == 3);
            model.Courriel_sur_Appropriation = lstPreference.Exists(p => p.NoPreference == 4);
            model.Courriel_sur_Retrait = lstPreference.Exists(p => p.NoPreference == 5);
            model.NbDVDParPage = lstPreference.Exists(p => p.NoPreference == 7) ? Convert.ToInt32(lstPreference.Find(p => p.NoPreference == 7)): DVD_PAR_PAGE;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("Password, ConfirmPassword, Courriel_Sur_Ajout, Courriel_sur_Retrait, Courriel_sur_Appropriation, NbDVDParPage")] PersonnalisationViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                //Mot de passe
                if (!string.IsNullOrWhiteSpace(model.Password) || !string.IsNullOrEmpty(model.Password)) 
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.Password);
                }
                List<UtilisateursPreferences> lstPreference = _context.UtilisateursPreferences.Where(u => u.NoUtilisateur == user.Id).ToList();
                //Set les preferences
                var Courriel_Sur_Ajout = lstPreference.Find(p => p.NoPreference == 3);
                var Courriel_sur_Appropriation = lstPreference.Find(p => p.NoPreference == 4);
                var Courriel_sur_Retrait = lstPreference.Find(p => p.NoPreference == 5);


                // TODO FAIRE LES MISES A JOUR DES PARAMÈTRES NÉCESSAIRE
                //_context.UtilisateursPreferences.Remove(Courriel_Sur_Ajout);
                //_context.UtilisateursPreferences.Remove(Courriel_sur_Appropriation);
                //_context.UtilisateursPreferences.Remove(Courriel_sur_Retrait);

               // if (Courriel_Sur_Ajout == null)
                 //   (model.Courriel_Sur_Ajout) ? 


                //Courriel_sur_Appropriation
                //Courriel_sur_Retrait
                //Si True existe, si false delete?
            }

            return View(model);
        }
    }
}
