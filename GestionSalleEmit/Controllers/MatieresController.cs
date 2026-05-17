using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.Data;
using GestionSalleEmit.Models;
using GestionSalleEmit.DTOs.Matiere;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatieresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatieresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatiereResponseDTO>>> GetMatieres()
        {
            var matieres = await _context.Matieres.ToListAsync();
            var response = matieres.Select(m => MapToResponseDTO(m)).ToList();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatiereResponseDTO>> GetMatiere(int id)
        {
            var matiere = await _context.Matieres.FindAsync(id);
            if (matiere == null)
            {
                return NotFound(new { message = $"La matière avec l'ID {id} n'existe pas" });
            }
            return Ok(MapToResponseDTO(matiere));
        }

        [HttpPost]
        public async Task<ActionResult<MatiereResponseDTO>> PostMatiere(MatiereCreateDTO createDTO)
        {
            // Vérifier que le code n'existe pas déjà
            if (_context.Matieres.Any(m => m.CodeMatiere == createDTO.CodeMatiere))
            {
                return BadRequest(new { message = "Ce code de matière existe déjà" });
            }

            var matiere = new Matiere
            {
                CodeMatiere = createDTO.CodeMatiere,
                NomMatiere = createDTO.NomMatiere,
                VolumeHoraire = createDTO.VolumeHoraire
            };

            _context.Matieres.Add(matiere);
            await _context.SaveChangesAsync();
            var responseDTO = MapToResponseDTO(matiere);
            return CreatedAtAction(nameof(GetMatiere), new { id = matiere.IdMatiere }, responseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatiere(int id, MatiereUpdateDTO updateDTO)
        {
            if (id != updateDTO.IdMatiere)
            {
                return BadRequest(new { message = "L'ID de l'URL ne correspond pas à l'ID du corps de la requête" });
            }
            var matiere = await _context.Matieres.FindAsync(id);
            if (matiere == null)
            {
                return NotFound(new { message = $"La matière avec l'ID {id} n'existe pas" });
            }

            // Vérifier que le code n'existe pas pour une autre matière
            if (_context.Matieres.Any(m => m.CodeMatiere == updateDTO.CodeMatiere && m.IdMatiere != id))
            {
                return BadRequest(new { message = "Ce code de matière existe déjà pour une autre matière" });
            }

            matiere.CodeMatiere = updateDTO.CodeMatiere;
            matiere.NomMatiere = updateDTO.NomMatiere;
            matiere.VolumeHoraire = updateDTO.VolumeHoraire;

            _context.Entry(matiere).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatiereExists(id))
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
        public async Task<IActionResult> DeleteMatiere(int id)
        {
            var matiere = await _context.Matieres.FindAsync(id);
            if (matiere == null)
            {
                return NotFound(new { message = $"La matière avec l'ID {id} n'existe pas" });
            }
            _context.Matieres.Remove(matiere);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"La matière avec l'ID {id} a été supprimée avec succès" });
        }

        [NonAction]
        private bool MatiereExists(int id)
        {
            return _context.Matieres.Any(e => e.IdMatiere == id);
        }

        [NonAction]
        private MatiereResponseDTO MapToResponseDTO(Matiere matiere)
        {
            return new MatiereResponseDTO
            {
                IdMatiere = matiere.IdMatiere,
                CodeMatiere = matiere.CodeMatiere,
                NomMatiere = matiere.NomMatiere,
                VolumeHoraire = matiere.VolumeHoraire
            };
        }
    }
}