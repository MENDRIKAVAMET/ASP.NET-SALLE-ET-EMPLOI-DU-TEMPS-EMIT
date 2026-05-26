using GestionSalleEmit.DTOs.EmploiDuTemps;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmploisDuTempsController : ControllerBase
    {
        private readonly IEmploiDuTempsService _service;
        private readonly IExportService _exportService;

        public EmploisDuTempsController(IEmploiDuTempsService service, IExportService exportService)
        {
            _service = service;
            _exportService = exportService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Emploi du temps introuvable" });

            return Ok(result);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<ActionResult> Create(EmploiDuTempsCreateDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.IdEDT }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, EmploiDuTempsUpdateDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);

                if (result == null)
                    return NotFound(new { message = "Emploi du temps introuvable" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Emploi du temps introuvable" });

            return Ok(new { message = "Supprimé avec succès" });
        }

        // =========================
        // FILTRES
        // =========================
        [HttpGet("enseignant/{id}")]
        public async Task<ActionResult> GetByEnseignant(int id)
        {
            var result = await _service.GetByEnseignantAsync(id);
            return Ok(result);
        }

        [HttpGet("salle/{id}")]
        public async Task<ActionResult> GetBySalle(int id)
        {
            var result = await _service.GetBySalleAsync(id);
            return Ok(result);
        }

        [HttpGet("niveau/{id}")]
        public async Task<ActionResult> GetByNiveau(int id)
        {
            var result = await _service.GetByNiveauAsync(id);
            return Ok(result);
        }

        // =========================
        // EXPORT EXCEL
        // =========================
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var data = await _service.GetAllAsync();
            var file = _exportService.ExportToExcel(data);

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "emploi_du_temps.xlsx");
        }

        // ========================
        // EXPORT PDF
        // ========================
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            var data = await _service.GetAllAsync();
            var file = _exportService.ExportToPdf(data);

            return File(file, "application/pdf", "emploi_du_temps.pdf");
        }
    }
}