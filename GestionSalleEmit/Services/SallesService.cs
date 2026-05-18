using GestionSalleEmit.Data;
using GestionSalleEmit.Models;
using GestionSalleEmit.DTOs.Salle;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Services
{
    /// <summary>
    /// Implémentation du service pour la création et la modification des salles.
    /// </summary>
    public class SallesService : ISallesService
    {
        private readonly AppDbContext _context;

        // Injection du contexte de base de données de l'application
        public SallesService(AppDbContext context)
        {
            _context = context;
        }

        // Logique métier pour le POST
        public async Task<Salle> CreateSalleAsync(SalleCreateDTO createDTO)
        {
            // RG4 : La capacité d'une salle doit être supérieure à 0
            if (createDTO.Capacite <= 0)
            {
                throw new ArgumentException("La capacité de la salle doit être supérieure à 0.");
            }

            // Transfert des données du DTO vers l'entité Salle (Exactement comme dans le contrôleur)
            var salle = new Salle
            {
                NomSalle = createDTO.NomSalle,
                Capacite = createDTO.Capacite,
                TypeSalle = createDTO.TypeSalle
            };

            _context.Salles.Add(salle);
            await _context.SaveChangesAsync();

            return salle; // Retourne la salle créée avec son ID généré par la base
        }

        // Logique métier pour le PUT
        public async Task<bool> UpdateSalleAsync(int id, SalleUpdateDTO updateDTO)
        {
            // Recherche de la salle dans la base de données
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
            {
                return false; // La salle n'existe pas
            }

            // RG4 : Validation de la capacité lors de la modification
            if (updateDTO.Capacite <= 0)
            {
                throw new ArgumentException("La capacité de la salle doit être supérieure à 0.");
            }

            // Mise à jour des valeurs (Exactement comme dans le contrôleur)
            salle.NomSalle = updateDTO.NomSalle;
            salle.Capacite = updateDTO.Capacite;
            salle.TypeSalle = updateDTO.TypeSalle;

            _context.Entry(salle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true; // Modification enregistrée avec succès
            }
            catch (DbUpdateConcurrencyException)
            {
                // Vérification si la salle existe toujours en cas de conflit
                if (!_context.Salles.Any(e => e.IdSalle == id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}