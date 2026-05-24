using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Parcours;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    public class ParcoursService : IParcoursService
    {
        private readonly AppDbContext _context;

        public ParcoursService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // MAPPING
        // =========================
        private static ParcoursResponseDTO Map(Parcours p)
        {
            return new ParcoursResponseDTO
            {
                IdParcours = p.IdParcours,
                NomParcours = p.NomParcours,
                IdFiliere = p.IdFiliere,
                Filiere = p.Filiere?.NomFiliere
            };
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<ParcoursResponseDTO>> GetAllAsync()
        {
            var data = await _context.Parcours
                .Include(p => p.Filiere)
                .OrderBy(p => p.NomParcours)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<ParcoursResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.Parcours
                .Include(p => p.Filiere)
                .FirstOrDefaultAsync(p => p.IdParcours == id);

            return entity == null ? null : Map(entity);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<ParcoursResponseDTO> CreateAsync(ParcoursCreateDTO dto)
        {
            if (dto == null)
                throw new Exception("Données invalides");

            // vérifier filière
            var filiereExiste = await _context.Filieres
                .AnyAsync(f => f.IdFiliere == dto.IdFiliere);

            if (!filiereExiste)
                throw new Exception("Filière introuvable");

            // vérifier doublon
            var existe = await _context.Parcours
                .AnyAsync(p =>
                    p.NomParcours == dto.NomParcours &&
                    p.IdFiliere == dto.IdFiliere);

            if (existe)
                throw new Exception("Ce parcours existe déjà");

            var entity = new Parcours
            {
                NomParcours = dto.NomParcours,
                IdFiliere = dto.IdFiliere
            };

            _context.Parcours.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(p => p.Filiere)
                .LoadAsync();

            return Map(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<ParcoursResponseDTO?> UpdateAsync(int id, ParcoursUpdateDTO dto)
        {
            var entity = await _context.Parcours
                .Include(p => p.Filiere)
                .FirstOrDefaultAsync(p => p.IdParcours == id);

            if (entity == null)
                return null;

            // vérifier filière
            var filiereExiste = await _context.Filieres
                .AnyAsync(f => f.IdFiliere == dto.IdFiliere);

            if (!filiereExiste)
                throw new Exception("Filière introuvable");

            // vérifier doublon
            var existe = await _context.Parcours
                .AnyAsync(p =>
                    p.NomParcours == dto.NomParcours &&
                    p.IdFiliere == dto.IdFiliere &&
                    p.IdParcours != id);

            if (existe)
                throw new Exception("Ce parcours existe déjà");

            entity.NomParcours = dto.NomParcours;
            entity.IdFiliere = dto.IdFiliere;

            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Parcours
                .FirstOrDefaultAsync(p => p.IdParcours == id);

            if (entity == null)
                return false;

            // vérifier dépendance Niveau
            var utilise = await _context.Niveaux
                .AnyAsync(n => n.IdParcours == id);

            if (utilise)
                throw new Exception(
                    "Impossible de supprimer ce parcours car il est utilisé dans des niveaux");

            _context.Parcours.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // SEARCH
        // =========================
        public async Task<List<ParcoursResponseDTO>> SearchAsync(string nomParcours)
        {
            var query = _context.Parcours
                .Include(p => p.Filiere)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomParcours))
            {
                query = query.Where(p =>
                    p.NomParcours.Contains(nomParcours));
            }

            var data = await query
                .OrderBy(p => p.NomParcours)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // EXISTS
        // =========================
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Parcours
                .AnyAsync(p => p.IdParcours == id);
        }

        // =========================
        // NOM EXISTE
        // =========================
        public async Task<bool> NomExisteAsync(string nomParcours)
        {
            return await _context.Parcours
                .AnyAsync(p => p.NomParcours == nomParcours);
        }

        // =========================
        // GET PAR FILIERE
        // =========================
        public async Task<List<ParcoursResponseDTO>> GetByFiliereAsync(int idFiliere)
        {
            var filiereExiste = await _context.Filieres
                .AnyAsync(f => f.IdFiliere == idFiliere);

            if (!filiereExiste)
                throw new Exception("Filière introuvable");

            var data = await _context.Parcours
                .Include(p => p.Filiere)
                .Where(p => p.IdFiliere == idFiliere)
                .OrderBy(p => p.NomParcours)
                .ToListAsync();

            return data.Select(Map).ToList();
        }
    }
}