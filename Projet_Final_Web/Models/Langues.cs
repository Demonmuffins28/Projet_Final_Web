using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class Langues
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoLangue { get; set; }
        [Column("Langue", TypeName = "nvarchar(10)")]
        public string Langue { get; set; }

        public virtual ICollection<FilmsLangues> FilmsLangues { get; set; }

    }
}
