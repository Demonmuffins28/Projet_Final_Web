using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Final_Web.Models;
using Projet_Final_Web.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Projet_Final_Web.Controllers
{
    public class DVDEnMainController : Controller
    {
        private readonly DbContextProjetFinal _context;
        private readonly SignInManager<Utilisateurs> _signInManager;
        private readonly UserManager<Utilisateurs> _userManager;
        private Utilisateurs UtilisateurActuel;
        private DVDEnMainViewModel model;

        public DVDEnMainController(DbContextProjetFinal context, 
            SignInManager<Utilisateurs> signInManager, UserManager<Utilisateurs> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int? id, int page = 1)
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToAction("Login", "Account");

            if (UtilisateurActuel == null)
                UtilisateurActuel = await _userManager.GetUserAsync(HttpContext.User);

            if (model == null)
            {
                await initialiserModel(id.ToString(), page);
            }
            else
            {
                await changerPage(id.ToString(), page);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }

        [NonAction]
        // Combien de DVD afficher par page
        private async Task<int> getDVDParPage()
        {
            return 99;
            //bool aPreferenceDVDParPage = await _context.ValeursPreferences.Where(v => v.NoUtilisateur == UtilisateurActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync() != null;
            //return aPreferenceDVDParPage ? Convert.ToInt32((await _context.ValeursPreferences.Where(v => v.NoUtilisateur == UtilisateurActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync()).Valeur) : 12;
        }

        [NonAction]
        // Quels DVDs afficher
        private async Task<IEnumerable<Films>> getDVD(string utilisateurID, int page)
        {
            int DVDParPage = await getDVDParPage();
            return await _context.Films.Where(v => v.NoUtilisateurMAJ == utilisateurID).Skip(DVDParPage * (page - 1)).Take(DVDParPage).OrderBy(v => v.TitreFrancais).ToListAsync();
        }

        [NonAction]
        private async Task initialiserModel(string utilisateurID, int page)
        {
            int nbDVDTotal = (await _context.Films.Where(v => v.NoUtilisateurMAJ == utilisateurID).ToListAsync()).Count;
            int DVDParPage = await getDVDParPage();

            model = new DVDEnMainViewModel
            {
                nbPage = (nbDVDTotal + DVDParPage - 1) / DVDParPage,
                //utilisateursActuel = UtilisateurActuel
            };
            await changerPage(utilisateurID, page);
        }

        [NonAction]
        private async Task changerPage(string utilisateurID, int page)
        {
            model.listDVD = new List<Tuple<Films, int>>();

            foreach (Films DVD in await getDVD(utilisateurID, page))
            {
                model.listDVD.Add(new Tuple<Films, int>(DVD, await getIdUtilisateurDVDEnMain(DVD)));
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

        private bool FilmsExists(int id)
        {
            return _context.Films.Any(e => e.NoFilm == id);
        }
    }
}
