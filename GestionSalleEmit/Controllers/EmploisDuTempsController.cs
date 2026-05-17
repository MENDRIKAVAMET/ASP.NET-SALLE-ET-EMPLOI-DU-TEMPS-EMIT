using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.Data;
using GestionSalleEmit.Models;
using GestionSalleEmit.DTOs.EmploiDuTemps;
using GestionSalleEmit.Services;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploisDuTempsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmploiDuTempsService _service;

        public EmploisDuTempsController(AppDbContext context, IEmploiDuTempsService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmploiDuTempsResponseDTO>>> GetEmploisDuTemps()
        {
            var emplois = await _context.EmploisDuTemps.ToListAsync();
            var response = emplois.Select(e => MapToResponseDTO(e)).ToList();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmploiDuTempsResponseDTO>> GetEmploiDuTemps(int id)
        {
            var emploiDuTemps = await _context.EmploisDuTemps.FindAsync(id);
            if (emploiDuTemps == null)
            {
                return NotFound(new { message = $"L'emploi du temps avec l'ID {id} n'existe pas" });
            }
            return Ok(MapToResponseDTO(emploiDuTemps));
        }

        [HttpPost]
        public async Task<ActionResult<EmploiDuTempsResponseDTO>> PostEmploiDuTemps(EmploiDuTempsCreateDTO createDTO)
        {
            if(await _service.VerifierConflitSalle(createDTO))
            {
                return BadRequest("La salle est déjà occupée");
            }
            if(await _service.VerifierConflitEnseignant(createDTO))
            {
                return BadRequest("L'enseignant est déjà occupé");
            }
            // Vérifier que les références existent
            if (!_context.Salles.Any(s => s.IdSalle == createDTO.IdSalle))
                return BadRequest(new { message = $"La salle avec l'ID {createDTO.IdSalle} n'existe pas" });

            if (!_context.Enseignants.Any(e => e.IdEnseignant == createDTO.IdEnseignant))
                return BadRequest(new { message = $"L'enseignant avec l'ID {createDTO.IdEnseignant} n'existe pas" });

            if (!_context.Matieres.Any(m => m.IdMatiere == createDTO.IdMatiere))
                return BadRequest(new { message = $"La matière avec l'ID {createDTO.IdMatiere} n'existe pas" });

            if (!_context.Niveaux.Any(n => n.IdNiveau == createDTO.IdNiveau))
                return BadRequest(new { message = $"Le niveau avec l'ID {createDTO.IdNiveau} n'existe pas" });

            var emploiDuTemps = new EmploiDuTemps
            {
                Jour = createDTO.Jour,
                HeureDebut = createDTO.HeureDebut,
                HeureFin = createDTO.HeureFin,
                Semestre = createDTO.Semestre,
                IdSalle = createDTO.IdSalle,
                IdEnseignant = createDTO.IdEnseignant,
                IdMatiere = createDTO.IdMatiere,
                IdNiveau = createDTO.IdNiveau
            };

            _context.EmploisDuTemps.Add(emploiDuTemps);
            await _context.SaveChangesAsync();
            var responseDTO = MapToResponseDTO(emploiDuTemps);
            return CreatedAtAction(nameof(GetEmploiDuTemps), new { id = emploiDuTemps.IdEDT }, responseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmploiDuTemps(int id, EmploiDuTempsUpdateDTO updateDTO)
        {
            if (id != updateDTO.IdEDT)
            {
                return BadRequest(new { message = "L'ID de l'URL ne correspond pas à l'ID du corps de la requête" });
            }

            var emploiDuTemps = await _context.EmploisDuTemps.FindAsync(id);
            if (emploiDuTemps == null)
            {
                return NotFound(new { message = $"L'emploi du temps avec l'ID {id} n'existe pas" });
            }

            // Vérifier que les références existent
            if (!_context.Salles.Any(s => s.IdSalle == updateDTO.IdSalle))
                return BadRequest(new { message = $"La salle avec l'ID {updateDTO.IdSalle} n'existe pas" });

            if (!_context.Enseignants.Any(e => e.IdEnseignant == updateDTO.IdEnseignant))
                return BadRequest(new { message = $"L'enseignant avec l'ID {updateDTO.IdEnseignant} n'existe pas" });

            if (!_context.Matieres.Any(m => m.IdMatiere == updateDTO.IdMatiere))
                return BadRequest(new { message = $"La matière avec l'ID {updateDTO.IdMatiere} n'existe pas" });

            if (!_context.Niveaux.Any(n => n.IdNiveau == updateDTO.IdNiveau))
                return BadRequest(new { message = $"Le niveau avec l'ID {updateDTO.IdNiveau} n'existe pas" });

            emploiDuTemps.Jour = updateDTO.Jour;
            emploiDuTemps.HeureDebut = updateDTO.HeureDebut;
            emploiDuTemps.HeureFin = updateDTO.HeureFin;
            emploiDuTemps.Semestre = updateDTO.Semestre;
            emploiDuTemps.IdSalle = updateDTO.IdSalle;
            emploiDuTemps.IdEnseignant = updateDTO.IdEnseignant;
            emploiDuTemps.IdMatiere = updateDTO.IdMatiere;
            emploiDuTemps.IdNiveau = updateDTO.IdNiveau;

            _context.Entry(emploiDuTemps).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmploiDuTempsExists(id))
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
        public async Task<IActionResult> DeleteEmploiDuTemps(int id)
        {
            var emploiDuTemps = await _context.EmploisDuTemps.FindAsync(id);
            if (emploiDuTemps == null)
            {
                return NotFound(new { message = $"L'emploi du temps avec l'ID {id} n'existe pas" });
            }
            _context.EmploisDuTemps.Remove(emploiDuTemps);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"L'emploi du temps avec l'ID {id} a été supprimé avec succès" });
        }

        [NonAction]
        private bool EmploiDuTempsExists(int id)
        {
            return _context.EmploisDuTemps.Any(e => e.IdEDT == id);
        }

        [NonAction]
        private EmploiDuTempsResponseDTO MapToResponseDTO(EmploiDuTemps emploi)
        {
            return new EmploiDuTempsResponseDTO
            {
                IdEDT = emploi.IdEDT,
                Jour = emploi.Jour,
                HeureDebut = emploi.HeureDebut,
                HeureFin = emploi.HeureFin,
                Semestre = emploi.Semestre,
                IdSalle = emploi.IdSalle,
                IdEnseignant = emploi.IdEnseignant,
                IdMatiere = emploi.IdMatiere,
                IdNiveau = emploi.IdNiveau
            };
        }
    }
}