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
        private Utilisateurs UtilisateurActuel;
        private DVDViewModel model;

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

            if (UtilisateurActuel == null)
                UtilisateurActuel = await _userManager.GetUserAsync(HttpContext.User);

            if (model == null)
            {
                await initialiserModel(page);
            }
            else
            {
                await changerPage(page);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string DVDFiltreSelectionner)
        {
            return View();
        }

        [NonAction]
        private async Task<int> getDVDParPage()
        {
            bool aPreferenceDVDParPage = await _context.ValeursPreferences.Where(v => v.NoUtilisateur == UtilisateurActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync() != null;
            return aPreferenceDVDParPage ? Convert.ToInt32((await _context.ValeursPreferences.Where(v => v.NoUtilisateur == UtilisateurActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync()).Valeur) : 12;
        }

        [NonAction]
        private async Task<IEnumerable<Films>> getDVD(int page)
        {
            int DVDParPage = await getDVDParPage();
            return await _context.Films.Skip(DVDParPage * (page-1)).Take(DVDParPage).ToListAsync();
        }

        [NonAction]
        private async Task initialiserModel(int page)
        {
            int nbDVDTotal = (await _context.Films.ToListAsync()).Count;
            int DVDParPage = await getDVDParPage();

            model = new DVDViewModel
            {
                nbPage = (nbDVDTotal + DVDParPage - 1) / DVDParPage,
                utilisateursActuel = UtilisateurActuel
            };
            await changerPage(page);
        }

        [NonAction]
        private async Task changerPage(int page)
        {
            model.listDVD = new List<Tuple<Films, int>>();

            foreach (Films DVD in await getDVD(page))
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
