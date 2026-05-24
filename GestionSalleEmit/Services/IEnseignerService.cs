using GestionSalleEmit.DTOs.Enseigner;

namespace GestionSalleEmit.Services
{
    public interface IEnseignerService
    {
        // =========================
        // GET ALL
        // =========================
        Task<List<EnseignerResponseDTO>> GetAllAsync();

        // =========================
        // GET PAR ENSEIGNANT
        // =========================
        Task<List<EnseignerResponseDTO>>
            GetByEnseignantAsync(int idEnseignant);

        // =========================
        // GET PAR MATIERE
        // =========================
        Task<List<EnseignerResponseDTO>>
            GetByMatiereAsync(int idMatiere);

        // =========================
        // CREATE RELATION
        // =========================
        Task<EnseignerResponseDTO>
            CreateAsync(EnseignerCreateDTO dto);

        // =========================
        // DELETE RELATION
        // =========================
        Task<bool> DeleteAsync(
            int idEnseignant,
            int idMatiere);

        // =========================
        // EXISTS
        // =========================
        Task<bool> ExistsAsync(
            int idEnseignant,
            int idMatiere);
    }
}