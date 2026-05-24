using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Filiere;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    public class FiliereService : IFiliereService
    {
        private readonly AppDbContext _context;

        public FiliereService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // MAPPING
        // =========================
        private static FiliereResponseDTO Map(Filiere f)
        {
            return new FiliereResponseDTO
            {
                IdFiliere = f.IdFiliere,
                NomFiliere = f.NomFiliere
            };
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<FiliereResponseDTO>> GetAllAsync()
        {
            var data = await _context.Filieres
                .OrderBy(f => f.NomFiliere)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<FiliereResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.Filieres
                .FirstOrDefaultAsync(f => f.IdFiliere == id);

            return entity == null ? null : Map(entity);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<FiliereResponseDTO> CreateAsync(FiliereCreateDTO dto)
        {
            if (dto == null)
                throw new Exception("Données invalides");

            var existe = await _context.Filieres
                .AnyAsync(f => f.NomFiliere == dto.NomFiliere);

            if (existe)
                throw new Exception("Cette filière existe déjà");

            var entity = new Filiere
            {
                NomFiliere = dto.NomFiliere
            };

            _context.Filieres.Add(entity);
            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<FiliereResponseDTO?> UpdateAsync(int id, FiliereUpdateDTO dto)
        {
            var entity = await _context.Filieres
                .FirstOrDefaultAsync(f => f.IdFiliere == id);

            if (entity == null)
                return null;

            var existe = await _context.Filieres
                .AnyAsync(f =>
                    f.NomFiliere == dto.NomFiliere &&
                    f.IdFiliere != id);

            if (existe)
                throw new Exception("Cette filière existe déjà");

            entity.NomFiliere = dto.NomFiliere;

            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Filieres
                .FirstOrDefaultAsync(f => f.IdFiliere == id);

            if (entity == null)
                return false;

            // Vérifier dépendances
            var utiliseParcours = await _context.Parcours
                .AnyAsync(p => p.IdFiliere == id);

            var utiliseNiveau = await _context.Niveaux
                .AnyAsync(n => n.Parcours.IdFiliere == id);

            if (utiliseParcours || utiliseNiveau)
                throw new Exception("Impossible de supprimer cette filière car elle est utilisée");

            _context.Filieres.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // SEARCH
        // =========================
        public async Task<List<FiliereResponseDTO>> SearchAsync(string nomFiliere)
        {
            var query = _context.Filieres.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomFiliere))
            {
                query = query.Where(f =>
                    f.NomFiliere.Contains(nomFiliere));
            }

            var data = await query
                .OrderBy(f => f.NomFiliere)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // EXISTS
        // =========================
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Filieres
                .AnyAsync(f => f.IdFiliere == id);
        }

        // =========================
        // NOM EXISTE
        // =========================
        public async Task<bool> NomExisteAsync(string nomFiliere)
        {
            return await _context.Filieres
                .AnyAsync(f => f.NomFiliere == nomFiliere);
        }
    }
}