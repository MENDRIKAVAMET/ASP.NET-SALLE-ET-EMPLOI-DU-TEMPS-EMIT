using GestionSalleEmit.DTOs.JWT;

namespace GestionSalleEmit.Services
{
    public interface IJwtService
    {
        string GenerateToken(JwtUserDto user);
    }
}
