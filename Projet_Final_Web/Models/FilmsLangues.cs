using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class FilmsLangues
    {
        [ForeignKey("Films")]
        public int NoFilm { get; set; }
        [ForeignKey("Langues")]
        public int NoLangue { get; set; }

        public virtual Films Films { get; set; }
        public virtual Langues Langues { get; set; }
    }
}
