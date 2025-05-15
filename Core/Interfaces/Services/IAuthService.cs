using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> Login(LoginUserDto userDto);
    
    Task<string> RefreshToken(string refreshToken);
}