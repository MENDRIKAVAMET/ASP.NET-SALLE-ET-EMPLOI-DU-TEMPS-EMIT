using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Matiere
{
    public class MatiereCreateDTO
    {
        [Required(ErrorMessage = "Le code de la matière est obligatoire")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le code doit contenir entre 2 et 50 caractères")]
        public string CodeMatiere { get; set; }

        [Required(ErrorMessage = "Le nom de la matière est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string NomMatiere { get; set; }

        [Required(ErrorMessage = "Le volume horaire est obligatoire")]
        [Range(1, 1000, ErrorMessage = "Le volume horaire doit être entre 1 et 1000")]
        public int VolumeHoraire { get; set; }
    }
}
