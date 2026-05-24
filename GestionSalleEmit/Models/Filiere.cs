namespace GestionSalleEmit.Models
{
    public class Filiere
    {
        public int IdFiliere { get; set; }
        public string NomFiliere { get; set; }
        public ICollection<Parcours> Parcours { get; set; }
        public ICollection<Niveau> Niveaux { get; set; }
    }
}
