using GestionSalleEmit.DTOs.Enseignant;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnseignantsController : ControllerBase
    {
        private readonly IEnseignantService _enseignantService;

        public EnseignantsController(IEnseignantService enseignantService)
        {
            _enseignantService = enseignantService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<ActionResult<List<EnseignantResponseDTO>>> GetAll()
        {
            var result = await _enseignantService.GetAllAsync();

            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<EnseignantResponseDTO>> GetById(int id)
        {
            var result = await _enseignantService.GetByIdAsync(id);

            if (result == null)
                return NotFound($"Enseignant avec id {id} introuvable");

            return Ok(result);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<ActionResult<EnseignantResponseDTO>> Create(
            [FromBody] EnseignantCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var created = await _enseignantService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.IdEnseignant },
                    created
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        public async Task<ActionResult<EnseignantResponseDTO>> Update(
            int id,
            [FromBody] EnseignantCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var updated = await _enseignantService.UpdateAsync(id, dto);

                if (updated == null)
                    return NotFound($"Enseignant avec id {id} introuvable");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _enseignantService.DeleteAsync(id);

                if (!deleted)
                    return NotFound($"Enseignant avec id {id} introuvable");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // SEARCH
        // =========================
        [HttpGet("search")]
        public async Task<ActionResult<List<EnseignantResponseDTO>>> Search(
            [FromQuery] string nom = "",
            [FromQuery] string prenom = "")
        {
            var result = await _enseignantService.SearchAsync(nom, prenom);

            return Ok(result);
        }

        // =========================
        // DISPONIBILITE
        // =========================
        [HttpGet("disponibilite")]
        public async Task<ActionResult<List<bool>>> IsDisponible(
            int idEnseignant,
            DateTime jour,
            TimeSpan heureDebut,
            TimeSpan heureFin)
        {
            try
            {
                var result = await _enseignantService.IsDisponibleAsync(
                    idEnseignant,
                    jour,
                    heureDebut,
                    heureFin);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // GET BY MATIERE
        // =========================
        [HttpGet("matiere/{idMatiere}")]
        public async Task<ActionResult<List<EnseignantResponseDTO>>> GetByMatiere(
            int idMatiere)
        {
            var result = await _enseignantService.GetByMatiereAsync(idMatiere);

            return Ok(result);
        }

        // =========================
        // PLANNING
        // =========================
        [HttpGet("{idEnseignant}/planning")]
        public async Task<ActionResult<List<EnseignantResponseDTO>>> GetPlanning(
            int idEnseignant)
        {
            try
            {
                var result = await _enseignantService.GetPlanningAsync(idEnseignant);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}