using GestionSalleEmit.Models;

namespace GestionSalleEmit.DTOs.Parcours
{
    public class ParcoursResponseDTO
    {
        public int IdParcours { get; set; }
        public string NomParcours { get; set; }
        public int IdFiliere { get; set; }
        public string? Filiere { get; set; }
    }
}
