using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Final_Web.Models;

namespace Projet_Final_Web.Controllers
{
    public class FilmsController : Controller
    {
        private readonly DbContextProjetFinal _context;

        public FilmsController(DbContextProjetFinal context)
        {
            _context = context;
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
            ViewData["NoUtilisateurMAJ"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoFilm,AnneeSortie,NoCategorie,NoFormat,DateMAJ,NoUtilisateurMAJ,Resume,DureeMinutes,FilmOriginal,ImagePochette,NbDisques,TitreFrancais,TitreOriginal,VersionEtendue,NoRealisateur,NoProducteur,Xtra")] Films films)
        {
            if (ModelState.IsValid)
            {
                _context.Add(films);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "NoCategorie", films.NoCategorie);
            ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "NoFormat", films.NoFormat);
            ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "NoProducteur", films.NoProducteur);
            ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "NoRealisateur", films.NoRealisateur);
            ViewData["NoUtilisateurMAJ"] = new SelectList(_context.Users, "Id", "Id", films.NoUtilisateurMAJ);
            return View(films);
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
