using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs
{
    public class FiliereDTO
    {
        public int IdFiliere { get; set; }

        [Required(ErrorMessage = "Le nom de la filière est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string NomFiliere { get; set; }
    }
}
