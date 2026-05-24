using GestionSalleEmit.DTOs.Matiere;

namespace GestionSalleEmit.Services
{
    public interface IMatiereService
    {
        // =========================
        // CRUD
        // =========================
        Task<List<MatiereResponseDTO>> GetAllAsync();

        Task<MatiereResponseDTO?> GetByIdAsync(int id);

        Task<MatiereResponseDTO> CreateAsync(MatiereCreateDTO dto);

        Task<MatiereResponseDTO?> UpdateAsync(
            int id,
            MatiereCreateDTO dto);

        Task<bool> DeleteAsync(int id);

        // =========================
        // SEARCH
        // =========================
        Task<List<MatiereResponseDTO>> SearchAsync(
            string nomMatiere = "",
            string semestre = "");

        // =========================
        // FILTRES METIER
        // =========================
        Task<List<MatiereResponseDTO>> GetByNiveauAsync(int idNiveau);

        Task<List<MatiereResponseDTO>> GetByEnseignantAsync(int idEnseignant);

        Task<List<MatiereResponseDTO>> GetBySemestreAsync(string semestre);

        // =========================
        // VALIDATIONS
        // =========================
        Task<bool> ExistsAsync(int id);

        Task<bool> NomExisteAsync(string nomMatiere);
    }
}