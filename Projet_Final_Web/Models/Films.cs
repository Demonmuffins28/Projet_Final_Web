using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Final_Web.Models
{
    public class Films
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoFilm { get; set; }
        [Range(1750, 3000)]
        public int? AnneeSortie { get; set; }
        [ForeignKey("Categories")]
        public int? NoCategorie { get; set; }
        [ForeignKey("Formats")]
        public int? NoFormat { get; set; }
        public DateTime DateMAJ { get; set; }
        [ForeignKey("Utilisateurs")]
        public string NoUtilisateurMAJ { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        [MaxLength(500)]
        public string? Resume { get; set; }
        [Range(1, 2000)]
        public int? DureeMinutes { get; set; }
        public bool? FilmOriginal { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [MaxLength(50)]
        public string ImagePochette { get; set; }
        [Range(1, 250)]
        public int? NbDisques { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Le titre francais de DVD est obligatoire")]
        public string TitreFrancais { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? TitreOriginal { get; set; }
        public bool? VersionEtendue { get; set; }
        [ForeignKey("Realisateurs")]
        public int? NoRealisateur { get; set; }
        [ForeignKey("Producteurs")]
        public int? NoProducteur { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Xtra { get; set; }

        public virtual Categories Categories { get; set; }
        public virtual Formats Formats { get; set; }
        public virtual Utilisateurs Utilisateurs { get; set; }
        public virtual Realisateurs Realisateurs { get; set; }
        public virtual Producteurs Producteurs { get; set; }

        public virtual ICollection<FilmsActeurs> FilmsActeurs { get; set; }
        public virtual ICollection<FilmsLangues> FilmsLangues { get; set; }
        public virtual ICollection<FilmsSousTitres> FilmsSousTitres { get; set; }
        public virtual ICollection<FilmsSupplements> FilmsSupplements { get; set; }

    }
}
