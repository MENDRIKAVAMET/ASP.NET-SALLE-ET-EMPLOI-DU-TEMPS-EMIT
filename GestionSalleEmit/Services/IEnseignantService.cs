using GestionSalleEmit.DTOs.Enseignant;

namespace GestionSalleEmit.Services
{
    public interface IEnseignantService
    {
        Task<List<EnseignantResponseDTO>> GetAllAsync();
        Task<EnseignantResponseDTO?> GetByIdAsync(int id);
        Task<EnseignantResponseDTO> CreateAsync(EnseignantCreateDTO dto);
        Task<EnseignantResponseDTO?> UpdateAsync(int id, EnseignantCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<List<EnseignantResponseDTO>> SearchAsync(string nom, string prenom);
        Task<List<bool>> IsDisponibleAsync(int idEnseignant, DateTime jour, TimeSpan heureDebut, TimeSpan heureFin);
        Task<List<EnseignantResponseDTO>> GetByMatiereAsync(int idMatiere);
        Task<List<EnseignantResponseDTO>> GetPlanningAsync(int idEnseignant);

    }
}
