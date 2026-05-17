namespace GestionSalleEmit.Models
{
    public class Matiere
    {
        public int IdMatiere { get; set; }
        public string CodeMatiere { get; set; }
        public string NomMatiere { get; set; }
        public int VolumeHoraire { get; set; }
        public ICollection<Enseigner> Enseigners { get; set; }
        public ICollection<EmploiDuTemps> EmploisDuTemps { get; set; }
    }
}
