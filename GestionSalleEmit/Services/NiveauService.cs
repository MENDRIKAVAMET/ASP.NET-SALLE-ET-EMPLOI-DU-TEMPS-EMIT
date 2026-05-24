using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Niveau;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    public class NiveauService : INiveauService
    {
        private readonly AppDbContext _context;

        public NiveauService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // MAPPING
        // =========================
        private static NiveauResponseDTO Map(Niveau n)
        {
            return new NiveauResponseDTO
            {
                IdNiveau = n.IdNiveau,
                NomNiveau = n.NomNiveau,
                IdParcours = n.IdParcours
            };
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<NiveauResponseDTO>> GetAllAsync()
        {
            var data = await _context.Niveaux
                .OrderBy(n => n.NomNiveau)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<NiveauResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.Niveaux
                .FirstOrDefaultAsync(n => n.IdNiveau == id);

            return entity == null ? null : Map(entity);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<NiveauResponseDTO> CreateAsync(NiveauCreateDTO dto)
        {
            // 1. Vérifier si filière existe (IMPORTANT)
            var filiereExiste = await _context.Filieres
                .AnyAsync(f => f.IdFiliere == dto.IdParcours);

            if (!filiereExiste)
                throw new Exception("Filière invalide");

            // 2. Vérifier doublon
            var existe = await _context.Niveaux
                .AnyAsync(n =>
                    n.NomNiveau == dto.NomNiveau &&
                    n.IdParcours == dto.IdParcours);

            if (existe)
                throw new Exception("Ce niveau existe déjà pour cette filière");

            // 3. Création
            var entity = new Niveau
            {
                NomNiveau = dto.NomNiveau,
                IdParcours = dto.IdParcours
            };

            _context.Niveaux.Add(entity);
            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<NiveauResponseDTO?> UpdateAsync(int id, NiveauCreateDTO dto)
        {
            var entity = await _context.Niveaux
                .FirstOrDefaultAsync(n => n.IdNiveau == id);

            if (entity == null)
                return null;

            // 1. Vérifier que la filière existe
            var filiereExiste = await _context.Filieres
                .AnyAsync(f => f.IdFiliere == dto.IdParcours);

            if (!filiereExiste)
                throw new Exception("Filière invalide");

            // 2. Vérifier doublon (Nom + Filière)
            var existe = await _context.Niveaux
                .AnyAsync(n =>
                    n.NomNiveau == dto.NomNiveau &&
                    n.IdParcours == dto.IdParcours &&
                    n.IdNiveau != id);

            if (existe)
                throw new Exception("Ce niveau existe déjà pour cette filière");

            // 3. Mise à jour des champs
            entity.NomNiveau = dto.NomNiveau;
            entity.IdParcours = dto.IdParcours;

            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Niveaux
                .FirstOrDefaultAsync(n => n.IdNiveau == id);

            if (entity == null)
                return false;

            // Vérifie utilisation dans emploi du temps
            var utilise = await _context.EmploisDuTemps
                .AnyAsync(e => e.IdNiveau == id);

            if (utilise)
                throw new Exception("Impossible de supprimer ce niveau car il est utilisé dans l'emploi du temps");

            _context.Niveaux.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // SEARCH
        // =========================
        public async Task<List<NiveauResponseDTO>> SearchAsync(string nomNiveau = "")
        {
            var query = _context.Niveaux.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomNiveau))
            {
                query = query.Where(n =>
                    n.NomNiveau.Contains(nomNiveau));
            }

            var data = await query
                .OrderBy(n => n.NomNiveau)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // EXISTS
        // =========================
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Niveaux
                .AnyAsync(n => n.IdNiveau == id);
        }

        // =========================
        // NOM EXISTE
        // =========================
        public async Task<bool> NomExisteAsync(string nomNiveau)
        {
            return await _context.Niveaux
                .AnyAsync(n => n.NomNiveau == nomNiveau);
        }

        // =========================
        // MATIERES D'UN NIVEAU
        // =========================
        public async Task<List<NiveauResponseDTO>> GetMatieresAsync(int idNiveau)
        {
            var existe = await _context.Niveaux
                .AnyAsync(n => n.IdNiveau == idNiveau);

            if (!existe)
                throw new Exception("Niveau introuvable");

            var data = await _context.EmploisDuTemps
                .Include(e => e.Matiere)
                .Where(e => e.IdNiveau == idNiveau)
                .Select(e => e.Matiere)
                .Distinct()
                .ToListAsync();

            return data.Select(m => new NiveauResponseDTO
            {
                IdNiveau = idNiveau,
                NomNiveau = null // optionnel si tu veux enrichir
            }).ToList();
        }

        // =========================
        // EMPLOI DU TEMPS D'UN NIVEAU
        // =========================
        public async Task<List<NiveauResponseDTO>> GetEmploiDuTempsAsync(int idNiveau)
        {
            var existe = await _context.Niveaux
                .AnyAsync(n => n.IdNiveau == idNiveau);

            if (!existe)
                throw new Exception("Niveau introuvable");

            var data = await _context.EmploisDuTemps
                .Where(e => e.IdNiveau == idNiveau)
                .ToListAsync();

            return data.Select(e => new NiveauResponseDTO
            {
                IdNiveau = idNiveau,
                NomNiveau = null
            }).ToList();
        }
    }
}