using Projet_Final_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class DetailSupprimerViewModel
    {
        public Films Film { get; set; }

        public List<string> ListFilmActeur { get; set; }
        public List<string> ListFilmsLangues { get; set; }
        public List<string> ListFilmsSousTitres { get; set; }
        public List<string> ListFilmsSupplements { get; set; }
        public string LienRetour { get; set; }

        public string NomEmprunter { get; set; }
        public string NomProprietaire { get; set; }
        public string Categorie { get; set; }
        public string Format { get; set; }
        public string Realisateur { get; set; }
        public string Producteur { get; set; }
    }
}
