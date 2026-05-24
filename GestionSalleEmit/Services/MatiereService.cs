using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Matiere;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    public class MatiereService : IMatiereService
    {
        private readonly AppDbContext _context;

        public MatiereService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // MAPPING
        // =========================
        private static MatiereResponseDTO Map(Matiere m)
        {
            return new MatiereResponseDTO
            {
                IdMatiere = m.IdMatiere,
                NomMatiere = m.NomMatiere,
                Semestre = m.Semestre,
                VolumeHoraire = m.VolumeHoraire,
                Coefficient = m.Coefficient
            };
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<MatiereResponseDTO>> GetAllAsync()
        {
            var data = await _context.Matieres
                .OrderBy(m => m.NomMatiere)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<MatiereResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.Matieres
                .FirstOrDefaultAsync(m => m.IdMatiere == id);

            return entity == null ? null : Map(entity);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<MatiereResponseDTO> CreateAsync(MatiereCreateDTO dto)
        {
            var nomExiste = await _context.Matieres
                .AnyAsync(m => m.NomMatiere == dto.NomMatiere);

            if (nomExiste)
                throw new Exception("Cette matière existe déjà");

            var entity = new Matiere
            {
                NomMatiere = dto.NomMatiere,
                Semestre = dto.Semestre,
                VolumeHoraire = dto.VolumeHoraire,
                Coefficient = dto.Coefficient
            };

            _context.Matieres.Add(entity);

            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<MatiereResponseDTO?> UpdateAsync(
            int id,
            MatiereCreateDTO dto)
        {
            var entity = await _context.Matieres
                .FirstOrDefaultAsync(m => m.IdMatiere == id);

            if (entity == null)
                return null;

            var nomExiste = await _context.Matieres
                .AnyAsync(m =>
                    m.NomMatiere == dto.NomMatiere &&
                    m.IdMatiere != id);

            if (nomExiste)
                throw new Exception("Cette matière existe déjà");

            entity.NomMatiere = dto.NomMatiere;
            entity.Semestre = dto.Semestre;
            entity.VolumeHoraire = dto.VolumeHoraire;
            entity.Coefficient = dto.Coefficient;

            await _context.SaveChangesAsync();

            return Map(entity);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Matieres
                .FirstOrDefaultAsync(m => m.IdMatiere == id);

            if (entity == null)
                return false;

            // Vérifie utilisation dans EDT
            var utiliseDansEDT = await _context.EmploisDuTemps
                .AnyAsync(e => e.IdMatiere == id);

            if (utiliseDansEDT)
                throw new Exception(
                    "Impossible de supprimer cette matière car elle est utilisée dans l'emploi du temps");

            _context.Matieres.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // SEARCH
        // =========================
        public async Task<List<MatiereResponseDTO>> SearchAsync(
            string nomMatiere = "",
            string semestre = "")
        {
            var query = _context.Matieres.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomMatiere))
            {
                query = query.Where(m =>
                    m.NomMatiere.Contains(nomMatiere));
            }

            if (!string.IsNullOrWhiteSpace(semestre))
            {
                query = query.Where(m =>
                    m.Semestre == semestre);
            }

            var data = await query
                .OrderBy(m => m.NomMatiere)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY NIVEAU
        // =========================
        public async Task<List<MatiereResponseDTO>> GetByNiveauAsync(int idNiveau)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Matiere)
                .Where(e => e.IdNiveau == idNiveau)
                .Select(e => e.Matiere)
                .Distinct()
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY ENSEIGNANT
        // =========================
        public async Task<List<MatiereResponseDTO>> GetByEnseignantAsync(int idEnseignant)
        {
            var data = await _context.Enseigners
                .Include(e => e.Matiere)
                .Where(e => e.IdEnseignant == idEnseignant)
                .Select(e => e.Matiere)
                .Distinct()
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY SEMESTRE
        // =========================
        public async Task<List<MatiereResponseDTO>> GetBySemestreAsync(string semestre)
        {
            var data = await _context.Matieres
                .Where(m => m.Semestre == semestre)
                .OrderBy(m => m.NomMatiere)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // EXISTS
        // =========================
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Matieres
                .AnyAsync(m => m.IdMatiere == id);
        }

        // =========================
        // NOM EXISTE
        // =========================
        public async Task<bool> NomExisteAsync(string nomMatiere)
        {
            return await _context.Matieres
                .AnyAsync(m => m.NomMatiere == nomMatiere);
        }
    }
}