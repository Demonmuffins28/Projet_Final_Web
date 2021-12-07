using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projet_Final_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class AjoutEditDVDViewModel
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


        public string FilmOriginal { get; set; }
        public string VersionEtendue { get; set; }
        public Dictionary<int, int> DictionaryLangues { get; private set; } = new Dictionary<int, int> {
            {1,-1},{2,-1},{3,-1}
        };
        public Dictionary<int, int> DictionarySousTitre { get; private set; } = new Dictionary<int, int> {
            {1,-1},{2,-1},{3,-1}
        };
        public Dictionary<int, int> DictionaryActeurs { get; private set; } = new Dictionary<int, int> {
            {1,-1},{2,-1},{3,-1}
        };
        public Dictionary<int, int> DictionarySupplements { get; private set; } = new Dictionary<int, int> {
            {1,-1},{2,-1},{3,-1}
        };

        public IFormFile image { get; set; }
    }
}
