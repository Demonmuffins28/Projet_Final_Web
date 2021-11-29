using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class UtilisateurViewModel
    {
        public UtilisateurViewModel(string id, string prenom, string courriel, int nbDVD)
        {
            this.Id = id;
            this.Prenom = prenom;
            this.Courriel = courriel;
            this.NbDVD = nbDVD;
        }

        public string Id { get; set; }

        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Display(Name = "Addresse Courriel")]
        public string Courriel { get; set; }

        [Display(Name = "Nombre de DVD empruntés")]
        public int NbDVD { get; set; }

    }
}
