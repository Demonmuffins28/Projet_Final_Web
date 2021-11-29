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
            model.TrierPar = DVDModel.TrierPar;
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
        private async Task<List<Films>> getDVDParPage(int page)
        {
            int nbDVDParPage = await getNbDVDParPage();
            return (await getDVD()).Skip(nbDVDParPage * (page - 1)).Take(nbDVDParPage).ToList();
        }

        [NonAction]
        private async Task<List<Films>> getDVD()
        {
            List<Films> listDVD;
            if (model.TitreRechercher == null)
            {
                listDVD = await _context.Films.ToListAsync();
            }
            else
            {
                listDVD =  await _context.Films.Where(a => a.TitreFrancais.Contains(model.TitreRechercher) || a.TitreOriginal.Contains(model.TitreRechercher)).ToListAsync();
            }

            switch (model.TrierPar)
            {
                case 1:
                    listDVD = (from unFilm in listDVD
                              join user in await _userManager.Users.ToListAsync()
                              on unFilm.NoUtilisateurMAJ equals user.Id
                              orderby user.UserName
                              select unFilm).ToList();
                    break;
                case 2:
                    listDVD = listDVD.OrderBy(f => f.TitreFrancais).ToList();
                    break;
                case 3:
                    listDVD = (from unFilm in listDVD
                               join user in await _userManager.Users.ToListAsync()
                               on unFilm.NoUtilisateurMAJ equals user.Id
                               orderby user.UserName, unFilm.TitreFrancais
                               select unFilm).ToList();
                    break;
            }

            return listDVD;
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
            model.listDVD = await getDVDParPage(page);
            model.page = page;
        }

        [NonAction]
        private async Task<Utilisateurs> GetUtilisateursByID(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }
    }
}
