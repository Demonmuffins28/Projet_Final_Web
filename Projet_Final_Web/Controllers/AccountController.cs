using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projet_Final_Web.Models;
using Projet_Final_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Utilisateurs> userManager;
        private readonly SignInManager<Utilisateurs> signInManager;
        public AccountController(UserManager<Utilisateurs> userManager, SignInManager<Utilisateurs> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "DVD");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
