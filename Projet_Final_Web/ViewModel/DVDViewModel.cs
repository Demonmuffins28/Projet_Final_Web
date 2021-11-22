using Projet_Final_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.ViewModel
{
    public class DVDViewModel
    {
        public List<Tuple<Films, int>> listDVD { get; set; }

        public int page { get; set; }

        public int nbPage { get; set; }

        public string TitreRechercher { get; set; }

        public Utilisateurs utilisateursActuel { get; set; }
    }
}
