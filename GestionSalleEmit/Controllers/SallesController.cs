using GestionSalleEmit.Data;
using Microsoft.AspNetCore.Mvc;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.DTOs.Salle;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SallesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SallesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalleResponseDTO>>> GetSalles()
        {
            var salles = await _context.Salles.ToListAsync();
            var response = salles.Select(s => MapToResponseDTO(s)).ToList();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalleResponseDTO>> GetSalle(int id)
        {
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
            {
                return NotFound(new { message = $"La salle avec l'ID {id} n'existe pas" });
            }
            return Ok(MapToResponseDTO(salle));
        }

        [HttpPost]
        public async Task<ActionResult<SalleResponseDTO>> PostSalle(SalleCreateDTO createDTO)
        {
            var salle = new Salle
            {
                NomSalle = createDTO.NomSalle,
                Capacite = createDTO.Capacite,
                TypeSalle = createDTO.TypeSalle
            };
            _context.Salles.Add(salle);
            await _context.SaveChangesAsync();
            var responseDTO = MapToResponseDTO(salle);
            return CreatedAtAction(nameof(GetSalle), new { id = salle.IdSalle }, responseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalle(int id, SalleUpdateDTO updateDTO)
        {
            if (id != updateDTO.IdSalle)
            {
                return BadRequest(new { message = "L'ID de l'URL ne correspond pas à l'ID du corps de la requête" });
            }
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
            {
                return NotFound(new { message = $"La salle avec l'ID {id} n'existe pas" });
            }

            salle.NomSalle = updateDTO.NomSalle;
            salle.Capacite = updateDTO.Capacite;
            salle.TypeSalle = updateDTO.TypeSalle;

            _context.Entry(salle).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalle(int id)
        {
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
            {
                return NotFound(new { message = $"La salle avec l'ID {id} n'existe pas" });
            }
            _context.Salles.Remove(salle);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"La salle avec l'ID {id} a été supprimée avec succès" });
        }

        [NonAction]
        private bool SalleExists(int id)
        {
            return _context.Salles.Any(e => e.IdSalle == id);
        }

        [NonAction]
        private SalleResponseDTO MapToResponseDTO(Salle salle)
        {
            return new SalleResponseDTO
            {
                IdSalle = salle.IdSalle,
                NomSalle = salle.NomSalle,
                Capacite = salle.Capacite,
                TypeSalle = salle.TypeSalle
            };
        }
    }
}