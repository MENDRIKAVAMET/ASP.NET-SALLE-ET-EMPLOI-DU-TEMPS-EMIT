using GestionSalleEmit.DTOs.Parcours;

namespace GestionSalleEmit.Services
{
    public interface IParcoursService
    {
        // =========================
        // GET ALL
        // =========================
        Task<List<ParcoursResponseDTO>> GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        Task<ParcoursResponseDTO?> GetByIdAsync(int id);

        // =========================
        // CREATE
        // =========================
        Task<ParcoursResponseDTO> CreateAsync(ParcoursCreateDTO dto);

        // =========================
        // UPDATE
        // =========================
        Task<ParcoursResponseDTO?> UpdateAsync(int id, ParcoursUpdateDTO dto);

        // =========================
        // DELETE
        // =========================
        Task<bool> DeleteAsync(int id);

        // =========================
        // SEARCH
        // =========================
        Task<List<ParcoursResponseDTO>> SearchAsync(string nomParcours);

        // =========================
        // EXISTS
        // =========================
        Task<bool> ExistsAsync(int id);

        // =========================
        // NOM EXISTE
        // =========================
        Task<bool> NomExisteAsync(string nomParcours);

        // =========================
        // GET PAR FILIERE
        // =========================
        Task<List<ParcoursResponseDTO>> GetByFiliereAsync(int idFiliere);
    }
}