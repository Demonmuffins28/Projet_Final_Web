using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class RegisterViewModel
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "Champs requis!")]
        [DataType(DataType.Text)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Champs requis!")]
        [DataType(DataType.Text)]
        [Display(Name = "Adresse courriel")]
        [EmailAddress(ErrorMessage = "Veuillez entrer une adresse courriel valide")]
        public string Courriel { get; set; }

        [Required(ErrorMessage = "Champs requis!")]
        [Compare("Courriel", ErrorMessage = "Courriel et confirmation de courriel ne sont pas identique!")]
        [Display(Name = "Veuillez confirmer l'adresse courriel")]
        [EmailAddress(ErrorMessage = "Veuillez entrer une adresse courriel valide")]
        public string ConfirmCourriel { get; set; }

        [Required(ErrorMessage = "Champs requis!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Champs requis!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mot de passe et confirmation de mot de passe ne sont pas identique!")]
        [Display(Name = "Veuillez confirmer le mot de passe")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Champs requis!")]
        [Display(Name = "Type d'utilisateur")]
        public string TypeUtilisateur { get; set; }
    }
}
