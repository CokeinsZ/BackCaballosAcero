using System.Security.Claims;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService: IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly JWTService _jwtService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepo, IRefreshTokenRepository refreshTokenRepo, JWTService jwtService, IPasswordHasher<User> passwordHasher)
    {
        _userRepo = userRepo;
        _refreshTokenRepo = refreshTokenRepo;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> Login(LoginUserDto userDto)
    {
        var user = await _userRepo.GetByEmail(userDto.email);
        if (user == null) throw new Exception("Invalid Email");
        
        if (!user.status!.Equals("Active")) throw new Exception("User not active");

        var passwordValid = _passwordHasher.VerifyHashedPassword(user, user.password, userDto.password) !=
                            PasswordVerificationResult.Failed;
        if (!passwordValid) throw new Exception("Incorrect password");

        var accesToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user);
        
        await _refreshTokenRepo.AddRefreshToken(user.id, refreshToken);

        return new AuthResponseDto()
        {
            AccesToken = accesToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<string> RefreshToken(string refreshToken)
    {
        try
        {
            var claimsPrincipal = _jwtService.ValidateToken(refreshToken);
        
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("Token inválido: no contiene el ID del usuario");

            int.TryParse(userIdClaim.Value, out int userId);
        
            // Obtener el usuario por su ID
            var user = await _userRepo.GetById(userId);
            if (user == null)
                throw new Exception("Usuario no encontrado");
            
            var storedToken = await _refreshTokenRepo.GetRefreshToken(userId);
            if (string.IsNullOrWhiteSpace(storedToken))
                throw new Exception("Token inválido: Token revocado");

            if (storedToken != refreshToken)
                throw new Exception("Token invalido");
        
            // Generar un nuevo token de acceso
            return _jwtService.GenerateAccessToken(user);
        }
        catch (SecurityTokenException ex)
        {
            throw new Exception($"Token inválido: {ex.Message}");
        }
    }


}