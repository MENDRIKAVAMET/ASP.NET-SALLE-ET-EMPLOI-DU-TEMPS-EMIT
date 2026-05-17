using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs
{
    public class NiveauDTO
    {
        public int IdNiveau { get; set; }

        [Required(ErrorMessage = "Le nom du niveau est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string NomNiveau { get; set; }

        [Required(ErrorMessage = "L'ID de la filière est obligatoire")]
        public int IdFiliere { get; set; }
    }
}
