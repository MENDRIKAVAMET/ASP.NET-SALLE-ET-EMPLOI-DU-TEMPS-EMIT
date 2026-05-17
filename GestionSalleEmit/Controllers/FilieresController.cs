using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.Data;
using GestionSalleEmit.Models;
using GestionSalleEmit.DTOs.Filiere;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilieresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilieresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FiliereResponseDTO>>> GetFilieres()
        {
            var filieres = await _context.Filieres.ToListAsync();
            var response = filieres.Select(f => MapToResponseDTO(f)).ToList();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FiliereResponseDTO>> GetFiliere(int id)
        {
            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere == null)
            {
                return NotFound(new { message = $"La filière avec l'ID {id} n'existe pas" });
            }
            return Ok(MapToResponseDTO(filiere));
        }

        [HttpPost]
        public async Task<ActionResult<FiliereResponseDTO>> PostFiliere(FiliereCreateDTO createDTO)
        {
            var filiere = new Filiere
            {
                NomFiliere = createDTO.NomFiliere,
            };
            _context.Filieres.Add(filiere);
            await _context.SaveChangesAsync();
            var responseDTO = MapToResponseDTO(filiere);
            return CreatedAtAction(nameof(GetFiliere), new { id = filiere.IdFiliere }, responseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFiliere(int id, FiliereUpdateDTO updateDTO)
        {
            if (id != updateDTO.IdFiliere)
            {
                return BadRequest(new { message = "L'ID de l'URL ne correspond pas à l'ID du corps de la requête" });
            }
            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere == null)
            {
                return NotFound(new { message = $"La filière avec l'ID {id} n'existe pas" });
            }

            filiere.NomFiliere = updateDTO.NomFiliere;

            _context.Entry(filiere).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FiliereExists(id))
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
        public async Task<IActionResult> DeleteFiliere(int id)
        {
            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere == null)
            {
                return NotFound(new { message = $"La filière avec l'ID {id} n'existe pas" });
            }
            _context.Filieres.Remove(filiere);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"La filière avec l'ID {id} a été supprimée avec succès" });
        }

        [NonAction]
        private bool FiliereExists(int id)
        {
            return _context.Filieres.Any(e => e.IdFiliere == id);
        }

        [NonAction]
        private FiliereResponseDTO MapToResponseDTO(Filiere filiere)
        {
            return new FiliereResponseDTO
            {
                IdFiliere = filiere.IdFiliere,
                NomFiliere = filiere.NomFiliere
            };
        }
    }
}
