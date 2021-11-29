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
    public class ListeEmpruntsController : Controller
    {

        private readonly DbContextProjetFinal _context;
        private readonly SignInManager<Utilisateurs> _signInManager;
        private readonly UserManager<Utilisateurs> _userManager;
        private List<UtilisateurViewModel> model;

        public ListeEmpruntsController(DbContextProjetFinal context, SignInManager<Utilisateurs> signInManager, UserManager<Utilisateurs> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            model = GetUtilisateurs();
        }


        public IActionResult Index()
        {
            return View(model);
        }

        [NonAction]
        private List<UtilisateurViewModel> GetUtilisateurs()
        {
            List<UtilisateurViewModel> tempList = new List<UtilisateurViewModel>();

            foreach (Utilisateurs u in _userManager.Users.Where(u => u.TypesUtilisateurID != "A").ToList())
            {

                int nbDVD = _context.Films.Where(f => f.NoUtilisateurMAJ == u.Id).Count();

                UtilisateurViewModel newUser = new UtilisateurViewModel(u.Id, u.UserName, u.Email, nbDVD);
                tempList.Add(newUser);
            }
            return tempList;
        }
    }
}
