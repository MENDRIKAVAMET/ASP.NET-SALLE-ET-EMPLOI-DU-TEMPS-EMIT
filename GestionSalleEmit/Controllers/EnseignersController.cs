using GestionSalleEmit.DTOs.Enseigner;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnseignersController : ControllerBase
    {
        private readonly IEnseignerService _enseignerService;

        public EnseignersController(
            IEnseignerService enseignerService)
        {
            _enseignerService = enseignerService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<ActionResult<List<EnseignerResponseDTO>>>
            GetAll()
        {
            var result = await _enseignerService
                .GetAllAsync();

            return Ok(result);
        }

        // =========================
        // GET PAR ENSEIGNANT
        // =========================
        [HttpGet("enseignant/{idEnseignant}")]
        public async Task<ActionResult<List<EnseignerResponseDTO>>>
            GetByEnseignant(int idEnseignant)
        {
            try
            {
                var result = await _enseignerService
                    .GetByEnseignantAsync(idEnseignant);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =========================
        // GET PAR MATIERE
        // =========================
        [HttpGet("matiere/{idMatiere}")]
        public async Task<ActionResult<List<EnseignerResponseDTO>>>
            GetByMatiere(int idMatiere)
        {
            try
            {
                var result = await _enseignerService
                    .GetByMatiereAsync(idMatiere);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =========================
        // CREATE RELATION
        // =========================
        [HttpPost]
        public async Task<ActionResult<EnseignerResponseDTO>>
            Create([FromBody] EnseignerCreateDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    message = "Données invalides"
                });
            }

            try
            {
                var created = await _enseignerService
                    .CreateAsync(dto);

                return CreatedAtAction(
                    nameof(Exists),
                    new
                    {
                        idEnseignant = created.IdEnseignant,
                        idMatiere = created.IdMatiere
                    },
                    created
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =========================
        // DELETE RELATION
        // =========================
        [HttpDelete("{idEnseignant}/{idMatiere}")]
        public async Task<ActionResult>
            Delete(
                int idEnseignant,
                int idMatiere)
        {
            try
            {
                var deleted = await _enseignerService
                    .DeleteAsync(
                        idEnseignant,
                        idMatiere);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        message = "Relation introuvable"
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =========================
        // EXISTS
        // =========================
        [HttpGet("exists")]
        public async Task<ActionResult<bool>>
            Exists(
                [FromQuery] int idEnseignant,
                [FromQuery] int idMatiere)
        {
            var result = await _enseignerService
                .ExistsAsync(
                    idEnseignant,
                    idMatiere);

            return Ok(result);
        }
    }
}