using Microsoft.AspNetCore.Mvc.Rendering;
using Projet_Final_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class DVDViewModel
    {

        public List<Films> listDVD { get; set; }

        public int page { get; set; }

        public int nbPage { get; set; }

        public string TitreRechercher { get; set; }

        public Utilisateurs utilisateursActuel { get; set; }

        public int TrierPar { get; set; } = 1;

        public List<SelectListItem> ListTrie { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Noms d’utilisateurs"},
            new SelectListItem { Value = "2", Text = "Titres français de DVD"},
            new SelectListItem { Value = "3", Text = "Noms d’utilisateurs et Titres français de DVD"},
        };
    }
}
