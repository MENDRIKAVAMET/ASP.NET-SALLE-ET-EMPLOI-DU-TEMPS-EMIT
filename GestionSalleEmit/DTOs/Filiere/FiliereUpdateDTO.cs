using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Filiere
{
    public class FiliereUpdateDTO
    {
        [Required(ErrorMessage = "L'ID de la filière est obligatoire")]
        public int IdFiliere { get; set; }

        [Required(ErrorMessage = "Le nom de la filière est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string NomFiliere { get; set; }
    }
}
