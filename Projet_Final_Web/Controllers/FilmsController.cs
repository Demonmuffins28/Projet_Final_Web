using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Final_Web.Models;
using Projet_Final_Web.ViewModel;

namespace Projet_Final_Web.Controllers
{
    public class FilmsController : Controller
    {
        private readonly DbContextProjetFinal _context;
        private readonly UserManager<Utilisateurs> _userManager;


        public FilmsController(DbContextProjetFinal context, UserManager<Utilisateurs> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var dbContextProjetFinal = _context.Films.Include(f => f.Categories).Include(f => f.Formats).Include(f => f.Producteurs).Include(f => f.Realisateurs).Include(f => f.Utilisateurs);
            return View(await dbContextProjetFinal.ToListAsync());
        }

        // GET: Films/Details/5
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

        // GET: Films/Create
        public IActionResult Create()
        {
            ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "NoCategorie");
            ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "NoFormat");
            ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "NoProducteur");
            ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "NoRealisateur");

            ViewData["ErreurGlobal"] = "";

            AjoutDVDViewModel model = new AjoutDVDViewModel()
            {
                TypeAjout = 1,
                LienRetour = Request.Headers["Referer"].ToString()
            };
            return View(model);
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AjoutDVDViewModel model, string btnAjouterSelectionner)
        {
            ViewData["ErreurGlobal"] = "";

            if (!string.IsNullOrEmpty(btnAjouterSelectionner)) // Verifie que le submit button a ete clicker et non le dropdown list
            {
                if (model.TypeAjout == 1)
                {
                    if (await TitresDVDSontValid(model))
                    {
                        List<string> listTitreNonNull = model.DictionaryNomFilm.Values.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                        Utilisateurs utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);
                        DateTime dateToday = DateTime.Now;
                        string debutNoFilm = dateToday.ToString("yyMM");
                        int dernierNoSeq = _context.Films.Where(f => f.NoFilm.ToString().Substring(0, 4) == debutNoFilm).Count();
                        foreach (string titreDVD in listTitreNonNull)
                        {
                            dernierNoSeq++;
                            Films film = new Films()
                            {
                                NoFilm = Convert.ToInt32(debutNoFilm + dernierNoSeq.ToString("D2")),
                                DateMAJ = dateToday,
                                TitreFrancais = titreDVD.Trim().Substring(0,1).ToUpper() + titreDVD.Trim().Substring(1,titreDVD.Trim().Length-1),
                                NoUtilisateurMAJ = utilisateursActuel.Id
                            };
                            Exemplaires exemplaires = new Exemplaires()
                            {
                                NoExemplaire = Convert.ToInt32(film.NoFilm + "01"),
                                NoUtilisateurProprietaire = film.NoUtilisateurMAJ
                            };
                            EmpruntsFilms empruntsFilms = new EmpruntsFilms()
                            {
                                NoExemplaire = exemplaires.NoExemplaire,
                                NoUtilisateur = exemplaires.NoUtilisateurProprietaire,
                                DateEmprunt = dateToday
                            };
                            _context.Add(film);
                            _context.Add(exemplaires);
                            _context.Add(empruntsFilms);
                        }
                        await _context.SaveChangesAsync();
                        return Redirect(model.LienRetour);
                    }
                }
                else
                {

                    /*if (ModelState.IsValid)
                    {
                        _context.Add(films);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "NoCategorie", films.NoCategorie);
                    ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "NoFormat", films.NoFormat);
                    ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "NoProducteur", films.NoProducteur);
                    ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "NoRealisateur", films.NoRealisateur);*/
                }
            }

            return View(model);
        }
        [NonAction]
        private async Task<Boolean> TitresDVDSontValid(AjoutDVDViewModel model)
        {
            bool binAuMoinsUnTitreDVDNonNull = model.DictionaryNomFilm.Values.Any(t => !string.IsNullOrWhiteSpace(t));
            if (binAuMoinsUnTitreDVDNonNull)
            {
                bool binTitresDVDValid = true;
                List<string> listTitreNonNull = model.DictionaryNomFilm.Values.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                foreach (string titreDVD in listTitreNonNull)
                {
                    bool binMemeTitreEntrerDeuxFois = listTitreNonNull.Where(t => t.Trim().ToLower() == titreDVD.Trim().ToLower()).Count() > 1;
                    bool binTitreDVDExisteDeja = _context.Films.Any(t => t.TitreFrancais.Trim().ToLower() == titreDVD.Trim().ToLower());
                    if (binMemeTitreEntrerDeuxFois || binTitreDVDExisteDeja)
                    {
                        binTitresDVDValid = false;
                    }
                }
                if (!binTitresDVDValid)
                {
                    ViewData["ErreurGlobal"] = "Un ou plusieurs DVD ont le même titre ou existe déjà";
                    return false;
                }
            }
            else
            {
                ViewData["ErreurGlobal"] = "Vous devez remplir au minimum 1 champs de titre de DVD";
                return false;
            }

            return true;
        }

        // GET: Films/Edit/5
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

        // POST: Films/Edit/5
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

        // GET: Films/Delete/5
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

        // POST: Films/Delete/5
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
