using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Enseigner;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    public class EnseignerService : IEnseignerService
    {
        private readonly AppDbContext _context;

        public EnseignerService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // MAPPING
        // =========================
        private static EnseignerResponseDTO Map(Enseigner e)
        {
            return new EnseignerResponseDTO
            {
                IdEnseignant = e.IdEnseignant,
                NomEnseignant = e.Enseignant?.NomEnseignant,

                IdMatiere = e.IdMatiere,
                NomMatiere = e.Matiere?.NomMatiere
            };
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<EnseignerResponseDTO>> GetAllAsync()
        {
            var data = await _context.Enseigners
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .OrderBy(e => e.Enseignant.NomEnseignant)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET PAR ENSEIGNANT
        // =========================
        public async Task<List<EnseignerResponseDTO>>
            GetByEnseignantAsync(int idEnseignant)
        {
            var enseignantExiste = await _context.Enseignants
                .AnyAsync(e => e.IdEnseignant == idEnseignant);

            if (!enseignantExiste)
                throw new Exception("Enseignant introuvable");

            var data = await _context.Enseigners
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Where(e => e.IdEnseignant == idEnseignant)
                .OrderBy(e => e.Matiere.NomMatiere)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET PAR MATIERE
        // =========================
        public async Task<List<EnseignerResponseDTO>>
            GetByMatiereAsync(int idMatiere)
        {
            var matiereExiste = await _context.Matieres
                .AnyAsync(m => m.IdMatiere == idMatiere);

            if (!matiereExiste)
                throw new Exception("Matière introuvable");

            var data = await _context.Enseigners
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Where(e => e.IdMatiere == idMatiere)
                .OrderBy(e => e.Enseignant.NomEnseignant)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // CREATE RELATION
        // =========================
        public async Task<EnseignerResponseDTO>
            CreateAsync(EnseignerCreateDTO dto)
        {
            // validation DTO
            if (dto == null)
                throw new Exception("Données invalides");

            // vérifier enseignant
            var enseignant = await _context.Enseignants
                .FirstOrDefaultAsync(e =>
                    e.IdEnseignant == dto.IdEnseignant);

            if (enseignant == null)
                throw new Exception("Enseignant introuvable");

            // vérifier matière
            var matiere = await _context.Matieres
                .FirstOrDefaultAsync(m =>
                    m.IdMatiere == dto.IdMatiere);

            if (matiere == null)
                throw new Exception("Matière introuvable");

            // vérifier relation existante
            var relationExiste = await _context.Enseigners
                .AnyAsync(e =>
                    e.IdEnseignant == dto.IdEnseignant &&
                    e.IdMatiere == dto.IdMatiere);

            if (relationExiste)
                throw new Exception(
                    "Cette relation existe déjà");

            // création relation
            var entity = new Enseigner
            {
                IdEnseignant = dto.IdEnseignant,
                IdMatiere = dto.IdMatiere
            };

            _context.Enseigners.Add(entity);

            await _context.SaveChangesAsync();

            // recharge navigation
            await _context.Entry(entity)
                .Reference(e => e.Enseignant)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.Matiere)
                .LoadAsync();

            return Map(entity);
        }

        // =========================
        // DELETE RELATION
        // =========================
        public async Task<bool> DeleteAsync(
            int idEnseignant,
            int idMatiere)
        {
            var entity = await _context.Enseigners
                .FirstOrDefaultAsync(e =>
                    e.IdEnseignant == idEnseignant &&
                    e.IdMatiere == idMatiere);

            if (entity == null)
                return false;

            // vérifier utilisation dans emploi du temps
            var utilise = await _context.EmploisDuTemps
                .AnyAsync(e =>
                    e.IdEnseignant == idEnseignant &&
                    e.IdMatiere == idMatiere);

            if (utilise)
                throw new Exception(
                    "Impossible de supprimer cette relation car elle est utilisée dans l'emploi du temps");

            _context.Enseigners.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // EXISTS
        // =========================
        public async Task<bool> ExistsAsync(
            int idEnseignant,
            int idMatiere)
        {
            return await _context.Enseigners
                .AnyAsync(e =>
                    e.IdEnseignant == idEnseignant &&
                    e.IdMatiere == idMatiere);
        }
    }
}