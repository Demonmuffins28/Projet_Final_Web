using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class EmpruntsFilms
    {
        [ForeignKey("Exemplaires")]
        public int NoExemplaire { get; set; }
        [ForeignKey("Utilisateurs")]
        public string NoUtilisateur { get; set; }
        public DateTime DateEmprunt { get; set; }
        public virtual Utilisateurs Utilisateurs { get; set; }
        public virtual Exemplaires Exemplaires { get; set; }
    }
}
