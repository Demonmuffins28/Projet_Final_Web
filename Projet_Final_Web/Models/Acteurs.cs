using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class Acteurs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoActeur { get; set; }
        [Column("Nom", TypeName = "nvarchar(50)")]
        public string Nom { get; set; }
        [Column("Sexe", TypeName = "nchar(1)")]
        public string Sexe { get; set; }

        public virtual ICollection<FilmsActeurs> FilmsActeurs { get; set; }
    }
}
