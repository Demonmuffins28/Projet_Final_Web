using System.Collections.Generic;
using Projet_Final_Web.Models;
using System.ComponentModel.DataAnnotations;

namespace Projet_Final_Web.ViewModel
{
    public class MessageViewModel
    {
        public bool AllUtilisateurs { get; set; }
        public bool Specific { get; set; }
        public List<int> ListUtilisateurs { get; set; }
        [Required]
        public string Sujet { get; set; }
        [Required]
        public string Corps { get; set; }
        public Utilisateurs Utilisateur { get; set; }
    }
}
