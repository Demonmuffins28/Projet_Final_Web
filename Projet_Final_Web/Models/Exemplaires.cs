using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class Exemplaires
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoExemplaire { get; set; }
        [ForeignKey("Utilisateurs")]
        public string NoUtilisateurProprietaire { get; set; }
        public virtual Utilisateurs Utilisateurs { get; set; }
        public virtual ICollection<EmpruntsFilms> EmpruntsFilms { get; set; }

    }
}
