using System.Collections.Generic;
using Projet_Final_Web.Models;
using System.ComponentModel.DataAnnotations;

namespace Projet_Final_Web.ViewModel
{
    public class Message
    {
        public bool btnEnvoye { get; set; } = false;
        public bool AllUtilisateurs { get; set; }
        public bool Specific { get; set; }
        public List<string> ListUtilisateurs { get; set; }

        public string Destinataire { get; set; }

        [Required]
        public string Sujet { get; set; }
        [Required]
        public string Corps { get; set; }
        public string UserId { get; set; }
    }
}
