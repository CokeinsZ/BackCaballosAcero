using System.Security.Claims;
using Core.Entities;
using Core.Security;

namespace Core.Interfaces.Security;

public interface IJWTService
{
    string GenerateToken(User user, double expiration, int? branchId);
    ClaimsPrincipal ValidateToken(string token);
}