using GestionSalleEmit.DTOs.Niveau;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NiveauxController : ControllerBase
    {
        private readonly INiveauService _niveauService;

        public NiveauxController(INiveauService niveauService)
        {
            _niveauService = niveauService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<ActionResult<List<NiveauResponseDTO>>> GetAll()
        {
            var result = await _niveauService.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<NiveauResponseDTO>> GetById(int id)
        {
            var result = await _niveauService.GetByIdAsync(id);

            if (result == null)
                return NotFound($"Niveau avec id {id} introuvable");

            return Ok(result);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<ActionResult<NiveauResponseDTO>> Create(
            [FromBody] NiveauCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var created = await _niveauService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.IdNiveau },
                    created
                );
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        public async Task<ActionResult<NiveauResponseDTO>> Update(
            int id,
            [FromBody] NiveauCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var updated = await _niveauService.UpdateAsync(id, dto);

                if (updated == null)
                    return NotFound($"Niveau avec id {id} introuvable");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
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
                var deleted = await _niveauService.DeleteAsync(id);

                if (!deleted)
                    return NotFound($"Niveau avec id {id} introuvable");

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }

        // =========================
        // SEARCH
        // =========================
        [HttpGet("search")]
        public async Task<ActionResult<List<NiveauResponseDTO>>> Search(
            [FromQuery] string nomNiveau = "")
        {
            var result = await _niveauService.SearchAsync(nomNiveau);
            return Ok(result);
        }

        // =========================
        // EXISTS
        // =========================
        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var result = await _niveauService.ExistsAsync(id);
            return Ok(result);
        }

        // =========================
        // NOM EXISTE
        // =========================
        [HttpGet("exists-nom")]
        public async Task<ActionResult<bool>> NomExiste(
            [FromQuery] string nomNiveau)
        {
            var result = await _niveauService.NomExisteAsync(nomNiveau);
            return Ok(result);
        }

        // =========================
        // MATIERES D'UN NIVEAU
        // =========================
        [HttpGet("{id}/matieres")]
        public async Task<ActionResult> GetMatieres(int id)
        {
            try
            {
                var result = await _niveauService.GetMatieresAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }

        // =========================
        // EMPLOI DU TEMPS D'UN NIVEAU
        // =========================
        [HttpGet("{id}/emploi-du-temps")]
        public async Task<ActionResult> GetEmploiDuTemps(int id)
        {
            try
            {
                var result = await _niveauService.GetEmploiDuTempsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: 400);
            }
        }
    }
}