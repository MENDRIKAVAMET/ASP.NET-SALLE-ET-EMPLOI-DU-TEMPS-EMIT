using GestionSalleEmit.DTOs.Matiere;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MatieresController : ControllerBase
    {
        private readonly IMatiereService _matiereService;

        public MatieresController(IMatiereService matiereService)
        {
            _matiereService = matiereService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<ActionResult<List<MatiereResponseDTO>>> GetAll()
        {
            var result = await _matiereService.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<MatiereResponseDTO>> GetById(int id)
        {
            var result = await _matiereService.GetByIdAsync(id);

            if (result == null)
                return NotFound($"Matière avec id {id} introuvable");

            return Ok(result);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<ActionResult<MatiereResponseDTO>> Create(
            [FromBody] MatiereCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var created = await _matiereService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.IdMatiere },
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
        public async Task<ActionResult<MatiereResponseDTO>> Update(
            int id,
            [FromBody] MatiereCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var updated = await _matiereService.UpdateAsync(id, dto);

                if (updated == null)
                    return NotFound($"Matière avec id {id} introuvable");

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
                var deleted = await _matiereService.DeleteAsync(id);

                if (!deleted)
                    return NotFound($"Matière avec id {id} introuvable");

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
        public async Task<ActionResult<List<MatiereResponseDTO>>> Search(
            [FromQuery] string nomMatiere = "",
            [FromQuery] string semestre = "")
        {
            var result = await _matiereService.SearchAsync(nomMatiere, semestre);
            return Ok(result);
        }

        // =========================
        // GET BY NIVEAU
        // =========================
        [HttpGet("niveau/{idNiveau}")]
        public async Task<ActionResult<List<MatiereResponseDTO>>> GetByNiveau(int idNiveau)
        {
            var result = await _matiereService.GetByNiveauAsync(idNiveau);
            return Ok(result);
        }

        // =========================
        // GET BY ENSEIGNANT
        // =========================
        [HttpGet("enseignant/{idEnseignant}")]
        public async Task<ActionResult<List<MatiereResponseDTO>>> GetByEnseignant(int idEnseignant)
        {
            var result = await _matiereService.GetByEnseignantAsync(idEnseignant);
            return Ok(result);
        }

        // =========================
        // GET BY SEMESTRE
        // =========================
        [HttpGet("semestre/{semestre}")]
        public async Task<ActionResult<List<MatiereResponseDTO>>> GetBySemestre(string semestre)
        {
            var result = await _matiereService.GetBySemestreAsync(semestre);
            return Ok(result);
        }

        // =========================
        // EXISTS
        // =========================
        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var result = await _matiereService.ExistsAsync(id);
            return Ok(result);
        }

        // =========================
        // NOM EXISTE
        // =========================
        [HttpGet("exists-nom")]
        public async Task<ActionResult<bool>> NomExiste([FromQuery] string nomMatiere)
        {
            var result = await _matiereService.NomExisteAsync(nomMatiere);
            return Ok(result);
        }
    }
}