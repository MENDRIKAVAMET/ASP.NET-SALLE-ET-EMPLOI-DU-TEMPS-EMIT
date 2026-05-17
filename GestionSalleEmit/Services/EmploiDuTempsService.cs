using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs;
using GestionSalleEmit.DTOs.EmploiDuTemps;
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

        public async Task<bool> VerifierConflitSalle(EmploiDuTempsCreateDTO dto)
        {
            return await _context.EmploisDuTemps.AnyAsync(e =>
                e.IdSalle == dto.IdSalle &&
                e.Jour == dto.Jour &&
                (dto.HeureDebut < e.HeureFin &&
                dto.HeureFin > e.HeureDebut)
            );
        }
        public async Task<bool> VerifierConflitEnseignant(EmploiDuTempsCreateDTO dto)
        {
            return await _context.EmploisDuTemps.AnyAsync(e =>
                e.IdEnseignant == dto.IdEnseignant &&
                e.Jour == dto.Jour &&
                (dto.HeureDebut < e.HeureFin &&
                dto.HeureFin > e.HeureDebut)
            );
        }

        public async Task<EmploiDuTemps> creerEmploiDuTemps(EmploiDuTempsCreateDTO dto)
        {
            if(dto.HeureDebut >= dto.HeureFin)
            {
                throw new Exception("L'heure de début doit être inférieure à l'heure de fin");
            }
            var joursAutorisers = new List<string>
            {
            "Lundi",
            "Mardi",
            "Mercredi",
            "Jeudi",
            "Vendredi",
            "Samedi"
            };

            if(!joursAutorisers.Contains(dto.Jour))
            {
                throw new Exception("Jour invalide.");
            }
            TimeSpan heureMin = new TimeSpan(7, 0, 0);
            TimeSpan heureMax = new TimeSpan(18, 0, 0);

            if(dto.HeureDebut < heureMin || dto.HeureFin > heureMax)
            {
                throw new Exception("Les heures doivent être comprises entre 7h00 et 18h00.");
            }

            var salleExiste = await _context.Salles.AnyAsync(s => s.IdSalle == dto.IdSalle);

            if (!salleExiste)
            {
                throw new Exception("Salle introuvable");
            }

            var enseignantExiste = await _context.Enseignants.AnyAsync(e => e.IdEnseignant == dto.IdEnseignant);
            if (!enseignantExiste)
            {
                throw new Exception("Enseignant introuvable");
            }

            var matiereExiste = await _context.Matieres.AnyAsync(m => m.IdMatiere == dto.IdMatiere);
            if(!matiereExiste)
            {
                throw new Exception("Matière introuvable");
            }

            var niveauExiste = await _context.Niveaux.AnyAsync(n => n.IdNiveau == dto.IdNiveau);
            if(!niveauExiste)
            {
                throw new Exception("Niveau introuvable");
            }

            var enseigneMatiere = await _context.Enseigners.AnyAsync(em => em.IdEnseignant == dto.IdEnseignant && em.IdMatiere == dto.IdMatiere);
            if (!enseigneMatiere)
            {
                throw new Exception("Cet enseignant n'enseigne pas cette matière");
            }

            var conflitSalle = await VerifierConflitSalle(dto);
            if (conflitSalle)
            {
                throw new Exception("La salle est déjà occupée à ce créneau horaire");
            }

            var conflitEnseignant = await VerifierConflitEnseignant(dto);
            if(conflitEnseignant)
            {
                throw new Exception("L'enseignant est déjà occupé à ce créneau horaire");
            }

            var doublon = await _context.EmploisDuTemps.AnyAsync(e =>
                e.Jour == dto.Jour &&
                e.HeureDebut == dto.HeureDebut &&
                e.HeureFin == dto.HeureFin &&
                e.IdSalle == dto.IdSalle &&
                e.IdEnseignant == dto.IdEnseignant &&
                e.IdMatiere == dto.IdMatiere &&
                e.IdNiveau == dto.IdNiveau
            );

            if (doublon)
            {
                throw new Exception("Cet emploi du temps existe déjà.");
            }

            var coursJour = await _context.EmploisDuTemps.Where(e => e.IdEnseignant == dto.IdEnseignant && e.Jour == dto.Jour).ToListAsync();

        }
        
    }
}
