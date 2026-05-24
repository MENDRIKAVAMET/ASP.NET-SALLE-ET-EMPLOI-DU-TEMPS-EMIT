using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
        public string Nom { get; set; }
        
        [Required(ErrorMessage = "L'email est requis.")]
        [StringLength(100, ErrorMessage = "L'email doit contenir au maximum 100 caractères.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit contenir entre 8 et 100 caractères.")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La confirmation du mot de passe doit contenir entre 8 et 100 caractères.")]
        [DataType(DataType.Password)]
        [Compare("MotDePasse", ErrorMessage = "Le mot de passe et la confirmation du mot de passe ne correspondent pas.")]
        public string ConfirmMotDePasse { get; set; }
        [Required(ErrorMessage = "Le rôle est requis.")]
        public string Role { get; set; }
    }
}
