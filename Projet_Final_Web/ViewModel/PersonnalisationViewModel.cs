
using System.ComponentModel.DataAnnotations;

namespace Projet_Final_Web.ViewModel
{
    public class PersonnalisationViewModel
    {

        [Display(Name = "Changer de mot de passe?")]
        public bool Is_Changing_Password { get; set; }

        public string Prenom { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mot de passe et confirmation de mot de passe ne sont pas identique!")]
        [Display(Name = "Veuillez confirmer le mot de passe")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Recevoir un courriel lors d'ajout de DVD?")]
        public bool Courriel_Sur_Ajout { get; set; }

        [Required]
        [Display(Name = "Recevoir un courriel lors du retrait d'un DVD?")]
        public bool Courriel_sur_Retrait { get; set; }

        [Required]
        [Display(Name = "Recevoir un courriel lors de l'emprunt d'un DVD?")]
        public bool Courriel_sur_Appropriation { get; set; }

        [Range(6, 99, ErrorMessage = "Ajouter un nombre valide(6-99")]
        [Display(Name = "Nombre de DVDs par page (6-99)")]
        public int NbDVDParPage { get; set; }


    }
}
