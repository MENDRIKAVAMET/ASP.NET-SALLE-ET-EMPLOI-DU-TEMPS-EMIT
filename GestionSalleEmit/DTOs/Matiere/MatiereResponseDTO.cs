namespace GestionSalleEmit.DTOs.Matiere
{
    public class MatiereResponseDTO
    {
        public int IdMatiere { get; set; }
        public string Semestre { get; set; }
        public string NomMatiere { get; set; }
        public int VolumeHoraire { get; set; }
        public int Coefficient { get; set; }
    }
}
