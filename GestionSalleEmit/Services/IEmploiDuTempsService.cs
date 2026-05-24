using GestionSalleEmit.DTOs.EmploiDuTemps;

namespace GestionSalleEmit.Services
{
    public interface IEmploiDuTempsService
    {
        // ======================
        // CRUD
        // ======================
        Task<List<EmploiDuTempsResponseDTO>> GetAllAsync();

        Task<EmploiDuTempsResponseDTO?> GetByIdAsync(int id);

        Task<EmploiDuTempsResponseDTO> CreateAsync(EmploiDuTempsCreateDTO dto);

        Task<EmploiDuTempsResponseDTO?> UpdateAsync(int id, EmploiDuTempsUpdateDTO dto);

        Task<bool> DeleteAsync(int id);

        // ======================
        // FILTRES
        // ======================
        Task<List<EmploiDuTempsResponseDTO>> GetByEnseignantAsync(int idEnseignant);

        Task<List<EmploiDuTempsResponseDTO>> GetBySalleAsync(int idSalle);

        Task<List<EmploiDuTempsResponseDTO>> GetByNiveauAsync(int idNiveau);
        Task<List<EmploiDuTempsResponseDTO>> GetByParcoursAsync(int idParcours);
        Task<List<EmploiDuTempsResponseDTO>> GetByMatiereAsync(int idMatiere);
        Task<List<EmploiDuTempsResponseDTO>> GetByFiliereAsync(int idFiliere);
        Task<List<EmploiDuTempsResponseDTO>> GetByWeekAsync(DateTime start, DateTime end);

        // ======================
        // VALIDATION MÉTIER
        // ======================
        Task VerifierConflitsAsync(EmploiDuTempsCreateDTO dto);
    }
}