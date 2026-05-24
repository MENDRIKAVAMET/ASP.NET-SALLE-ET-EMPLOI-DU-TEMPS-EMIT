using GestionSalleEmit.DTOs;
using GestionSalleEmit.DTOs.Salle;
using GestionSalleEmit.Filters;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalleEmit.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SallesController : ControllerBase
{
    private readonly ISalleService _salleService;

    public SallesController(ISalleService salleService)
    {
        _salleService = salleService;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public async Task<ActionResult<List<SalleResponseDTO>>> GetAll()
    {
        var result = await _salleService.GetAllAsync();
        return Ok(result);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<ActionResult<SalleResponseDTO>> GetById(int id)
    {
        var result = await _salleService.GetByIdAsync(id);

        if (result == null)
            return NotFound($"Salle avec id {id} introuvable");

        return Ok(result);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<ActionResult<SalleResponseDTO>> Create([FromBody] SalleCreateDTO dto)
    {
        if (dto == null)
            return BadRequest("Données invalides");
        try
        {
            var created = await _salleService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.IdSalle },
                created
            );
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<ActionResult<SalleUpdateDTO>> Update(int id, [FromBody] SalleUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest("Données invalides");

        var updated = await _salleService.UpdateAsync(id, dto);

        if (updated == null)
            return NotFound($"Salle avec id {id} introuvable");

        return Ok(updated);
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _salleService.DeleteAsync(id);

        if (!deleted)
            return NotFound($"Salle avec id {id} introuvable");

        return NoContent();
    }

    // =========================
    // SEARCH (ULTRA IMPORTANT)
    // =========================
    [HttpPost("search")]
    public async Task<ActionResult<List<SalleResponseDTO>>> Search([FromBody] SalleFilter filter)
    {
        var result = await _salleService.SearchAsync(filter);
        return Ok(result);
    }

    // =========================
    // DISPONIBILITE
    // =========================
    [HttpGet("disponibilite")]
    public async Task<ActionResult<bool>> IsDisponible(
        int idSalle,
        DateTime jour,
        TimeSpan heureDebut,
        TimeSpan heureFin)
    {
        var result = await _salleService.IsSalleDisponibleAsync(
            idSalle,
            jour,
            heureDebut,
            heureFin
        );

        return Ok(result);
    }
}