using GestionSalleEmit.Models;
using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.Parcours
{
    public class ParcoursCreateDTO
    {
        [Required(ErrorMessage = "Le nom du parcours est obligatoire.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom du parcours doit contenir entre 2 et 100 caractères.")]
        public string NomParcours { get; set; }

        [Required(ErrorMessage = "L'identifiant de la filière est obligatoire.")]
        [Range(1, int.MaxValue, ErrorMessage = "L'identifiant de la filière doit être un entier positif.")]
        public int IdFiliere { get; set; }
    }
}
