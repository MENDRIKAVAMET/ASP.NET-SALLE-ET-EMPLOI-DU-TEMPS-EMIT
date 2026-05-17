using GestionSalleEmit.DTOs;
using GestionSalleEmit.DTOs.EmploiDuTemps;

namespace GestionSalleEmit.Services
{
    public interface IEmploiDuTempsService
    {
        Task<bool> VerifierConflitSalle(
            EmploiDuTempsCreateDTO dto);
        Task<bool> VerifierConflitEnseignant(
            EmploiDuTempsCreateDTO dto);
    }
}
