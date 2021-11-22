﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class Producteurs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoProducteur { get; set; }
        [Column("Nom", TypeName = "nvarchar(50)")]
        public string Nom { get; set; }
        public virtual ICollection<Films> Films { get; set; }

    }
}
