using GestionSalleEmit.Models;
using GestionSalleEmit.DTOs.Salle;

namespace GestionSalleEmit.Services
{
    /// <summary>
    /// Interface du service Salles gérant uniquement la logique de création et de modification.
    /// </summary>
    public interface ISallesService
    {
        // Méthode pour la logique du POST (Création)
        Task<Salle> CreateSalleAsync(SalleCreateDTO createDTO);

        // Méthode pour la logique du PUT (Modification)
        Task<bool> UpdateSalleAsync(int id, SalleUpdateDTO updateDTO);
    }
}