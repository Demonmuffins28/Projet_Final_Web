using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private IWebHostEnvironment _env;

        public FilmsController(DbContextProjetFinal context, UserManager<Utilisateurs> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var dbContextProjetFinal = _context.Films.Include(f => f.Categories).Include(f => f.Formats).Include(f => f.Producteurs).Include(f => f.Realisateurs).Include(f => f.Utilisateurs);
            return View(await dbContextProjetFinal.ToListAsync());
        }

        public IActionResult Message(Message model)
        {
            return View(model);
        }

        public async Task<IActionResult> Emprunt(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Films films = await _context.Films.FirstOrDefaultAsync(m => m.NoFilm == id);
            Utilisateurs utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);


            DetailSupprimerViewModel model = new DetailSupprimerViewModel()
            {
                Film = await _context.Films.FirstOrDefaultAsync(m => m.NoFilm == id),
                ListFilmActeur = _context.FilmsActeurs.Where(a => a.NoFilm == id).Select(a => a.Acteurs.Nom).ToList(),
                ListFilmsLangues = _context.FilmsLangues.Where(a => a.NoFilm == id).Select(a => a.Langues.Langue).ToList(),
                ListFilmsSousTitres = _context.FilmsSousTitres.Where(a => a.NoFilm == id).Select(a => a.SousTitres.LangueSousTitre).ToList(),
                ListFilmsSupplements = _context.FilmsSupplements.Where(a => a.NoFilm == id).Select(a => a.Supplements.Description).ToList(),
                NomEmprunter = (await _userManager.FindByIdAsync(films.NoUtilisateurMAJ)).UserName,
                NomProprietaire = (await _userManager.FindByIdAsync(_context.Exemplaires.FirstOrDefault(a => a.NoExemplaire.ToString().Substring(0, 6) == id.ToString()).NoUtilisateurProprietaire)).UserName,
                LienRetour = Request.Headers["Referer"].ToString(),
                Categorie = films.NoCategorie != null ? _context.Categories.FirstOrDefault(c => c.NoCategorie == films.NoCategorie).Description : "",
                Format = films.Formats != null ? _context.Formats.FirstOrDefault(c => c.NoFormat == films.NoFormat).Description : "",
                Realisateur = films.NoRealisateur != null ? _context.Realisateurs.FirstOrDefault(c => c.NoRealisateur == films.NoRealisateur).Nom : "",
                Producteur = films.NoProducteur != null ? _context.Producteurs.FirstOrDefault(c => c.NoProducteur == films.NoProducteur).Nom : "",
                IDUtilisateursSelectionner = utilisateursActuel.Id,
                typeUtilisateursConnecter = utilisateursActuel.TypesUtilisateurID
            };

            ViewData["listUtilisateurs"] = new SelectList(_userManager.Users.Where(a => a.TypesUtilisateurID != "A"), "Id", "UserName");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Emprunt(int id, string LienRetour, string IDUtilisateursSelectionner)
        {
            var films = await _context.Films.FindAsync(id);
            Utilisateurs utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);

            string noUtilisateur = utilisateursActuel.TypesUtilisateurID == "S" ? IDUtilisateursSelectionner : utilisateursActuel.Id;

            films.NoUtilisateurMAJ = noUtilisateur;
            films.DateMAJ = DateTime.Now;

            int NoExemplaire = Convert.ToInt32(films.NoFilm + "01");

            EmpruntsFilms? emprunt = _context.EmpruntsFilms.FirstOrDefault(a => a.NoUtilisateur == noUtilisateur && a.NoExemplaire == NoExemplaire);

            if (emprunt == null)
            {
                EmpruntsFilms empruntsFilms = new EmpruntsFilms() { 
                    NoExemplaire =NoExemplaire,
                    NoUtilisateur = noUtilisateur,
                    DateEmprunt = DateTime.Now
                };
                _context.Add(empruntsFilms);
            }
            else
            {
                emprunt.DateEmprunt = DateTime.Now;
                _context.Update(emprunt);
            }
            _context.Update(films);

            await _context.SaveChangesAsync();


            var ListIdUtilisateur = _context.UtilisateursPreferences.Where(a => a.NoPreference == 4).Select(a => a.NoUtilisateur);
            var EmailUtilisateurs = _userManager.Users.Where(a => ListIdUtilisateur.Contains(a.Id)).Select(a => a.Email);

            if (EmailUtilisateurs.Count() > 0)
            {
                return RedirectToAction("Message", new
                {
                    Destinataire = String.Join(", ", EmailUtilisateurs.ToArray()),
                    Sujet = "Appropriation d'un DVD",
                    Corps = $"L'utilisateur {(await _userManager.FindByIdAsync(noUtilisateur)).UserName} à emprunter le DVD suivant :  {films.TitreFrancais}"
                });
            }

            return Redirect(LienRetour);
        }


        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Films films = await _context.Films.FirstOrDefaultAsync(m => m.NoFilm == id);

            DetailSupprimerViewModel model = new DetailSupprimerViewModel() {
                Film = await _context.Films.FirstOrDefaultAsync(m => m.NoFilm == id),
                ListFilmActeur = _context.FilmsActeurs.Where(a => a.NoFilm == id).Select(a => a.Acteurs.Nom).ToList(),
                ListFilmsLangues = _context.FilmsLangues.Where(a => a.NoFilm == id).Select(a => a.Langues.Langue).ToList(),
                ListFilmsSousTitres = _context.FilmsSousTitres.Where(a => a.NoFilm == id).Select(a => a.SousTitres.LangueSousTitre).ToList(),
                ListFilmsSupplements = _context.FilmsSupplements.Where(a => a.NoFilm == id).Select(a => a.Supplements.Description).ToList(),
                NomEmprunter = (await _userManager.FindByIdAsync(films.NoUtilisateurMAJ)).UserName,
                NomProprietaire = (await _userManager.FindByIdAsync(_context.Exemplaires.FirstOrDefault(a=> a.NoExemplaire.ToString().Substring(0, 6) == id.ToString()).NoUtilisateurProprietaire)).UserName,
                LienRetour = Request.Headers["Referer"].ToString(),
                Categorie = films.NoCategorie != null ? _context.Categories.FirstOrDefault(c => c.NoCategorie == films.NoCategorie).Description : "",
                Format = films.Formats != null ? _context.Formats.FirstOrDefault(c => c.NoFormat == films.NoFormat).Description : "",
                Realisateur = films.NoRealisateur != null ?  _context.Realisateurs.FirstOrDefault(c => c.NoRealisateur == films.NoRealisateur).Nom : "",
                Producteur = films.NoProducteur != null ?  _context.Producteurs.FirstOrDefault(c => c.NoProducteur == films.NoProducteur).Nom : ""
            };

            return View(model);
        }

        // GET: Films/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ErreurGlobal"] = "";

            Utilisateurs utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);


            AjoutEditDVDViewModel model = new AjoutEditDVDViewModel()
            {
                TypeAjout = 1,
                LienRetour = Request.Headers["Referer"].ToString(),
                IDUtilisateursSelectionner = utilisateursActuel.Id,
                typeUtilisateursConnecter = utilisateursActuel.TypesUtilisateurID
            };

            ViewData["listUtilisateurs"] = new SelectList(_userManager.Users.Where(a => a.TypesUtilisateurID != "A"), "Id", "UserName");
            return View(model);
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AjoutEditDVDViewModel model, string btnAjouterSelectionner)
        {
            ViewData["ErreurGlobal"] = "";
            ViewData["ErreurImageClass"] = "";
            ViewData["ErreurPasImage"] = "";

            Utilisateurs utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);
            model.typeUtilisateursConnecter = utilisateursActuel.TypesUtilisateurID;
            ViewData["listUtilisateurs"] = new SelectList(_userManager.Users.Where(a => a.TypesUtilisateurID != "A"), "Id", "UserName");


            if (!string.IsNullOrEmpty(btnAjouterSelectionner)) // Verifie que le submit button a ete clicker et non le dropdown list
            {
                if (model.TypeAjout == 1)
                {
                    if (await TitresDVDSontValid(model))
                    {
                        List<string> listTitreNonNull = model.DictionaryNomFilm.Values.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
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
                                NoUtilisateurMAJ = model.typeUtilisateursConnecter == "S" ? model.IDUtilisateursSelectionner : utilisateursActuel.Id
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

                        var ListIdUtilisateur = _context.UtilisateursPreferences.Where(a => a.NoPreference == 3).Select(a => a.NoUtilisateur);
                        var EmailUtilisateurs = _userManager.Users.Where(a => ListIdUtilisateur.Contains(a.Id)).Select(a => a.Email);

                        if (EmailUtilisateurs.Count() > 0)
                        {
                            return RedirectToAction("Message", new {
                                Destinataire = String.Join(", ", EmailUtilisateurs.ToArray()),
                                Sujet = "Ajout de plusieurs nouveau DVD",
                                Corps = model.typeUtilisateursConnecter == "S" ?
                                $"Le super utilisateur {utilisateursActuel.UserName} à ajouté les DVD suivant :  {String.Join(", ", listTitreNonNull.ToArray())} à l'utilisateur {await _userManager.FindByIdAsync(model.IDUtilisateursSelectionner)}"
                                : $"L'utilisateur {utilisateursActuel.UserName} à ajouté les DVD suivant :  {String.Join(",", listTitreNonNull.ToArray())}"
                            });
                        }

                        return Redirect(model.LienRetour);
                    }
                }
                else
                {
                    model.Film.FilmOriginal = model.FilmOriginal == "1" ? true : false;
                    model.Film.VersionEtendue = model.VersionEtendue == "1" ? true : false;
                    if (ModelState.IsValid)
                    {
                        if (await ValidationAjoutComplet(model))
                        {
                            DateTime dateToday = DateTime.Now;
                            string debutNoFilm = dateToday.ToString("yyMM");
                            int NoSeq = _context.Films.Where(f => f.NoFilm.ToString().Substring(0, 4) == debutNoFilm).Count()+1;
                            model.Film.NoFilm = Convert.ToInt32(debutNoFilm + NoSeq.ToString("D2"));
                            model.Film.DateMAJ = dateToday;
                            model.Film.NoUtilisateurMAJ = model.typeUtilisateursConnecter == "S" ? model.IDUtilisateursSelectionner : utilisateursActuel.Id;    
                            model.Film.TitreFrancais = model.Film.TitreFrancais.Trim().Substring(0, 1).ToUpper() + model.Film.TitreFrancais.Trim().Substring(1, model.Film.TitreFrancais.Trim().Length - 1);
                            
                            if (model.image != null)
                            {
                                model.Film.ImagePochette = model.Film.NoFilm + "."+ model.image.FileName.Split('.')[1].ToLower();
                                string wwwPath = _env.WebRootPath;
                                string path = Path.Combine(wwwPath, "images");

                                string fileName = Path.GetFileName(model.Film.ImagePochette);
                                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                                {
                                    model.image.CopyTo(stream);
                                }
                            }

                            Exemplaires exemplaires = new Exemplaires()
                            {
                                NoExemplaire = Convert.ToInt32(model.Film.NoFilm + "01"),
                                NoUtilisateurProprietaire = model.typeUtilisateursConnecter == "S" ? model.IDUtilisateursSelectionner : utilisateursActuel.Id
                            };
                            EmpruntsFilms empruntsFilms = new EmpruntsFilms()
                            {
                                NoExemplaire = exemplaires.NoExemplaire,
                                NoUtilisateur = exemplaires.NoUtilisateurProprietaire,
                                DateEmprunt = dateToday
                            };
                            _context.Add(model.Film);
                            _context.Add(exemplaires);
                            _context.Add(empruntsFilms);

                            List<int> listActeursNonNull = model.DictionaryActeurs.Values.Where(t => t != -1).ToList();
                            List<int> listLanguesNonNull = model.DictionaryLangues.Values.Where(t => t != -1).ToList();
                            List<int> listSousTitreNonNull = model.DictionarySousTitre.Values.Where(t => t != -1).ToList();
                            List<int> listSupplementNonNull = model.DictionarySupplements.Values.Where(t => t != -1).ToList();

                            foreach (int NoActeur in listActeursNonNull)
                            {
                                FilmsActeurs filmsActeurs = new FilmsActeurs() 
                                { 
                                    NoActeur = NoActeur,
                                    NoFilm = model.Film.NoFilm
                                };
                                _context.Add(filmsActeurs);
                            }
                            foreach (int NoLangue in listLanguesNonNull)
                            {
                                FilmsLangues filmsLangues = new FilmsLangues()
                                {
                                    NoLangue = NoLangue,
                                    NoFilm = model.Film.NoFilm
                                };
                                _context.Add(filmsLangues);
                            }
                            foreach (int NoSousTitre in listSousTitreNonNull)
                            {
                                FilmsSousTitres filmsSousTitres = new FilmsSousTitres()
                                {
                                    NoSousTitre = NoSousTitre,
                                    NoFilm = model.Film.NoFilm
                                };
                                _context.Add(filmsSousTitres);
                            }
                            foreach (int NoSupplement in listSupplementNonNull)
                            {
                                FilmsSupplements filmsSupplements = new FilmsSupplements()
                                {
                                    NoSupplement = NoSupplement,
                                    NoFilm = model.Film.NoFilm
                                };
                                _context.Add(filmsSupplements);
                            }
                            await _context.SaveChangesAsync();


                            var ListIdUtilisateur = _context.UtilisateursPreferences.Where(a => a.NoPreference == 3).Select(a => a.NoUtilisateur);
                            var EmailUtilisateurs = _userManager.Users.Where(a => ListIdUtilisateur.Contains(a.Id)).Select(a => a.Email);

                            if (EmailUtilisateurs.Count() > 0)
                            {
                                return RedirectToAction("Message", new
                                {
                                    Destinataire = String.Join(", ", EmailUtilisateurs.ToArray()),
                                    Sujet = "Ajout d'un nouveau DVD",
                                    Corps = model.typeUtilisateursConnecter == "S" ?
                                    $"Le super utilisateur {utilisateursActuel.UserName} à ajouté le DVD suivant :  {model.Film.TitreFrancais} à l'utilisateur {await _userManager.FindByIdAsync(model.IDUtilisateursSelectionner)}"
                                    : $"L'utilisateur {utilisateursActuel.UserName} à ajouté le DVD suivant :  {model.Film.TitreFrancais}"
                                });
                            }

                            return Redirect(model.LienRetour);
                        }
                    }
                }
            }

            ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "Description", model.Film).Prepend(new SelectListItem("", null));
            ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "Description", model.Film).Prepend(new SelectListItem("", null));
            ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "Nom", model.Film).Prepend(new SelectListItem("", null));
            ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "Nom", model.Film).Prepend(new SelectListItem("", null));
            ViewData["FilmOriginal"] = new List<SelectListItem>() { new SelectListItem("Non", "0"), new SelectListItem("Oui", "1", model.Film != null ? model.FilmOriginal == "1": false) };
            ViewData["VersionEtendue"] = new List<SelectListItem>() { new SelectListItem("Non", "0"), new SelectListItem("Oui", "1", model.Film != null ? model.VersionEtendue == "1" : false) };

            ViewData["NoActeur"] = new SelectList(_context.Acteurs, "NoActeur", "Nom").Prepend(new SelectListItem("", "-1"));
            ViewData["NoLangue"] = new SelectList(_context.Langues, "NoLangue", "Langue").Prepend(new SelectListItem("", "-1"));
            ViewData["NoSousTitre"] = new SelectList(_context.SousTitres, "NoSousTitre", "LangueSousTitre").Prepend(new SelectListItem("", "-1"));
            ViewData["NoSupplement"] = new SelectList(_context.Supplements, "NoSupplement", "Description").Prepend(new SelectListItem("", "-1"));

            return View(model);
        }

        [NonAction]
        private async Task<Boolean> ValidationAjoutComplet(AjoutEditDVDViewModel model)
        {
            bool binValidation = true;
            if (model.DictionaryActeurs.Values.Where(a => a != -1).ToList().Count != model.DictionaryActeurs.Values.Distinct().Where(a => a != -1).ToList().Count)
            {
                ViewData["ErreurGlobal"] = "Vous avez séléctionné plusieurs fois le même acteur.";
                binValidation = false;
            }
            else if (model.DictionaryLangues.Values.Where(a => a != -1).ToList().Count != model.DictionaryLangues.Values.Distinct().Where(a => a != -1).ToList().Count)
            {
                ViewData["ErreurGlobal"] = "Vous avez séléctionné plusieurs fois la même langues";
                binValidation = false;
            }
            else if (model.DictionarySousTitre.Values.Where(a => a != -1).ToList().Count != model.DictionarySousTitre.Values.Distinct().Where(a => a != -1).ToList().Count)
            {
                ViewData["ErreurGlobal"] = "Vous avez séléctionné plusieurs fois le même sous titre";
                binValidation = false;
            }
            else if (model.DictionarySupplements.Values.Where(a => a != -1).ToList().Count != model.DictionarySupplements.Values.Distinct().Where(a => a != -1).ToList().Count)
            {
                ViewData["ErreurGlobal"] = "Vous avez séléctionné plusieurs fois le même suppléments";
                binValidation = false;
            }
            else if ((model.Film.NoFilm == null && _context.Films.Any(f => f.TitreFrancais.Trim().ToLower() == model.Film.TitreFrancais.Trim().ToLower())) ||
                (model.Film.NoFilm != null && _context.Films.Any(f => f.TitreFrancais.Trim().ToLower() == model.Film.TitreFrancais.Trim().ToLower() && f.NoFilm != model.Film.NoFilm)))
            {
                ViewData["ErreurGlobal"] = "Le titre de DVD entrer existe déjà";
                binValidation = false;
            }
            if (model.image != null)
            {
                if (!new List<string>() { "png", "jpg", "jpeg" }.Contains(model.image.FileName.Split('.')[1].ToLower()))
                {
                    binValidation = false;
                    ViewData["ErreurPasImage"] = "Vous devez selectionner une image valide de format (png, jpg ou jpeg)";
                    ViewData["ErreurImageClass"] = "is-invalid";
                }
            }

            return binValidation;
        }

        [NonAction]
        private async Task<Boolean> TitresDVDSontValid(AjoutEditDVDViewModel model)
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
            ViewData["ErreurGlobal"] = "";

            AjoutEditDVDViewModel model = new AjoutEditDVDViewModel()
            {
                LienRetour = Request.Headers["Referer"].ToString(),
                Film = films
            };


            List<int> ListNoActeur = _context.FilmsActeurs.Where(a => a.NoFilm == films.NoFilm).Select(a => a.NoActeur).ToList();
            List<int> ListNoLangue = _context.FilmsLangues.Where(a => a.NoFilm == films.NoFilm).Select(a => a.NoLangue).ToList();
            List<int> ListNoSousTitre = _context.FilmsSousTitres.Where(a => a.NoFilm == films.NoFilm).Select(a => a.NoSousTitre).ToList();
            List<int> ListNoSupplement = _context.FilmsSupplements.Where(a => a.NoFilm == films.NoFilm).Select(a => a.NoSupplement).ToList();

            for (int i = 1; i <= ListNoActeur.Count; i++)
            {
                model.DictionaryActeurs[i] = ListNoActeur[i-1];
            }

            for (int i = 1; i <= ListNoLangue.Count; i++)
            {
                model.DictionaryLangues[i] = ListNoLangue[i - 1];
            }

            for (int i = 1; i <= ListNoSousTitre.Count; i++)
            {
                model.DictionarySousTitre[i] = ListNoSousTitre[i - 1];
            }

            for (int i = 1; i <= ListNoSupplement.Count; i++)
            {
                model.DictionarySupplements[i] = ListNoSupplement[i - 1];
            }

            ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "Description", films).Prepend(new SelectListItem("", null));
            ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "Description", films).Prepend(new SelectListItem("", null));
            ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "Nom", films).Prepend(new SelectListItem("", null));
            ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "Nom", films).Prepend(new SelectListItem("", null));
            ViewData["FilmOriginal"] = new List<SelectListItem>() { new SelectListItem("Non", "0"), new SelectListItem("Oui", "1", films.FilmOriginal == true) };
            ViewData["VersionEtendue"] = new List<SelectListItem>() { new SelectListItem("Non", "0"), new SelectListItem("Oui", "1", films.VersionEtendue == true) };

            ViewData["NoActeur"] = new SelectList(_context.Acteurs, "NoActeur", "Nom").Prepend(new SelectListItem("", "-1"));
            ViewData["NoLangue"] = new SelectList(_context.Langues, "NoLangue", "Langue").Prepend(new SelectListItem("", "-1"));
            ViewData["NoSousTitre"] = new SelectList(_context.SousTitres, "NoSousTitre", "LangueSousTitre").Prepend(new SelectListItem("", "-1"));
            ViewData["NoSupplement"] = new SelectList(_context.Supplements, "NoSupplement", "Description").Prepend(new SelectListItem("", "-1"));
            return View(model);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AjoutEditDVDViewModel model)
        {
            ViewData["ErreurGlobal"] = "";
            ViewData["ErreurImageClass"] = "";
            ViewData["ErreurPasImage"] = "";

            model.Film.FilmOriginal = model.FilmOriginal == "1" ? true : false;
            model.Film.VersionEtendue = model.VersionEtendue == "1" ? true : false;
            if (ModelState.IsValid)
            {
                if (await ValidationAjoutComplet(model))
                {
                    DateTime dateToday = DateTime.Now;
                    string debutNoFilm = dateToday.ToString("yyMM");
                    int NoSeq = _context.Films.Where(f => f.NoFilm.ToString().Substring(0, 4) == debutNoFilm).Count() + 1;
                    model.Film.DateMAJ = dateToday;
                    model.Film.TitreFrancais = model.Film.TitreFrancais.Trim().Substring(0, 1).ToUpper() + model.Film.TitreFrancais.Trim().Substring(1, model.Film.TitreFrancais.Trim().Length - 1);

                    if (model.image != null)
                    {
                        string wwwPath = _env.WebRootPath;
                        string path = Path.Combine(wwwPath, "images");

                        if (model.Film.ImagePochette != null && System.IO.File.Exists(path + model.Film.ImagePochette))
                        {
                            System.IO.File.Delete(path + model.Film.ImagePochette);
                        }

                        model.Film.ImagePochette = model.Film.NoFilm + "." + model.image.FileName.Split('.')[1].ToLower();
                        string fileName = Path.GetFileName(model.Film.ImagePochette);
                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            model.image.CopyTo(stream);
                        }
                    }

                    _context.Update(model.Film);

                    List<int> listActeursNonNull = model.DictionaryActeurs.Values.Where(t => t != -1).ToList();
                    List<int> listLanguesNonNull = model.DictionaryLangues.Values.Where(t => t != -1).ToList();
                    List<int> listSousTitreNonNull = model.DictionarySousTitre.Values.Where(t => t != -1).ToList();
                    List<int> listSupplementNonNull = model.DictionarySupplements.Values.Where(t => t != -1).ToList();

                    _context.FilmsActeurs.RemoveRange(_context.FilmsActeurs.Where(a => a.NoFilm == model.Film.NoFilm));
                    _context.FilmsLangues.RemoveRange(_context.FilmsLangues.Where(a => a.NoFilm == model.Film.NoFilm));
                    _context.FilmsSousTitres.RemoveRange(_context.FilmsSousTitres.Where(a => a.NoFilm == model.Film.NoFilm));
                    _context.FilmsSupplements.RemoveRange(_context.FilmsSupplements.Where(a => a.NoFilm == model.Film.NoFilm));


                    foreach (int NoActeur in listActeursNonNull)
                    {
                        FilmsActeurs filmsActeurs = new FilmsActeurs()
                        {
                            NoActeur = NoActeur,
                            NoFilm = model.Film.NoFilm
                        };
                        _context.Add(filmsActeurs);
                    }
                    foreach (int NoLangue in listLanguesNonNull)
                    {
                        FilmsLangues filmsLangues = new FilmsLangues()
                        {
                            NoLangue = NoLangue,
                            NoFilm = model.Film.NoFilm
                        };
                        _context.Add(filmsLangues);
                    }
                    foreach (int NoSousTitre in listSousTitreNonNull)
                    {
                        FilmsSousTitres filmsSousTitres = new FilmsSousTitres()
                        {
                            NoSousTitre = NoSousTitre,
                            NoFilm = model.Film.NoFilm
                        };
                        _context.Add(filmsSousTitres);
                    }
                    foreach (int NoSupplement in listSupplementNonNull)
                    {
                        FilmsSupplements filmsSupplements = new FilmsSupplements()
                        {
                            NoSupplement = NoSupplement,
                            NoFilm = model.Film.NoFilm
                        };
                        _context.Add(filmsSupplements);
                    }
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                    return Redirect(model.LienRetour);
                }
            }

            ViewData["NoCategorie"] = new SelectList(_context.Categories, "NoCategorie", "Description", model.Film).Prepend(new SelectListItem("", null));
            ViewData["NoFormat"] = new SelectList(_context.Formats, "NoFormat", "Description", model.Film).Prepend(new SelectListItem("", null));
            ViewData["NoProducteur"] = new SelectList(_context.Producteurs, "NoProducteur", "Nom", model.Film).Prepend(new SelectListItem("", null));
            ViewData["NoRealisateur"] = new SelectList(_context.Realisateurs, "NoRealisateur", "Nom", model.Film).Prepend(new SelectListItem("", null));
            ViewData["FilmOriginal"] = new List<SelectListItem>() { new SelectListItem("Non", "0"), new SelectListItem("Oui", "1", model.Film != null ? model.FilmOriginal == "1": false) };
            ViewData["VersionEtendue"] = new List<SelectListItem>() { new SelectListItem("Non", "0"), new SelectListItem("Oui", "1", model.Film != null ? model.VersionEtendue == "1" : false) };

            ViewData["NoActeur"] = new SelectList(_context.Acteurs, "NoActeur", "Nom").Prepend(new SelectListItem("", "-1"));
            ViewData["NoLangue"] = new SelectList(_context.Langues, "NoLangue", "Langue").Prepend(new SelectListItem("", "-1"));
            ViewData["NoSousTitre"] = new SelectList(_context.SousTitres, "NoSousTitre", "LangueSousTitre").Prepend(new SelectListItem("", "-1"));
            ViewData["NoSupplement"] = new SelectList(_context.Supplements, "NoSupplement", "Description").Prepend(new SelectListItem("", "-1"));

            return View(model);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Films films = await _context.Films.FirstOrDefaultAsync(m => m.NoFilm == id);

            DetailSupprimerViewModel model = new DetailSupprimerViewModel()
            {
                Film = await _context.Films.FirstOrDefaultAsync(m => m.NoFilm == id),
                ListFilmActeur = _context.FilmsActeurs.Where(a => a.NoFilm == id).Select(a => a.Acteurs.Nom).ToList(),
                ListFilmsLangues = _context.FilmsLangues.Where(a => a.NoFilm == id).Select(a => a.Langues.Langue).ToList(),
                ListFilmsSousTitres = _context.FilmsSousTitres.Where(a => a.NoFilm == id).Select(a => a.SousTitres.LangueSousTitre).ToList(),
                ListFilmsSupplements = _context.FilmsSupplements.Where(a => a.NoFilm == id).Select(a => a.Supplements.Description).ToList(),
                NomEmprunter = (await _userManager.FindByIdAsync(films.NoUtilisateurMAJ)).UserName,
                NomProprietaire = (await _userManager.FindByIdAsync(_context.Exemplaires.FirstOrDefault(a => a.NoExemplaire.ToString().Substring(0, 6) == id.ToString()).NoUtilisateurProprietaire)).UserName,
                LienRetour = Request.Headers["Referer"].ToString(),
                Categorie = films.NoCategorie != null ? _context.Categories.FirstOrDefault(c => c.NoCategorie == films.NoCategorie).Description : "",
                Format = films.Formats != null ? _context.Formats.FirstOrDefault(c => c.NoFormat == films.NoFormat).Description : "",
                Realisateur = films.NoRealisateur != null ? _context.Realisateurs.FirstOrDefault(c => c.NoRealisateur == films.NoRealisateur).Nom : "",
                Producteur = films.NoProducteur != null ? _context.Producteurs.FirstOrDefault(c => c.NoProducteur == films.NoProducteur).Nom : ""
            };

            return View(model);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string LienRetour)
        {
            var examplaire = await _context.Exemplaires.FindAsync(Convert.ToInt32(id+"01"));
            var listEmprunt =  _context.EmpruntsFilms.Where(a => a.NoExemplaire == Convert.ToInt32(id + "01"));
            var ListFilmActeur = _context.FilmsActeurs.Where(a => a.NoFilm == id);
            var ListFilmsLangues = _context.FilmsLangues.Where(a => a.NoFilm == id);
            var ListFilmsSousTitres = _context.FilmsSousTitres.Where(a => a.NoFilm == id);
            var ListFilmsSupplements = _context.FilmsSupplements.Where(a => a.NoFilm == id);
            var films = await _context.Films.FindAsync(id);

            _context.EmpruntsFilms.RemoveRange(listEmprunt);
            _context.Exemplaires.Remove(examplaire);
            _context.FilmsActeurs.RemoveRange(ListFilmActeur);
            _context.FilmsLangues.RemoveRange(ListFilmsLangues);
            _context.FilmsSousTitres.RemoveRange(ListFilmsSousTitres);
            _context.FilmsSupplements.RemoveRange(ListFilmsSupplements);
            _context.Films.Remove(films);

            if (films.ImagePochette != null)
            {
                string wwwPath = _env.WebRootPath;
                string path = Path.Combine(wwwPath, "images", films.ImagePochette);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            await _context.SaveChangesAsync();

            Utilisateurs utilisateursActuel = await _userManager.GetUserAsync(HttpContext.User);
            var ListIdUtilisateur = _context.UtilisateursPreferences.Where(a => a.NoPreference == 5).Select(a => a.NoUtilisateur);
            var EmailUtilisateurs = _userManager.Users.Where(a => ListIdUtilisateur.Contains(a.Id)).Select(a => a.Email);

            if (EmailUtilisateurs.Count() > 0)
            {
                return RedirectToAction("Message", new
                {
                    Destinataire = String.Join(", ", EmailUtilisateurs.ToArray()),
                    Sujet = "Retrait d'un DVD",
                    Corps =  $"L'utilisateur {utilisateursActuel.UserName} à retirer le DVD suivant :  {films.TitreFrancais}"
                });
            }

            return Redirect(LienRetour);
        }

        private bool FilmsExists(int id)
        {
            return _context.Films.Any(e => e.NoFilm == id);
        }
    }
}
