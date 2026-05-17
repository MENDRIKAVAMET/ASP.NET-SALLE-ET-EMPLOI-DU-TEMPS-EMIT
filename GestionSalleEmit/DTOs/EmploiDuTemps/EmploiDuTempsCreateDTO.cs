using System.ComponentModel.DataAnnotations;

namespace GestionSalleEmit.DTOs.EmploiDuTemps
{
    public class EmploiDuTempsCreateDTO
    {
        [Required(ErrorMessage = "Le jour est obligatoire")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Le jour doit contenir entre 2 et 20 caractères")]
        public string Jour { get; set; }

        [Required(ErrorMessage = "L'heure de début est obligatoire")]
        public TimeSpan HeureDebut { get; set; }

        [Required(ErrorMessage = "L'heure de fin est obligatoire")]
        public TimeSpan HeureFin { get; set; }

        [Required(ErrorMessage = "Le semestre est obligatoire")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Le semestre doit contenir entre 1 et 50 caractères")]
        public string Semestre { get; set; }

        [Required(ErrorMessage = "L'ID de la salle est obligatoire")]
        public int IdSalle { get; set; }

        [Required(ErrorMessage = "L'ID de l'enseignant est obligatoire")]
        public int IdEnseignant { get; set; }

        [Required(ErrorMessage = "L'ID de la matière est obligatoire")]
        public int IdMatiere { get; set; }

        [Required(ErrorMessage = "L'ID du niveau est obligatoire")]
        public int IdNiveau { get; set; }
    }
}
