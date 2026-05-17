namespace GestionSalleEmit.Models
{
    public class Niveau
    {
        public int IdNiveau { get; set; }

        public string NomNiveau { get; set; }

        public int IdFiliere { get; set; }

        public Filiere Filiere { get; set; }

        public ICollection<EmploiDuTemps> EmploisDuTemps { get; set; }
    }
}
