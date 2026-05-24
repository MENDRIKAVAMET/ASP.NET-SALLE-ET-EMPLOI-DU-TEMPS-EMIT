using GestionSalleEmit.DTOs.EmploiDuTemps;

namespace GestionSalleEmit.Services
{
    public interface IExportService
    {
        byte[] ExportToExcel(List<EmploiDuTempsResponseDTO> data);
        byte[] ExportToPdf(List<EmploiDuTempsResponseDTO> data);
    }
}