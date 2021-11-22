using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_Final_Web.Models
{
    public class FilmsActeurs
    {
        [ForeignKey("Films")]
        public int NoFilm {get; set;}
        [ForeignKey("Acteurs")]
        public int NoActeur { get; set; }

        public virtual Films Films { get; set; }
        public virtual Acteurs Acteurs { get; set; }

    }
}
