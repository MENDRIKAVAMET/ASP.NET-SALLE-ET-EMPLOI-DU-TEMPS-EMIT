using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.EmploiDuTemps;
using GestionSalleEmit.DTOs.Enseignant;
using GestionSalleEmit.DTOs.Matiere;
using GestionSalleEmit.DTOs.Niveau;
using GestionSalleEmit.DTOs.Salle;
using GestionSalleEmit.DTOs;
using GestionSalleEmit.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    public class EmploiDuTempsService : IEmploiDuTempsService
    {
        private readonly AppDbContext _context;

        public EmploiDuTempsService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // MAPPING
        // =========================
        private EmploiDuTempsResponseDTO Map(EmploiDuTemps e)
        {
            return new EmploiDuTempsResponseDTO
            {
                IdEDT = e.IdEDT,
                Jour = e.Jour,
                HeureDebut = e.HeureDebut,
                HeureFin = e.HeureFin,
                Semestre = e.Semestre,

                IdSalle = e.IdSalle,
                Salle = e.Salle?.NomSalle,

                IdEnseignant = e.IdEnseignant,
                Enseignant = e.Enseignant != null
                    ? $"{e.Enseignant.NomEnseignant} {e.Enseignant.PrenomEnseignant}"
                    : null,

                IdMatiere = e.IdMatiere,
                Matiere = e.Matiere?.NomMatiere,

                IdNiveau = e.IdNiveau,
                Niveau = e.Niveau?.NomNiveau,

                IdParcours = e.Niveau?.IdParcours ?? 0,
                Parcours = e.Niveau!.Parcours!.NomParcours,

                IdFiliere = e.Niveau?.Parcours?.IdFiliere ?? 0,
                Filiere = e.Niveau?.Parcours?.Filiere?.NomFiliere,
            };
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<EmploiDuTempsResponseDTO>> GetAllAsync()
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .OrderBy(e => e.Jour)
                .ThenBy(e => e.HeureDebut)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<EmploiDuTempsResponseDTO?> GetByIdAsync(int id)
        {
            var e = await _context.EmploisDuTemps
                .Include(x => x.Salle)
                .Include(x => x.Enseignant)
                .Include(x => x.Matiere)
                .Include(x => x.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .FirstOrDefaultAsync(x => x.IdEDT == id);

            return e == null ? null : Map(e);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<EmploiDuTempsResponseDTO> CreateAsync(EmploiDuTempsCreateDTO dto)
        {
            await VerifierConflitsAsync(dto);

            var entity = new EmploiDuTemps
            {
                Jour = dto.Jour,
                HeureDebut = dto.HeureDebut,
                HeureFin = dto.HeureFin,
                Semestre = dto.Semestre,
                IdSalle = dto.IdSalle,
                IdEnseignant = dto.IdEnseignant,
                IdMatiere = dto.IdMatiere,
                IdNiveau = dto.IdNiveau
            };

            _context.EmploisDuTemps.Add(entity);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(entity.IdEDT)
                   ?? throw new Exception("Erreur création EDT");
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<EmploiDuTempsResponseDTO?> UpdateAsync(int id, EmploiDuTempsUpdateDTO dto)
        {
            var entity = await _context.EmploisDuTemps.FindAsync(id);

            if (entity == null)
                return null;

            // validation conflits (on ignore l’élément actuel)
            await VerifierConflitsAsync(new EmploiDuTempsCreateDTO
            {
                Jour = dto.Jour,
                HeureDebut = dto.HeureDebut,
                HeureFin = dto.HeureFin,
                Semestre = dto.Semestre,
                IdSalle = dto.IdSalle,
                IdEnseignant = dto.IdEnseignant,
                IdMatiere = dto.IdMatiere,
                IdNiveau = dto.IdNiveau
            });

            entity.Jour = dto.Jour;
            entity.HeureDebut = dto.HeureDebut;
            entity.HeureFin = dto.HeureFin;
            entity.Semestre = dto.Semestre;
            entity.IdSalle = dto.IdSalle;
            entity.IdEnseignant = dto.IdEnseignant;
            entity.IdMatiere = dto.IdMatiere;
            entity.IdNiveau = dto.IdNiveau;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.EmploisDuTemps.FindAsync(id);

            if (entity == null)
                return false;

            _context.EmploisDuTemps.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // FILTRES
        // =========================
        public async Task<List<EmploiDuTempsResponseDTO>> GetByEnseignantAsync(int id)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .Where(e => e.IdEnseignant == id)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        public async Task<List<EmploiDuTempsResponseDTO>> GetBySalleAsync(int id)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .Where(e => e.IdSalle == id)
                .ToListAsync();

            return data.Select(Map).ToList();
        }

        public async Task<List<EmploiDuTempsResponseDTO>> GetByNiveauAsync(int id)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .Where(e => e.IdNiveau == id)
                .ToListAsync();

            return data.Select(Map).ToList();
        }
        public async Task<List<EmploiDuTempsResponseDTO>> GetByMatiereAsync(int id)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .Where(e => e.IdMatiere == id)
                .ToListAsync();
            return data.Select(Map).ToList();
        }

        public async Task<List<EmploiDuTempsResponseDTO>> GetByParcoursAsync(int id)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .Where(e => e.Niveau != null && e.Niveau.Parcours != null && e.Niveau.Parcours.IdParcours == id)
                .ToListAsync();
            return data.Select(Map).ToList();
        }

        public async Task<List<EmploiDuTempsResponseDTO>> GetByFiliereAsync(int id)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .Where(e => e.Niveau != null && e.Niveau.Parcours != null && e.Niveau.Parcours.Filiere != null && e.Niveau.Parcours.Filiere.IdFiliere == id)
                .ToListAsync();
            return data.Select(Map).ToList();
        }

        public async Task<List<EmploiDuTempsResponseDTO>> GetByWeekAsync(DateTime start, DateTime end)
        {
            var data = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                    .ThenInclude(n => n.Parcours)
                        .ThenInclude(p => p.Filiere)
                .Where(e => DateTime.Parse(e.Jour) >= start && DateTime.Parse(e.Jour) <= end)
                .ToListAsync();
            return data.Select(Map).ToList();
        }

        // =========================
        // VALIDATION MÉTIER CENTRALISÉE
        // =========================
        public async Task VerifierConflitsAsync(EmploiDuTempsCreateDTO dto)
        {
            if (dto.HeureDebut >= dto.HeureFin)
                throw new Exception("Heure de début invalide");

            // existence FK
            if (!await _context.Salles.AnyAsync(x => x.IdSalle == dto.IdSalle))
                throw new Exception("Salle inexistante");

            if (!await _context.Enseignants.AnyAsync(x => x.IdEnseignant == dto.IdEnseignant))
                throw new Exception("Enseignant inexistant");

            if (!await _context.Matieres.AnyAsync(x => x.IdMatiere == dto.IdMatiere))
                throw new Exception("Matière inexistante");

            if (!await _context.Niveaux.AnyAsync(x => x.IdNiveau == dto.IdNiveau))
                throw new Exception("Niveau inexistant");

            // enseignant-matière
            var ok = await _context.Enseigners.AnyAsync(x =>
                x.IdEnseignant == dto.IdEnseignant &&
                x.IdMatiere == dto.IdMatiere);

            if (!ok)
                throw new Exception("Enseignant non autorisé pour cette matière");

            // conflit salle
            var conflitSalle = await _context.EmploisDuTemps.AnyAsync(e =>
                e.IdSalle == dto.IdSalle &&
                e.Jour == dto.Jour &&
                dto.HeureDebut < e.HeureFin &&
                dto.HeureFin > e.HeureDebut);

            if (conflitSalle)
                throw new Exception("Conflit salle");

            // conflit enseignant
            var conflitEns = await _context.EmploisDuTemps.AnyAsync(e =>
                e.IdEnseignant == dto.IdEnseignant &&
                e.Jour == dto.Jour &&
                dto.HeureDebut < e.HeureFin &&
                dto.HeureFin > e.HeureDebut);

            if (conflitEns)
                throw new Exception("Conflit enseignant");
        }
    }
}