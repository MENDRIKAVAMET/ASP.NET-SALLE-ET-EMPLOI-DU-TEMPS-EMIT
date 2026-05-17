using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.Data;
using GestionSalleEmit.Models;
using GestionSalleEmit.DTOs;
using GestionSalleEmit.DTOs.Enseignant;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnseignantsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public EnseignantsController(AppDbContext context)
        {
            _dbContext = context;
        }

        // GET: api/enseignants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enseignant>>> GetEnseignants()
        {
            return await _dbContext.Enseignants.ToListAsync();
        }

        // GET: api/enseignants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enseignant>> GetEnseignant(int id)
        {
            var enseignant = await _dbContext.Enseignants.FindAsync(id);

            if (enseignant == null)
            {
                return NotFound();
            }

            return enseignant;
        }

        // PUT: api/enseignants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnseignant(int id, Enseignant enseignant)
        {
            if (id != enseignant.IdEnseignant)
            {
                return BadRequest();
            }

            _dbContext.Entry(enseignant).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnseignantExists(id))
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

        // POST: api/enseignants
        [HttpPost]
        public async Task<ActionResult<Enseignant>> PostEnseignant(EnseignantCreateDTO dto)
        {
            var enseignant = new Enseignant
            {
                NomEnseignant = dto.NomEnseignant,
                PrenomEnseignant = dto.PrenomEnseignant,
                EmailEnseignant = dto.EmailEnseignant,
                PhoneEnseignant = dto.PhoneEnseignant,
                GradeEnseignant = dto.GradeEnseignant
            };

            _dbContext.Enseignants.Add(enseignant);
            await _dbContext.SaveChangesAsync();

            var response = new EnseignantResponseDTO
            {
                IdEnseignant = enseignant.IdEnseignant,
                NomEnseignant = enseignant.NomEnseignant,
                PrenomEnseignant = enseignant.PrenomEnseignant,
                EmailEnseignant = enseignant.EmailEnseignant,
                PhoneEnseignant = enseignant.PhoneEnseignant,
                GradeEnseignant = enseignant.GradeEnseignant
            };

            return CreatedAtAction(nameof(GetEnseignant), new { id = enseignant.IdEnseignant }, response);
        }

        // DELETE: api/enseignants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnseignant(int id)
        {
            var enseignant = await _dbContext.Enseignants.FindAsync(id);
            if (enseignant == null)
            {
                return NotFound();
            }

            _dbContext.Enseignants.Remove(enseignant);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        private bool EnseignantExists(int id)
        {
            return _dbContext.Enseignants.Any(e => e.IdEnseignant == id);
        }
    }
}
