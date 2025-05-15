namespace Core.Interfaces.RepositoriesInterfaces;

public interface IRefreshTokenRepository
{
    Task<string> GetRefreshToken(int userId);
    Task AddRefreshToken(int userId, string refreshToken);
    Task RemoveRefreshToken(int userId);
}