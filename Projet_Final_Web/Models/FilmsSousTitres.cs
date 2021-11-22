using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class FilmsSousTitres
    {
        [ForeignKey("Films")]
        public int NoFilm { get; set; }
        [ForeignKey("SousTitres")]
        public int NoSousTitre { get; set; }

        public virtual Films Films { get; set; }
        public virtual SousTitres SousTitres { get; set; }
    }
}
