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
        public IActionResult Index()
        {
            return View();
        }

        [NonAction]
        // Combien de DVD afficher par page
        private async Task<int> getDVDParPage()
        {
            bool aPreferenceDVDParPage = await _context.ValeursPreferences.Where(v => v.NoUtilisateur == UtilisateurActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync() != null;
            return aPreferenceDVDParPage ? Convert.ToInt32((await _context.ValeursPreferences.Where(v => v.NoUtilisateur == UtilisateurActuel.Id && v.NoPreference == 7).FirstOrDefaultAsync()).Valeur) : 12;
        }

        [NonAction]
        // Quels DVDs afficher
        private async Task<IEnumerable<Films>> getDVD(int page)
        {
            int DVDParPage = await getDVDParPage();
            return await _context.Films.Where(v => v.NoUtilisateurMAJ == UtilisateurActuel.Id).Skip(DVDParPage * (page - 1)).Take(DVDParPage).OrderBy(v => v.TitreFrancais).ToListAsync();
        }

        [NonAction]
        private async Task initialiserModel(int page)
        {
            int nbDVDTotal = (await _context.Films.Where(v => v.NoUtilisateurMAJ == UtilisateurActuel.Id).ToListAsync()).Count;
            int DVDParPage = await getDVDParPage();

            model = new DVDEnMainViewModel
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

        // GET: DVDEnMain/Details/5
        /*
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var films = await _context.Films
                .Include(f => f.Categories)
                .Include(f => f.Formats)
                .Include(f => f.Producteurs)
                .Include(f => f.Realisateurs)
                .Include(f => f.Utilisateurs)
                .FirstOrDefaultAsync(m => m.NoFilm == id);
            if (films == null)
            {
                return NotFound();
            }

            return View(films);
        }
        */


        /****************************************************************************************************************/

       
        // GET: DVDEnMain/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var films = await _context.Films.FindAsync(id);
            if (films == null)
            {
                return NotFound();
            }
            ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "NoCategorie", films.NoCategorie);
            ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "NoFormat", films.NoFormat);
            ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "NoProducteur", films.NoProducteur);
            ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "NoRealisateur", films.NoRealisateur);
            ViewData["NoUtilisateurMAJ"] = new SelectList(_context.Users, "Id", "Id", films.NoUtilisateurMAJ);
            return View(films);
        }

        // POST: DVDEnMain/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoFilm,AnneeSortie,NoCategorie,NoFormat,DateMAJ,NoUtilisateurMAJ,Resume,DureeMinutes,FilmOriginal,ImagePochette,NbDisques,TitreFrancais,TitreOriginal,VersionEtendue,NoRealisateur,NoProducteur,Xtra")] Films films)
        {
            if (id != films.NoFilm)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(films);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmsExists(films.NoFilm))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "NoCategorie", films.NoCategorie);
            ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "NoFormat", films.NoFormat);
            ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "NoProducteur", films.NoProducteur);
            ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "NoRealisateur", films.NoRealisateur);
            ViewData["NoUtilisateurMAJ"] = new SelectList(_context.Users, "Id", "Id", films.NoUtilisateurMAJ);
            return View(films);
        }

        // GET: DVDEnMain/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var films = await _context.Films
                .Include(f => f.Categories)
                .Include(f => f.Formats)
                .Include(f => f.Producteurs)
                .Include(f => f.Realisateurs)
                .Include(f => f.Utilisateurs)
                .FirstOrDefaultAsync(m => m.NoFilm == id);
            if (films == null)
            {
                return NotFound();
            }

            return View(films);
        }

        // POST: DVDEnMain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var films = await _context.Films.FindAsync(id);
            _context.Films.Remove(films);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmsExists(int id)
        {
            return _context.Films.Any(e => e.NoFilm == id);
        }
    }
}
