using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Salle;
using GestionSalleEmit.Filters;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services;

public class SalleService : ISalleService
{
    private readonly AppDbContext _context;

    public SalleService(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    public async Task<List<SalleResponseDTO>> GetAllAsync()
    {
        return await _context.Salles
            .Select(s => MapToResponseDTO(s))
            .ToListAsync();
    }

    // =========================
    // GET BY ID
    // =========================
    public async Task<SalleResponseDTO?> GetByIdAsync(int id)
    {
        var salle = await _context.Salles.FindAsync(id);
        return salle == null ? null : MapToResponseDTO(salle);
    }

    // =========================
    // CREATE
    // =========================
    public async Task<SalleResponseDTO> CreateAsync(SalleCreateDTO dto)
    {
        var salleExiste = await _context.Salles.AnyAsync(s => s.NomSalle == dto.NomSalle);
        if (salleExiste)
        {
            throw new InvalidOperationException($"Une salle avec le nom '{dto.NomSalle}' existe déjà.");
        }
        var salle = new Salle
        {
            NomSalle = dto.NomSalle,
            TypeSalle = dto.TypeSalle,
            Capacite = dto.Capacite
        };

        _context.Salles.Add(salle);
        await _context.SaveChangesAsync();

        return MapToResponseDTO(salle);
    }

    // =========================
    // UPDATE
    // =========================
    public async Task<SalleUpdateDTO?> UpdateAsync(int id, SalleUpdateDTO dto)
    {
        var salle = await _context.Salles.FindAsync(id);
        if (salle == null) return null;

        salle.NomSalle = dto.NomSalle;
        salle.TypeSalle = dto.TypeSalle;
        salle.Capacite = dto.Capacite;

        await _context.SaveChangesAsync();

        return MapToUpdateDTO(salle);
    }

    // =========================
    // DELETE
    // =========================
    public async Task<bool> DeleteAsync(int id)
    {
        var salle = await _context.Salles.FindAsync(id);
        if (salle == null) return false;

        _context.Salles.Remove(salle);
        await _context.SaveChangesAsync();

        return true;
    }

    // =========================
    // FILTER BY TYPE
    // =========================
    public async Task<List<SalleResponseDTO>> GetByTypeAsync(string type)
    {
        return await _context.Salles
            .Where(s => s.TypeSalle == type)
            .Select(s => MapToResponseDTO(s))
            .ToListAsync();
    }

    // =========================
    // FILTER BY CAPACITE
    // =========================
    public async Task<List<SalleResponseDTO>> GetByCapaciteAsync(int capacite)
    {
        return await _context.Salles
            .Where(s => s.Capacite >= capacite)
            .Select(s => MapToResponseDTO(s))
            .ToListAsync();
    }

    // =========================
    // FILTER TYPE + CAPACITE
    // =========================
    public async Task<List<SalleResponseDTO>> GetByTypeAndCapaciteAsync(string type, int capacite)
    {
        return await _context.Salles
            .Where(s => s.TypeSalle == type && s.Capacite >= capacite)
            .Select(s => MapToResponseDTO(s))
            .ToListAsync();
    }

    // =========================
    // DISPONIBILITE SIMPLE
    // =========================
    public async Task<bool> IsSalleDisponibleAsync(int idSalle, DateTime jour, TimeSpan heureDebut, TimeSpan heureFin)
    {
        var conflit = await _context.EmploisDuTemps
            .AnyAsync(e =>
                e.IdSalle == idSalle &&
                DateTime.Parse(e.Jour) == jour &&
                (
                    (heureDebut < e.HeureFin && heureFin > e.HeureDebut)
                )
            );

        return !conflit;
    }

    // =========================
    //  SEARCH 
    // =========================
    public async Task<List<SalleResponseDTO>> SearchAsync(SalleFilter filter)
    {
        var query = _context.Salles.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Type))
            query = query.Where(s => s.TypeSalle == filter.Type);

        if (filter.Capacite.HasValue)
            query = query.Where(s => s.Capacite >= filter.Capacite.Value);

        var salles = await query.ToListAsync();

        // Filtrage disponibilité si demandé
        if (filter.Jour.HasValue && filter.HeureDebut.HasValue && filter.HeureFin.HasValue)
        {
            var result = new List<SalleResponseDTO>();

            foreach (var salle in salles)
            {
                bool dispo = !await _context.EmploisDuTemps.AnyAsync(e =>
                    e.IdSalle == salle.IdSalle &&
                    DateTime.Parse(e.Jour) == filter.Jour &&
                    (filter.HeureDebut < e.HeureFin && filter.HeureFin > e.HeureDebut)
                );

                if (dispo)
                    result.Add(MapToResponseDTO(salle));
            }

            return result;
        }

        return salles.Select(s => MapToResponseDTO(s)).ToList();
    }

    // =========================
    // MAPPING 
    // =========================
    private static SalleResponseDTO MapToResponseDTO(Salle s)
    {
        return new SalleResponseDTO
        {
            IdSalle = s.IdSalle,
            NomSalle = s.NomSalle,
            TypeSalle = s.TypeSalle,
            Capacite = s.Capacite
        };
    }
    private static SalleCreateDTO MapToCreateDTO(Salle s)
    {
        return new SalleCreateDTO
        {
            NomSalle = s.NomSalle,
            TypeSalle = s.TypeSalle,
            Capacite = s.Capacite
        };
    }

    private static SalleUpdateDTO MapToUpdateDTO(Salle s)
    {
        return new SalleUpdateDTO
        {
            NomSalle = s.NomSalle,
            TypeSalle = s.TypeSalle,
            Capacite = s.Capacite
        };
    }
}