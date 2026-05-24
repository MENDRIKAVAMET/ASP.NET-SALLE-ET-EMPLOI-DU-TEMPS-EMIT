using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Auth
{
    public class UpdateUtilisateurDTO
    {
        [Required]
        public int IdUtilisateur { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}