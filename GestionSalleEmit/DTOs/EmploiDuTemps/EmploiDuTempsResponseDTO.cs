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
    }
}
