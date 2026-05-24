using GestionSalleEmit.Data;
using GestionSalleEmit.DTOs.Auth;
using GestionSalleEmit.DTOs.JWT;
using GestionSalleEmit.Models;
using GestionSalleEmit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionSalleEmit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.EmailUtilisateur == dto.Email);

            if (user == null)
                return Unauthorized("Email ou mot de passe incorrect");

            if (!BCrypt.Net.BCrypt.Verify(dto.MotDePasse, user.MotDePasseUtilisateur))
                return Unauthorized("Email ou mot de passe incorrect");

            var token = _jwtService.GenerateToken(new JwtUserDto
            {
                Id = user.IdUtilisateur,
                Email = user.EmailUtilisateur,
                Role = user.RoleUtilisateur
            });

            return Ok(new AuthResponseDTO
            {
                Token = token,
                Role = user.RoleUtilisateur
            });
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.EmailUtilisateur == dto.Email);

            if (existingUser != null)
                return BadRequest("Cet email est déjà utilisé");

            string role;
            if(dto.Role == "Responsable" || dto.Role == "responsable")
            {
                role = Constants.Roles.Responsable;
            }
            else if(dto.Role == "Admin" || dto.Role == "admin")
            {
                role = Constants.Roles.Admin;
            }
            else
            {
                return BadRequest("Role invalide. Veuillez choisir 'Admin' ou 'Responsable'.");
            }

            var user = new Utilisateur
            {
                NomUtilisateur = dto.Nom,
                EmailUtilisateur = dto.Email,
                MotDePasseUtilisateur = BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse),
                RoleUtilisateur = role
            };

            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur créé avec succès");
        }

    }   
}