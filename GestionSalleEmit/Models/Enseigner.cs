namespace GestionSalleEmit.Models
{
    public class Enseigner
    {
        public int IdEnseignant { get; set; }

        public Enseignant Enseignant { get; set; }

        public int IdMatiere { get; set; }

        public Matiere Matiere { get; set; }
    }
}
