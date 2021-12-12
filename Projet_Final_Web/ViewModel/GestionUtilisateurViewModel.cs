using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class GestionUtilisateurViewModel
    {
        public GestionUtilisateurViewModel() { }
        public GestionUtilisateurViewModel(string id, string prenom, string courriel, string typeUtilisateur)
        {
            this.Id = id;
            this.Prenom = prenom;
            this.Courriel = courriel;
            this.TypeUtilisateur = typeUtilisateur;
        }

        [Display(Name = "Nom d'utilisateur")]
        public string Id { get; set; }

        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Display(Name = "Addresse Courriel")]
        public string Courriel { get; set; }

        [Display(Name = "Type d'utilisateur")]
        public string TypeUtilisateur { get; set; }

    }
}
