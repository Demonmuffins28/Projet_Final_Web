using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class Categories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoCategorie { get; set; }
        [Column("Description", TypeName = "nvarchar(50)")]
        public string Description { get; set; }
        public virtual ICollection<Films> Films { get; set; }
    }
}
