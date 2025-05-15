namespace Core.DTOs;

public class AuthResponseDto
{
    public required string AccesToken { get; set; }
    public required string RefreshToken { get; set; }
}

public class RefreshTokenDto
{
    public required string RefreshToken { get; set; }
}