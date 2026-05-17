namespace GestionSalleEmit.Models
{
    public class EmploiDuTemps
    {
        public int IdEDT { get; set; }

        public string Jour { get; set; }

        public TimeSpan HeureDebut { get; set; }

        public TimeSpan HeureFin { get; set; }

        public string Semestre { get; set; }

        public int IdSalle { get; set; }

        public Salle Salle { get; set; }

        public int IdEnseignant { get; set; }

        public Enseignant Enseignant { get; set; }

        public int IdMatiere { get; set; }

        public Matiere Matiere { get; set; }

        public int IdNiveau { get; set; }

        public Niveau Niveau { get; set; }
    }
}
