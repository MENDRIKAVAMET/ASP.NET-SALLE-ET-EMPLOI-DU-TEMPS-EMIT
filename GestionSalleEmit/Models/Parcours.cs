namespace GestionSalleEmit.Models
{
    public class Parcours
    {
        public int IdParcours { get; set; }
        public string NomParcours { get; set; }
        public int IdFiliere { get; set; }
        public Filiere Filiere { get; set; }
        public ICollection<Niveau> Niveaux { get; set; }
    }
}
