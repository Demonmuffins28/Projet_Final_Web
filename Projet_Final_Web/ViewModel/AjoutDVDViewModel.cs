using Microsoft.AspNetCore.Mvc.Rendering;
using Projet_Final_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class AjoutDVDViewModel
    {
        public int TypeAjout { get; set; }

        public bool TypeChanger { get; set; }

        public List<SelectListItem> ListTypeAjout { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Mode abrégé"},
            new SelectListItem { Value = "2", Text = "Mode complet"}
        };

        public string LienRetour { get; set; }
        public Films Film { get; set; }
        public Dictionary<int, string> DictionaryNomFilm { get; private set; } = new Dictionary<int, string> {
            {1,""},{2,""},{3,""},{4,""},{5,""},{6,""},{7,""},{8,""},{9,""},{10,""}
        };

    }
}
