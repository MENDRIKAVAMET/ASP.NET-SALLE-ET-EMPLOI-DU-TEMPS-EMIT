using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    public class ExportController : ControllerBase
    {
        private readonly IEmploiDuTempsService _service;
        private readonly IExportService _export;

        public ExportController(IEmploiDuTempsService service, IExportService export)
        {
            _service = service;
            _export = export;
        }
        [HttpGet("filiere/{id}/excel")]
        public async Task<IActionResult> ExportFiliereExcel(int id)
        {
            var data = await _service.GetByFiliereAsync(id);
            var file = _export.ExportToExcel(data);

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "emploi_filiere.xlsx");
        }
        [HttpGet("filiere/{id}/pdf")]
        public async Task<IActionResult> ExportFilierePdf(int id)
        {
            var data = await _service.GetByFiliereAsync(id);
            var file = _export.ExportToPdf(data);

            return File(file, "application/pdf", "emploi_filiere.pdf");
        }
        [HttpGet("enseignant/{id}/excel")]
        public async Task<IActionResult> ExportEnseignantExcel(int id)
        {
            var data = await _service.GetByEnseignantAsync(id);
            var file = _export.ExportToExcel(data);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "emploi_enseignant.xlsx");
        }


        [HttpGet("enseignant/{id}/pdf")]
        public async Task<IActionResult> ExportEnseignantPdf(int id)
        {
            var data = await _service.GetByEnseignantAsync(id);
            var file = _export.ExportToPdf(data);

            return File(file, "application/pdf", "emploi_enseignant.pdf");
        }

        [HttpGet("semaine/{id}/excel")]
        public async Task<IActionResult> ExportWeekExcel(DateTime start, DateTime end)
        {
            var data = await _service.GetByWeekAsync(start, end);
            var file = _export.ExportToExcel(data);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "emploi_semaine.xlsx");
        }

        [HttpGet("semaine/pdf")]
        public async Task<IActionResult> ExportWeekPdf(DateTime start, DateTime end)
        {
            var data = await _service.GetByWeekAsync(start, end);
            var file = _export.ExportToPdf(data);

            return File(file, "application/pdf", "emploi_semaine.pdf");
        }
    }
}
