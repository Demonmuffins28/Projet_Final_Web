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
    public class EnvoiMessageController : Controller
    {
        private readonly DbContextProjetFinal _context;
        private readonly UserManager<Utilisateurs> _userManager;
        private IWebHostEnvironment _env;

        public EnvoiMessageController(DbContextProjetFinal context, UserManager<Utilisateurs> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public IActionResult Index()
        {
            MessageViewModel model = new MessageViewModel();
            model.ListUtilisateurs = new List<int>();
            //TempData["dest"] = model.ListUtilisateurs;
            ViewData["NoUtilisateur"] = new SelectList(_userManager.Users, "Id", "Email").Prepend(new SelectListItem("", null));            
            return View(model);
        }

        public IActionResult EnvoiUtil(int? id)
        {
            MessageViewModel model = new MessageViewModel();
            model.ListUtilisateurs = new List<int>();
            model.Specific = true;
            ViewData["NoUtilisateur"] = new SelectList(_userManager.Users.Where(u=>u.Id == Convert.ToString(id)), "Id", "Email");

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Index(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("MessageEnvoye", model);
            } 
            else if (model.Specific)
            {
                int util = Convert.ToInt32(model.Utilisateur.Id);
                //model.ListUtilisateurs = (TempData["dest"] as List<int>);
                model.ListUtilisateurs.Add(util);
                //TempData["dest"] = model.ListUtilisateurs.ToList();

                ViewData["NoUtilisateur"] = new SelectList(_userManager.Users/*.Where(u=>u.Id)*/, "Id", "Email").Prepend(new SelectListItem("", null));
            } else
            {
                model.Specific = false;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult MessageEnvoye(MessageViewModel model)
        {
            return View(model);
        }
    }
}
