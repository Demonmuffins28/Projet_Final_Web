using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class Utilisateurs : IdentityUser
    {
        public virtual ICollection<EmpruntsFilms> EmpruntsFilms { get; set; }
        public virtual ICollection<Exemplaires> Exemplaires { get; set; }
        public virtual ICollection<UtilisateursPreferences> UtilisateursPreferences { get; set; }
        public virtual ICollection<Films> Films { get; set; }


        [ForeignKey("TypesUtilisateur")]
        public string TypesUtilisateurID { get; set; }
        public virtual TypesUtilisateur TypesUtilisateur { get; set; }

    }
}
