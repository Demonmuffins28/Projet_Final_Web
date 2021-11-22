using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class UtilisateursPreferences
    {
        [ForeignKey("Utilisateurs")]
        public string NoUtilisateur { get; set; }

        [ForeignKey("Preference")]
        public int NoPreference { get; set; }
        public virtual Utilisateurs Utilisateurs { get; set; }
        public virtual Preference Preference { get; set; }
    }
}
