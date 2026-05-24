using GestionSalleEmit.DTOs.Niveau;

namespace GestionSalleEmit.Services
{
    public interface INiveauService
    {
        // =========================
        // CRUD
        // =========================
        Task<List<NiveauResponseDTO>> GetAllAsync();

        Task<NiveauResponseDTO?> GetByIdAsync(int id);

        Task<NiveauResponseDTO> CreateAsync(NiveauCreateDTO dto);

        Task<NiveauResponseDTO?> UpdateAsync(int id, NiveauCreateDTO dto);

        Task<bool> DeleteAsync(int id);

        // =========================
        // SEARCH
        // =========================
        Task<List<NiveauResponseDTO>> SearchAsync(string nomNiveau = "");

        // =========================
        // VALIDATIONS
        // =========================
        Task<bool> ExistsAsync(int id);

        Task<bool> NomExisteAsync(string nomNiveau);

        // =========================
        // RELATIONS MÉTIER
        // =========================

        // Matières d’un niveau
        Task<List<NiveauResponseDTO>> GetMatieresAsync(int idNiveau);

        // Emploi du temps d’un niveau
        Task<List<NiveauResponseDTO>> GetEmploiDuTempsAsync(int idNiveau);
    }
}