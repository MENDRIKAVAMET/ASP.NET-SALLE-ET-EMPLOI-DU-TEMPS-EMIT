using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs
{
    public class SalleDTO
    {
        public int IdSalle { get; set; }

        [Required(ErrorMessage = "Le nom de la salle est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string NomSalle { get; set; }

        [Required(ErrorMessage = "La capacité est obligatoire")]
        [Range(1, 1000, ErrorMessage = "La capacité doit être entre 1 et 1000 personnes")]
        public int Capacite { get; set; }

        [Required(ErrorMessage = "Le type de salle est obligatoire")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le type doit contenir entre 2 et 50 caractères")]
        public string TypeSalle { get; set; }
    }
}
