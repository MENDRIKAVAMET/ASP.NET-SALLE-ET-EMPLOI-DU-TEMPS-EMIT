using GestionSalleEmit.DTOs.Parcours;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ParcoursController : ControllerBase
    {
        private readonly IParcoursService _parcoursService;

        public ParcoursController(IParcoursService parcoursService)
        {
            _parcoursService = parcoursService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<ActionResult<List<ParcoursResponseDTO>>> GetAll()
        {
            var result = await _parcoursService.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<ParcoursResponseDTO>> GetById(int id)
        {
            var result = await _parcoursService.GetByIdAsync(id);

            if (result == null)
                return NotFound($"Parcours avec id {id} introuvable");

            return Ok(result);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<ActionResult<ParcoursResponseDTO>> Create(
            [FromBody] ParcoursCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var created = await _parcoursService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.IdParcours },
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
        public async Task<ActionResult<ParcoursResponseDTO>> Update(
            int id,
            [FromBody] ParcoursUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest("Données invalides");

            try
            {
                var updated = await _parcoursService.UpdateAsync(id, dto);

                if (updated == null)
                    return NotFound($"Parcours avec id {id} introuvable");

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
                var deleted = await _parcoursService.DeleteAsync(id);

                if (!deleted)
                    return NotFound($"Parcours avec id {id} introuvable");

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
        public async Task<ActionResult<List<ParcoursResponseDTO>>> Search(
            [FromQuery] string nomParcours = "")
        {
            var result = await _parcoursService.SearchAsync(nomParcours);
            return Ok(result);
        }

        // =========================
        // EXISTS
        // =========================
        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var result = await _parcoursService.ExistsAsync(id);
            return Ok(result);
        }

        // =========================
        // NOM EXISTE
        // =========================
        [HttpGet("exists-nom")]
        public async Task<ActionResult<bool>> NomExiste(
            [FromQuery] string nomParcours)
        {
            var result = await _parcoursService.NomExisteAsync(nomParcours);
            return Ok(result);
        }

        // =========================
        // GET PAR FILIERE
        // =========================
        [HttpGet("filiere/{idFiliere}")]
        public async Task<ActionResult<List<ParcoursResponseDTO>>> GetByFiliere(int idFiliere)
        {
            try
            {
                var result = await _parcoursService.GetByFiliereAsync(idFiliere);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}