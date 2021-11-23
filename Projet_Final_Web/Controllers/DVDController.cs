using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_Final_Web.Models;
using Projet_Final_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Controllers
{
    public class DVDController : Controller
    {
        private readonly DbContextProjetFinal _context;
        private readonly SignInManager<Utilisateurs> _signInManager;
        private readonly UserManager<Utilisateurs> _userManager;
        private static DVDViewModel model;

        public DVDController(DbContextProjetFinal context, SignInManager<Utilisateurs> signInManager, UserManager<Utilisateurs> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Login", "Account");

            if (model == null)
            {
                await initialiserModel(page);
            }
            else
            {
                model.utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);
                model.TitreRechercher = "";
                await compterNombrePage();
                await changerPage(page);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(DVDViewModel DVDModel)
        {
            model.TitreRechercher = DVDModel.TitreRechercher;
            await compterNombrePage();
            await changerPage(1);
            model.utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);
            return View(model);
        }






        [NonAction]
        private async Task<int> getNbDVDParPage()
        {
            bool aPreferenceDVDParPage = await _context.ValeursPreferences.Where(v => v.NoUtilisateur == model.utilisateursActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync() != null;
            return aPreferenceDVDParPage ? Convert.ToInt32((await _context.ValeursPreferences.Where(v => v.NoUtilisateur == model.utilisateursActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync()).Valeur) : 12;
        }

        [NonAction]
        private async Task<IEnumerable<Films>> getDVDParPage(int page)
        {
            int nbDVDParPage = await getNbDVDParPage();
            return (await getDVD()).Skip(nbDVDParPage * (page - 1)).Take(nbDVDParPage);
        }

        [NonAction]
        private async Task<List<Films>> getDVD()
        {
            return model.TitreRechercher == null ?
            await _context.Films.ToListAsync() : await _context.Films.Where(a => a.TitreFrancais.Contains(model.TitreRechercher) || a.TitreOriginal.Contains(model.TitreRechercher)).ToListAsync();
        }

        [NonAction]
        private async Task initialiserModel(int page)
        {
            int nbDVDTotal = (await _context.Films.ToListAsync()).Count;

            model = new DVDViewModel();
            model.utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);
            await compterNombrePage();
            model.TitreRechercher = "";
            
            await changerPage(page);
        }

        [NonAction]
        private async Task compterNombrePage()
        {
            int DVDParPage = await getNbDVDParPage();
            model.nbPage = ((await getDVD()).Count() + DVDParPage - 1) / DVDParPage;
        }

        [NonAction]
        private async Task changerPage(int page)
        {
            model.listDVD = new List<Tuple<Films, int>>();

            foreach (Films DVD in await getDVDParPage(page))
            {
                model.listDVD.Add(new Tuple<Films, int> (DVD, await getIdUtilisateurDVDEnMain(DVD)));
            }
            
            model.page = page;
        }

        [NonAction]
        private async Task<int> getIdUtilisateurDVDEnMain(Films DVD)
        {
            return Convert.ToInt32(
                (await 
                _context.EmpruntsFilms
                .Where(v => v.NoExemplaire.ToString().Substring(0, v.NoExemplaire.ToString().Length - 2) == DVD.NoFilm.ToString())
                .OrderByDescending(a => a.DateEmprunt)
                .FirstOrDefaultAsync())
                .NoUtilisateur
            );
        }

    }
}
