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
        private static List<string> lstId;

        public EnvoiMessageController(DbContextProjetFinal context, UserManager<Utilisateurs> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public IActionResult Index()
        {
            Message model = new Message();
            lstId = new List<string>();
            model.ListUtilisateurs = lstId;
            ViewData["NoUtilisateur"] = new SelectList(_userManager.Users, "Id", "Email").Prepend(new SelectListItem("", null));
            ViewData["lstUtil"] = new SelectList(_userManager.Users, "Id", "Email").Prepend(new SelectListItem("", null));
            return View(model);
        }

        public async Task<IActionResult> EnvoiUtil(int? id)
        {
            Message model = new Message();
            lstId = new List<string>();
            lstId.Add((await _userManager.FindByIdAsync(Convert.ToString(id))).Email);
            model.ListUtilisateurs = lstId;
            model.Specific = true;
            //ViewData["lstUtil"] = new SelectList(_userManager.Users.Where(u=>u.Id == Convert.ToString(id)), "Id", "Email");

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Message model)
        {
            model.ListUtilisateurs.Clear();
            if (model.UserId != null)
            {
                string util = model.UserId;
                lstId.Add((await _userManager.FindByIdAsync(util)).Email);
                model.ListUtilisateurs = lstId;

                ViewData["NoUtilisateur"] = new SelectList(_userManager.Users, "Id", "Email").Prepend(new SelectListItem("", null));
                ViewData["lstUtil"] = new SelectList(_userManager.Users.Where(u=>!model.ListUtilisateurs.Contains(u.Email)), "Id", "Email").Prepend(new SelectListItem("", null));
            }
            
            /*
            if (ModelState.IsValid)
            {
                return RedirectToAction("MessageEnvoye", model);
            } */

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MessageEnvoye(Message model)
        {
            if (!model.AllUtilisateurs)
                model.ListUtilisateurs = lstId;
            else
            {
                model.ListUtilisateurs.Clear();
                for (int i = 1; i < _userManager.Users.Count() + 1; i++)
                {
                    model.ListUtilisateurs.Add((await _userManager.FindByIdAsync(Convert.ToString(i))).Email);
                }                
            }                

            return View(model);
        }
    }
}
