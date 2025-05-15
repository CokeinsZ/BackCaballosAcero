using System.Security.Claims;
using Core.Entities;
using Core.Security;

namespace Core.Interfaces.Security;

public interface IJWTService
{
    string GenerateToken(User user, double expiration);
    ClaimsPrincipal ValidateToken(string token);
}