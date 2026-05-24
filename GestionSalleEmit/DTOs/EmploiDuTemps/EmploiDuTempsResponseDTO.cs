using GestionSalleEmit.DTOs.Enseignant;
using GestionSalleEmit.DTOs.Matiere;
using GestionSalleEmit.DTOs.Niveau;
using GestionSalleEmit.DTOs.Salle;

namespace GestionSalleEmit.DTOs.EmploiDuTemps
{
    public class EmploiDuTempsResponseDTO
    {
        public int IdEDT { get; set; }
        public string Jour { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
        public string Semestre { get; set; }
        public int IdSalle { get; set; }
        public int IdEnseignant { get; set; }
        public int IdMatiere { get; set; }
        public int IdNiveau { get; set; }
        public string? Salle { get; set; }
        public string? Enseignant { get; set; }
        public string? Matiere { get; set; }
        public string? Niveau { get; set; }
        public int IdFiliere { get; set; }
        public string? Filiere { get; set; }
        public int IdParcours { get; set; }
        public string? Parcours { get; set; }

    }
}
