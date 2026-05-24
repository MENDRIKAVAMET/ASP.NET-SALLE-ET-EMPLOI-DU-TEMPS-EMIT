using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        [StringLength(100, ErrorMessage = "L'email doit contenir au maximum 100 caractères.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères.")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }
    }
}
