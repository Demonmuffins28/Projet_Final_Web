using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projet_Final_Web.Models;

namespace Projet_Final_Web.ViewModel
{
    public class DVDEnMainViewModel
    {
        public List<Tuple<Films, int>> listDVD { get; set; }

        public int page { get; set; }

        public int nbPage { get; set; }

        public Utilisateurs utilisateursActuel { get; set; }
    }
}
