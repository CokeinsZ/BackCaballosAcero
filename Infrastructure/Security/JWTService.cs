
using System.Text;
using Core.Entities;
using Core.Interfaces.Security;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JWTService : IJWTService
{
    private readonly JwtSettings _jwtSettings;

    public JWTService(IOptions<JwtSettings> jwtSettings, IPasswordHasher<User> passwordHasher)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(User user, double expiration, int? branchId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Role, user.role_id.ToString()!),
        };
        
        // Si es usuario de tipo 'Branch', añadimos también el branchId
        if (user.role_id.ToString() == IUserRole.Branch && branchId.HasValue)
        {
            claims.Add(new Claim("branchId", branchId.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(User user, int? branchId)
    {
        return GenerateToken(user, _jwtSettings.AccessTokenExpiration, branchId);
    }

    public string GenerateRefreshToken(User user, int? branchId)
    {
        return GenerateToken(user, _jwtSettings.RefreshTokenExpiration, branchId);
    }
    
    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        return tokenHandler.ValidateToken(token, parameters, out _);
    }

}