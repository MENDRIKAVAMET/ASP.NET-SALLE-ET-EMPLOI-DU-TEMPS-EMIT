using GestionSalleEmit.DTOs.Filiere;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilieresController : ControllerBase
    {
        private readonly IFiliereService _filiereService;

        public FilieresController(IFiliereService filiereService)
        {
            _filiereService = filiereService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<ActionResult<List<FiliereResponseDTO>>> GetAll()
        {
            var result = await _filiereService.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<FiliereResponseDTO>> GetById(int id)
        {
            var result = await _filiereService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new
                {
                    message = $"Filière avec id {id} introuvable"
                });

            return Ok(result);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<ActionResult<FiliereResponseDTO>> Create(
            [FromBody] FiliereCreateDTO dto)
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
                var created = await _filiereService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.IdFiliere },
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
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        public async Task<ActionResult<FiliereResponseDTO>> Update(
            int id,
            [FromBody] FiliereUpdateDTO dto)
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
                var updated = await _filiereService.UpdateAsync(id, dto);

                if (updated == null)
                {
                    return NotFound(new
                    {
                        message = $"Filière avec id {id} introuvable"
                    });
                }

                return Ok(updated);
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
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _filiereService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        message = $"Filière avec id {id} introuvable"
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
        // SEARCH
        // =========================
        [HttpGet("search")]
        public async Task<ActionResult<List<FiliereResponseDTO>>> Search(
            [FromQuery] string nomFiliere)
        {
            var result = await _filiereService.SearchAsync(nomFiliere);
            return Ok(result);
        }

        // =========================
        // EXISTS
        // =========================
        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var result = await _filiereService.ExistsAsync(id);
            return Ok(result);
        }

        // =========================
        // NOM EXISTE
        // =========================
        [HttpGet("exists-nom")]
        public async Task<ActionResult<bool>> NomExiste(
            [FromQuery] string nomFiliere)
        {
            var result = await _filiereService.NomExisteAsync(nomFiliere);
            return Ok(result);
        }
    }
}