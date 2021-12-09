
using System.ComponentModel.DataAnnotations;

namespace Projet_Final_Web.ViewModel
{
    public class PersonnalisationViewModel
    {

        public string Prenom { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mot de passe et confirmation de mot de passe ne sont pas identique!")]
        [Display(Name = "Veuillez confirmer le mot de passe")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool Courriel_Sur_Ajout { get; set; }

        [Required]
        public bool Courriel_sur_Retrait { get; set; }

        [Required]
        public bool Courriel_sur_Appropriation { get; set; }

        [Range(6, 99, ErrorMessage = "Please enter valid integer Number")]
        [Display(Name = "Nombre de DVDs par page(6-99)")]
        public int NbDVDParPage { get; set; }


    }
}
