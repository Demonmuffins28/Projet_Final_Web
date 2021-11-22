using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class FilmsSupplements
    {
        [ForeignKey("Films")]
        public int NoFilm { get; set; }
        [ForeignKey("Supplements")]
        public int NoSupplement { get; set; }

        public virtual Films Films { get; set; }
        public virtual Supplements Supplements { get; set; }
    }
}
