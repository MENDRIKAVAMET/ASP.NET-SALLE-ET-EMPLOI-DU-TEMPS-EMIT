using GestionSalleEmit.DTOs.Salle;
using GestionSalleEmit.Filters;

namespace GestionSalleEmit.Services;

public interface ISalleService
{
    Task<List<SalleResponseDTO>> GetAllAsync();
    Task<SalleResponseDTO?> GetByIdAsync(int id);
    Task<SalleResponseDTO> CreateAsync(SalleCreateDTO dto);
    Task<SalleUpdateDTO?> UpdateAsync(int id, SalleUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<List<SalleResponseDTO>> GetByTypeAsync(string type);
    Task<List<SalleResponseDTO>> GetByCapaciteAsync(int capacite);
    Task<List<SalleResponseDTO>> GetByTypeAndCapaciteAsync(string type, int capacite);
    Task<bool> IsSalleDisponibleAsync(int idSalle, DateTime jour, TimeSpan heureDebut, TimeSpan heureFin);
    Task<List<SalleResponseDTO>> SearchAsync(SalleFilter filter);

}
