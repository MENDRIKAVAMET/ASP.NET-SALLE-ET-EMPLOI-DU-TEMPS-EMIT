using GestionSalleEmit.Constants;
using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Auth;
using GestionSalleEmit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL USERS
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Utilisateurs
                .Select(u => new
                {
                    u.IdUtilisateur,
                    u.NomUtilisateur,
                    u.EmailUtilisateur,
                    u.RoleUtilisateur
                })
                .ToListAsync();

            return Ok(users);
        }

        // =========================
        // GET USER BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Utilisateurs
                .Where(u => u.IdUtilisateur == id)
                .Select(u => new
                {
                    u.IdUtilisateur,
                    u.NomUtilisateur,
                    u.EmailUtilisateur,
                    u.RoleUtilisateur
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("Utilisateur introuvable");

            return Ok(user);
        }

        // =========================
        // UPDATE USER
        // =========================
        [HttpPut]
        public async Task<IActionResult> Update(UpdateUtilisateurDTO dto)
        {
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.IdUtilisateur == dto.IdUtilisateur);

            if (user == null)
                return NotFound("Utilisateur introuvable");

            user.NomUtilisateur = dto.Nom;
            user.EmailUtilisateur = dto.Email;
            user.RoleUtilisateur = dto.Role;

            await _context.SaveChangesAsync();

            return Ok("Utilisateur mis à jour avec succès");
        }

        // =========================
        // DELETE USER
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.IdUtilisateur == id);

            if (user == null)
                return NotFound("Utilisateur introuvable");

            _context.Utilisateurs.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur supprimé avec succès");
        }

        // =========================
        // CHANGE PASSWORD
        // =========================
        [HttpPost("change-password")]
        [AllowAnonymous] // tu peux adapter plus tard pour "current user"
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.IdUtilisateur == dto.IdUtilisateur);

            if (user == null)
                return NotFound("Utilisateur introuvable");

            // vérifier ancien mot de passe
            if (!BCrypt.Net.BCrypt.Verify(dto.AncienMotDePasse, user.MotDePasseUtilisateur))
                return BadRequest("Ancien mot de passe incorrect");

            // update password
            user.MotDePasseUtilisateur = BCrypt.Net.BCrypt.HashPassword(dto.NouveauMotDePasse);

            await _context.SaveChangesAsync();

            return Ok("Mot de passe modifié avec succès");
        }
    }
}