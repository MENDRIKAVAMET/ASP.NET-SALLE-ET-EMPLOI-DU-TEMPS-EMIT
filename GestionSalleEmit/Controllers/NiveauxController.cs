using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.Data;
using GestionSalleEmit.Models;
using GestionSalleEmit.DTOs.Niveau;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NiveauxController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NiveauxController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NiveauResponseDTO>>> GetNiveaux()
        {
            var niveaux = await _context.Niveaux.ToListAsync();
            var response = niveaux.Select(n => MapToResponseDTO(n)).ToList();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NiveauResponseDTO>> GetNiveau(int id)
        {
            var niveau = await _context.Niveaux.FindAsync(id);
            if (niveau == null)
            {
                return NotFound(new { message = $"Le niveau avec l'ID {id} n'existe pas" });
            }
            return Ok(MapToResponseDTO(niveau));
        }

        [HttpPost]
        public async Task<ActionResult<NiveauResponseDTO>> PostNiveau(NiveauCreateDTO createDTO)
        {
            // Vérifier que la filière existe
            if (!_context.Filieres.Any(f => f.IdFiliere == createDTO.IdFiliere))
            {
                return BadRequest(new { message = $"La filière avec l'ID {createDTO.IdFiliere} n'existe pas" });
            }

            var niveau = new Niveau
            {
                NomNiveau = createDTO.NomNiveau,
                IdFiliere = createDTO.IdFiliere
            };
            _context.Niveaux.Add(niveau);
            await _context.SaveChangesAsync();
            var responseDTO = MapToResponseDTO(niveau);
            return CreatedAtAction(nameof(GetNiveau), new { id = niveau.IdNiveau }, responseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNiveau(int id, NiveauUpdateDTO updateDTO)
        {
            if (id != updateDTO.IdNiveau)
            {
                return BadRequest(new { message = "L'ID de l'URL ne correspond pas à l'ID du corps de la requête" });
            }
            var niveau = await _context.Niveaux.FindAsync(id);
            if (niveau == null)
            {
                return NotFound(new { message = $"Le niveau avec l'ID {id} n'existe pas" });
            }

            // Vérifier que la filière existe
            if (!_context.Filieres.Any(f => f.IdFiliere == updateDTO.IdFiliere))
            {
                return BadRequest(new { message = $"La filière avec l'ID {updateDTO.IdFiliere} n'existe pas" });
            }

            niveau.NomNiveau = updateDTO.NomNiveau;
            niveau.IdFiliere = updateDTO.IdFiliere;

            _context.Entry(niveau).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NiveauExists(id))
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
        public async Task<IActionResult> DeleteNiveau(int id)
        {
            var niveau = await _context.Niveaux.FindAsync(id);
            if (niveau == null)
            {
                return NotFound(new { message = $"Le niveau avec l'ID {id} n'existe pas" });
            }
            _context.Niveaux.Remove(niveau);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Le niveau avec l'ID {id} a été supprimé avec succès" });
        }

        [NonAction]
        private bool NiveauExists(int id)
        {
            return _context.Niveaux.Any(e => e.IdNiveau == id);
        }

        [NonAction]
        private NiveauResponseDTO MapToResponseDTO(Niveau niveau)
        {
            return new NiveauResponseDTO
            {
                IdNiveau = niveau.IdNiveau,
                NomNiveau = niveau.NomNiveau,
                IdFiliere = niveau.IdFiliere
            };
        }
    }
}
