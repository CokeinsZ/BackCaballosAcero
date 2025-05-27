using Core.Entities;

namespace Core.DTOs;

public class AuthResponseDto
{
    public required string AccesToken { get; set; }
    public required string RefreshToken { get; set; }
    public User User { get; set; }
}

public class RefreshTokenDto
{
    public required string RefreshToken { get; set; }
    public int? brachId { get; set; }
}