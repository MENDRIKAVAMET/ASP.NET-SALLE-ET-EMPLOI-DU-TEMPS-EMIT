namespace GestionSalleEmit.Models
{
    public class Niveau
    {
        public int IdNiveau { get; set; }

        public string NomNiveau { get; set; }

        public int IdParcours { get; set; }
        public Parcours Parcours { get; set; }


        public ICollection<EmploiDuTemps> EmploisDuTemps { get; set; }
    }
}
