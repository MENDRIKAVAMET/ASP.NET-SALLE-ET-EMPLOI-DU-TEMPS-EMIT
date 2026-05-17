namespace GestionSalleEmit.Models{
    public class Enseignant
    {
        public int IdEnseignant { get; set; }
        public string NomEnseignant { get; set; }
        public string PrenomEnseignant { get; set; }
        public string EmailEnseignant { get; set; }
        public string PhoneEnseignant { get; set; }
        public string GradeEnseignant { get; set; }
        public ICollection<Enseigner> Enseigners { get; set; }
        public ICollection<EmploiDuTemps> EmploisDuTemps { get; set; }
    }
}
