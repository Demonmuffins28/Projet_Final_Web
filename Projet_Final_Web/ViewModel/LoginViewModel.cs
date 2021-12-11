using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Adresse courriel"), EmailAddress(ErrorMessage = "Adresse courriel invalide")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mot de passe obligatoire!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }

}
