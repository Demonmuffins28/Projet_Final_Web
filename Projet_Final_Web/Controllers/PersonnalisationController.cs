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
            List<UtilisateursPreferences> lstPreference = getListPreferences(currentUser);

            //Remplir model
            PersonnalisationViewModel model = new PersonnalisationViewModel();
            model.Is_Changing_Password = false;
            model.Prenom = currentUser.UserName;
            model.Courriel_Sur_Ajout = lstPreference.Exists(p => p.NoPreference == 3);
            model.Courriel_Sur_Ajout = PrefExist(3, lstPreference);
            model.Courriel_sur_Appropriation = PrefExist(4, lstPreference);
            model.Courriel_sur_Retrait = PrefExist(5, lstPreference);
            model.NbDVDParPage = PrefExist(7, lstPreference) ?
                Convert.ToInt32(_context.ValeursPreferences.Where(v => v.NoPreference == 7 && v.NoUtilisateur == currentUser.Id).First().Valeur) : DVD_PAR_PAGE;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("Prenom, Is_Changing_Password, Password, ConfirmPassword, Courriel_Sur_Ajout, Courriel_sur_Retrait, Courriel_sur_Appropriation, NbDVDParPage")] PersonnalisationViewModel model)
        {
            ViewData["PreferenceSauvegarder"] = "";

            if (model.Is_Changing_Password && (string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrEmpty(model.Password)))
            {
                model.Is_Changing_Password = false;
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                //Changement de mot de passe
                if (!string.IsNullOrWhiteSpace(model.Password) || !string.IsNullOrEmpty(model.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.Password);
                }

                List<UtilisateursPreferences> lstPreference = getListPreferences(user);
                //Set les preferences

                //Regarder si les élements existe?
                var Courriel_Sur_Ajout = PrefExist(3, lstPreference) ? lstPreference.Find(p => p.NoPreference == 3) : null;
                var Courriel_sur_Appropriation = PrefExist(4, lstPreference) ? lstPreference.Find(p => p.NoPreference == 4) : null;
                var Courriel_sur_Retrait = PrefExist(5, lstPreference) ? lstPreference.Find(p => p.NoPreference == 5) : null;
                var DVD_Par_Page = PrefExist(7, lstPreference) ? lstPreference.Find(p => p.NoPreference == 7) : null;

                var DVD_Valeur = DVD_Par_Page != null ? _context.ValeursPreferences.Where(v => v.NoPreference == 7 && v.NoUtilisateur == user.Id).First() : null;

                try
                {
                    if (Courriel_Sur_Ajout != null)
                        _context.UtilisateursPreferences.Remove(Courriel_Sur_Ajout);
                    if (Courriel_sur_Appropriation != null)
                        _context.UtilisateursPreferences.Remove(Courriel_sur_Appropriation);
                    if (Courriel_sur_Retrait != null)
                        _context.UtilisateursPreferences.Remove(Courriel_sur_Retrait);
                    if (DVD_Valeur != null)
                        _context.ValeursPreferences.Remove(DVD_Valeur);
                    if (DVD_Par_Page != null)
                        _context.UtilisateursPreferences.Remove(DVD_Par_Page);

                    if (model.Courriel_Sur_Ajout)
                    {
                        UtilisateursPreferences PrefAjout = new UtilisateursPreferences();
                        PrefAjout.NoUtilisateur = user.Id;
                        PrefAjout.NoPreference = 3;
                        _context.UtilisateursPreferences.Add(PrefAjout);
                    }

                    if (model.Courriel_sur_Appropriation)
                    {
                        UtilisateursPreferences PrefAppropriation = new UtilisateursPreferences();
                        PrefAppropriation.NoUtilisateur = user.Id;
                        PrefAppropriation.NoPreference = 4;
                        _context.UtilisateursPreferences.Add(PrefAppropriation);
                    }

                    if (model.Courriel_sur_Retrait)
                    {
                        UtilisateursPreferences PrefRetrait = new UtilisateursPreferences();
                        PrefRetrait.NoUtilisateur = user.Id;
                        PrefRetrait.NoPreference = 5;
                        _context.UtilisateursPreferences.Add(PrefRetrait);
                    }
                    if (model.NbDVDParPage != 12)
                    {
                        UtilisateursPreferences PrefDVD = new UtilisateursPreferences();
                        PrefDVD.NoUtilisateur = user.Id;
                        PrefDVD.NoPreference = 7;
                        _context.UtilisateursPreferences.Add(PrefDVD);

                        ValeursPreferences DVDValeurs = new ValeursPreferences();
                        DVDValeurs.NoUtilisateur = user.Id;
                        DVDValeurs.NoPreference = 7;
                        DVDValeurs.Valeur = (model.NbDVDParPage).ToString();
                        _context.ValeursPreferences.Add(DVDValeurs);
                    }
                    _context.SaveChanges();
                } catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "Erreur lors de l'enregistrement");
                    return View(model);
                }

            }

            ViewData["PreferenceSauvegarder"] = "Préférence sauvegardées!";

            return View(model);
        }

        [NonAction]
        private List<UtilisateursPreferences> getListPreferences(Utilisateurs utilisateur)
        {
            return _context.UtilisateursPreferences.Where(u => u.NoUtilisateur == utilisateur.Id).ToList();
        }


        [NonAction]
        private bool PrefExist(int noPreference, List<UtilisateursPreferences> lstPreferences)
        {
            return lstPreferences.Exists(p => p.NoPreference == noPreference);
        }

    }
}
