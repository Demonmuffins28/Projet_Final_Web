using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Final_Web.Models;
using Projet_Final_Web.ViewModel;
using System.Security.Claims;

namespace Projet_Final_Web.Controllers
{
   // [Authorize(Policy = "AdminOnly")]
    public class GestionUtilisateurController : Controller
    {
        private readonly DbContextProjetFinal _context;
        private readonly SignInManager<Utilisateurs> _signInManager;
        private readonly UserManager<Utilisateurs> _userManager;
        private List<GestionUtilisateurViewModel> lstUtilisateurs;

        public GestionUtilisateurController(DbContextProjetFinal context, SignInManager<Utilisateurs> signInManager, UserManager<Utilisateurs> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            lstUtilisateurs = GetUtilisateurs();
        }

        [NonAction]
        private List<GestionUtilisateurViewModel> GetUtilisateurs()
        {
            List<GestionUtilisateurViewModel> tempList = new List<GestionUtilisateurViewModel>();

            foreach (Utilisateurs u in _userManager.Users.Where(u => u.TypesUtilisateurID != "A").ToList())
            {

                GestionUtilisateurViewModel newUser = new GestionUtilisateurViewModel(u.Id, u.UserName, u.Email, u.TypesUtilisateurID);
                tempList.Add(newUser);
            }
            return tempList;
        }


        // GET: GestionUtilisateur
        public IActionResult Index()
        {
            //Check si le user est un admin
            if (_userManager.GetUserName(User) != "admin")
                return RedirectToAction("index", "DVD");
            else return View(lstUtilisateurs);
        }

        // GET: GestionUtilisateur/Create
        public IActionResult Create()
        {
            if (_userManager.GetUserName(User) != "admin")
                return RedirectToAction("index", "DVD");
            ViewData["typeUtilisateur"] = new SelectList(_context.TypesUtilisateur.Where(t => t.TypeUtilisateur != "A"), "TypeUtilisateur", "Description");
            return View();
        }

        // POST: GestionUtilisateur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Prenom, Courriel, ConfirmCourriel, Password, ConfirmPassword, TypeUtilisateur")] RegisterViewModel model)
        {
            if (_userManager.GetUserName(User) != "admin")
                return RedirectToAction("index", "DVD");
            if (ModelState.IsValid)
            {
                //var Id = _userManager.Users.Select(u => u.Id).ToList();
                var userExist = await _userManager.FindByEmailAsync(model.Courriel);
                if (userExist is null)
                {
                    int newId = _userManager.Users.Select(a => Convert.ToInt32(a.Id)).Max() + 1;
                    var user = new Utilisateurs
                    {
                        Id = newId.ToString(),
                        UserName = model.Prenom,
                        Email = model.Courriel,
                        TypesUtilisateurID = model.TypeUtilisateur

                    };
                    // Store user data in AspNetUsers database table
                    var result = await _userManager.CreateAsync(user, model.Password);
                    // If user is successfully created, sign-in the user using. SignInManager and redirect to index action of HomeController
                    if (result.Succeeded)
                    {

                        //Ajouter les préférences de base
                        UtilisateursPreferences prefAjout = new UtilisateursPreferences();
                        prefAjout.NoUtilisateur = user.Id;
                        prefAjout.NoPreference = 3;

                        UtilisateursPreferences prefAppropriation = new UtilisateursPreferences();
                        prefAppropriation.NoUtilisateur = user.Id;
                        prefAppropriation.NoPreference = 4;

                        UtilisateursPreferences prefRetrait = new UtilisateursPreferences();
                        prefRetrait.NoUtilisateur = user.Id;
                        prefRetrait.NoPreference = 5;

                        _context.UtilisateursPreferences.Add(prefAjout);
                        _context.UtilisateursPreferences.Add(prefAppropriation);
                        _context.UtilisateursPreferences.Add(prefRetrait);
                        _context.SaveChanges();

                        // TODO: Envoyer un courriel

                        return RedirectToAction("index", "GestionUtilisateur");
                    }
                    // If there are any errors, add them to the ModelState object. which will be displayed by the validation summary tag helper
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Adresse courriel deja utilisé!");
                }
  
            }
            ViewData["typeUtilisateur"] = new SelectList(_context.TypesUtilisateur.Where(t => t.TypeUtilisateur != "A"), "TypeUtilisateur", "Description");
            return View(model);
        }

        // GET: GestionUtilisateur/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (_userManager.GetUserName(User) != "admin")
                return RedirectToAction("index", "DVD");
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();
            else
            {
                ViewData["typeUtilisateur"] = new SelectList(_context.TypesUtilisateur.Where(t => t.TypeUtilisateur != "A"), "TypeUtilisateur", "Description");
                RegisterViewModel model = new RegisterViewModel();
                model.Courriel = model.ConfirmCourriel = user.Email;
                model.Prenom = user.UserName;
                model.TypeUtilisateur = user.TypesUtilisateurID;
                return View(model);
            }
        }

        // POST: GestionUtilisateur/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Prenom, Courriel, ConfirmCourriel, Password, ConfirmPassword, TypeUtilisateur")] RegisterViewModel model)
        {
            if (_userManager.GetUserName(User) != "admin")
                return RedirectToAction("index", "DVD");

            if (ModelState.IsValid)
            {
                //Check Same Email
                var emailExist = await _userManager.FindByEmailAsync(model.Courriel);
                var currentUser = await _userManager.FindByIdAsync(id);
                if(emailExist.Id != null && emailExist.Id != currentUser.Id)
                {
                    ModelState.AddModelError(string.Empty, "Courriel deja utilisé par un autre utilisateur");
                    ViewData["typeUtilisateur"] = new SelectList(_context.TypesUtilisateur.Where(t => t.TypeUtilisateur != "A"), "TypeUtilisateur", "Description");
                    return View(model);
                }
                else
                {
                    currentUser.UserName = model.Prenom;
                    currentUser.Email = model.Courriel;
                    currentUser.TypesUtilisateurID = model.TypeUtilisateur;
                    await _userManager.RemovePasswordAsync(currentUser);
                    await _userManager.AddPasswordAsync(currentUser, model.Password);

                    var result = await _userManager.UpdateAsync(currentUser);
                }
            }

                return RedirectToAction(nameof(Index));
            
        }

        // GET: GestionUtilisateur/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (_userManager.GetUserName(User) != "admin")
                return RedirectToAction("index", "DVD");
            if (id == null)
            {
                return NotFound();
            }


            var deleteUser = await _userManager.FindByIdAsync(id);

            if (deleteUser == null)
                return RedirectToAction(nameof(Index));
            else
            {

                GestionUtilisateurViewModel model = new GestionUtilisateurViewModel();
                model.Courriel = deleteUser.Email;
                model.Prenom = deleteUser.UserName;
                model.TypeUtilisateur = deleteUser.TypesUtilisateurID;
                return View(model);
            }
        }

        // POST: GestionUtilisateur/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_userManager.GetUserName(User) != "admin")
                return RedirectToAction("index", "DVD");
            var deleteUser = await _userManager.FindByIdAsync(id);
            if(deleteUser != null) 
            {
                //Supprimer les preferences
                try
                {
                    _context.UtilisateursPreferences.RemoveRange(_context.UtilisateursPreferences.Where(p => p.NoUtilisateur == deleteUser.Id));
                    _context.ValeursPreferences.RemoveRange(_context.ValeursPreferences.Where(p => p.NoUtilisateur == deleteUser.Id));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                await _userManager.DeleteAsync(deleteUser);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
