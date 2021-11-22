using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class TypesUtilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("TypeUtilisateur", TypeName = "nchar(1)")]
        public string TypeUtilisateur { get; set; }
        [Column("Description", TypeName = "nvarchar(25)")]
        public string Description { get; set; }
    }
}
