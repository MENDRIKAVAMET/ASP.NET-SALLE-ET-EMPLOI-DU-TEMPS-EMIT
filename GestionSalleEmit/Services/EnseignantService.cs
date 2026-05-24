using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Enseignant;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    public class EnseignantService : IEnseignantService
    {
        private readonly AppDbContext _context;

        public EnseignantService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // MAPPING
        // =========================
        private static EnseignantResponseDTO Map(Enseignant e)
        {
            return new EnseignantResponseDTO
            {
                IdEnseignant = e.IdEnseignant,
                NomEnseignant = e.NomEnseignant,
                PrenomEnseignant = e.PrenomEnseignant,
                EmailEnseignant = e.EmailEnseignant,
                PhoneEnseignant = e.PhoneEnseignant,
                GradeEnseignant = e.GradeEnseignant
            };
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<EnseignantResponseDTO>> GetAllAsync()
        {
            var data = await _context.Enseignants
                .OrderBy(e => e.NomEnseignant)
                .ThenBy(e => e.PrenomEnseignant)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<EnseignantResponseDTO?> GetByIdAsync(int id)
        {
            var enseignant = await _context.Enseignants
                .FirstOrDefaultAsync(e => e.IdEnseignant == id);

            return enseignant == null ? null : Map(enseignant);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<EnseignantResponseDTO> CreateAsync(EnseignantCreateDTO dto)
        {
            // validation email unique
            var emailExiste = await _context.Enseignants
                .AnyAsync(e => e.EmailEnseignant == dto.EmailEnseignant);

            if (emailExiste)
                throw new Exception("Email déjà utilisé");

            var entity = new Enseignant
            {
                NomEnseignant = dto.NomEnseignant,
                PrenomEnseignant = dto.PrenomEnseignant,
                EmailEnseignant = dto.EmailEnseignant,
                PhoneEnseignant = dto.PhoneEnseignant,
                GradeEnseignant = dto.GradeEnseignant
            };

            _context.Enseignants.Add(entity);
            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<EnseignantResponseDTO?> UpdateAsync(int id, EnseignantCreateDTO dto)
        {
            var entity = await _context.Enseignants
                .FirstOrDefaultAsync(e => e.IdEnseignant == id);

            if (entity == null)
                return null;

            // email unique sauf lui-même
            var emailExiste = await _context.Enseignants
                .AnyAsync(e =>
                    e.EmailEnseignant == dto.EmailEnseignant &&
                    e.IdEnseignant != id);

            if (emailExiste)
                throw new Exception("Email déjà utilisé");

            entity.NomEnseignant = dto.NomEnseignant;
            entity.PrenomEnseignant = dto.PrenomEnseignant;
            entity.EmailEnseignant = dto.EmailEnseignant;
            entity.PhoneEnseignant = dto.PhoneEnseignant;
            entity.GradeEnseignant = dto.GradeEnseignant;

            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Enseignants
                .FirstOrDefaultAsync(e => e.IdEnseignant == id);

            if (entity == null)
                return false;

            // vérifie si utilisé dans EDT
            var utilise = await _context.EmploisDuTemps
                .AnyAsync(e => e.IdEnseignant == id);

            if (utilise)
                throw new Exception("Impossible de supprimer cet enseignant");

            _context.Enseignants.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // SEARCH
        // =========================
        public async Task<List<EnseignantResponseDTO>> SearchAsync(string nom, string prenom)
        {
            var query = _context.Enseignants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nom))
            {
                query = query.Where(e =>
                    e.NomEnseignant.Contains(nom));
            }

            if (!string.IsNullOrWhiteSpace(prenom))
            {
                query = query.Where(e =>
                    e.PrenomEnseignant.Contains(prenom));
            }

            var data = await query
                .OrderBy(e => e.NomEnseignant)
                .ThenBy(e => e.PrenomEnseignant)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // DISPONIBILITE
        // =========================
        public async Task<List<bool>> IsDisponibleAsync(
            int idEnseignant,
            DateTime jour,
            TimeSpan heureDebut,
            TimeSpan heureFin)
        {
            var conflit = await _context.EmploisDuTemps
                .AnyAsync(e =>
                    e.IdEnseignant == idEnseignant &&
                    DateTime.Parse(e.Jour) == jour &&
                    heureDebut < e.HeureFin &&
                    heureFin > e.HeureDebut);

            return new List<bool> { !conflit };
        }

        // =========================
        // GET BY MATIERE
        // =========================
        public async Task<List<EnseignantResponseDTO>> GetByMatiereAsync(int idMatiere)
        {
            var data = await _context.Enseigners
                .Where(x => x.IdMatiere == idMatiere)
                .Select(x => x.Enseignant)
                .Distinct()
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // PLANNING ENSEIGNANT
        // =========================
        public async Task<List<EnseignantResponseDTO>> GetPlanningAsync(int idEnseignant)
        {
            var existe = await _context.Enseignants
                .AnyAsync(e => e.IdEnseignant == idEnseignant);

            if (!existe)
                throw new Exception("Enseignant introuvable");

            var data = await _context.Enseignants
                .Where(e => e.IdEnseignant == idEnseignant)
                .ToListAsync();

            return data.Select(Map).ToList();
        }
    }
}