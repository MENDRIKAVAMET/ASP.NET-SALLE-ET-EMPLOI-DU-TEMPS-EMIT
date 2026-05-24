using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Enseigner
{
    public class EnseignerUpdateDTO
    {
        [Required(ErrorMessage = "L'identifiant de l'enseignant est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "L'identifiant de l'enseignant doit être un entier positif")]
        public int IdEnseignant { get; set; }

        [Required(ErrorMessage = "L'identifiant de la matière est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "L'identifiant de la matière doit être un entier positif")]
        public int IdMatiere { get; set; }
    }
}
