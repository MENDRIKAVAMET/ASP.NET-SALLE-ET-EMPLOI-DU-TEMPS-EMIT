using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Filiere
{
    public class FiliereCreateDTO
    {
        [Required(ErrorMessage = "Le nom de la filière est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string NomFiliere { get; set; }
    }
}
