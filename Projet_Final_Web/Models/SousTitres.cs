using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class SousTitres
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoSousTitre { get; set; }
        [Column("LangueSousTitre", TypeName = "nvarchar(10)")]
        public string LangueSousTitre { get; set; }
        public virtual ICollection<FilmsSousTitres> FilmsSousTitres { get; set; }
    }
}
