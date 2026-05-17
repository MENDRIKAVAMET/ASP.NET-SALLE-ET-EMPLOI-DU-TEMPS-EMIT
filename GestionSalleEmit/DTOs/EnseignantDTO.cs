using System.ComponentModel.DataAnnotations;
namespace GestionSalleEmit.DTOs
{
    public class EnseignantDTO
    {
        public int IdEnseignant { get; set; }

        [Required(ErrorMessage = "Le nom de l'enseignant est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string NomEnseignant { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères")]
        public string PrenomEnseignant { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string EmailEnseignant { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire")]
        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide")]
        public string PhoneEnseignant { get; set; }

        [Required(ErrorMessage = "Le grade est obligatoire")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le grade doit contenir entre 2 et 50 caractères")]
        public string GradeEnseignant { get; set; }
    }
}
