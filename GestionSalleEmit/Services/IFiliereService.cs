using GestionSalleEmit.DTOs.Filiere;

namespace GestionSalleEmit.Services
{
    public interface IFiliereService
    {
        // =========================
        // GET ALL
        // =========================
        Task<List<FiliereResponseDTO>> GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        Task<FiliereResponseDTO?> GetByIdAsync(int id);

        // =========================
        // CREATE
        // =========================
        Task<FiliereResponseDTO> CreateAsync(FiliereCreateDTO dto);

        // =========================
        // UPDATE
        // =========================
        Task<FiliereResponseDTO?> UpdateAsync(int id, FiliereUpdateDTO dto);

        // =========================
        // DELETE
        // =========================
        Task<bool> DeleteAsync(int id);

        // =========================
        // SEARCH
        // =========================
        Task<List<FiliereResponseDTO>> SearchAsync(string nomFiliere);

        // =========================
        // EXISTS
        // =========================
        Task<bool> ExistsAsync(int id);

        // =========================
        // NOM EXISTE
        // =========================
        Task<bool> NomExisteAsync(string nomFiliere);
    }
}