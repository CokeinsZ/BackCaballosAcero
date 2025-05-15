namespace Core.Interfaces.RepositoriesInterfaces;

public interface IVerificationCodesRepository
{
    Task<string?> GetCode(int userId);
    Task Add(string code, int userId);
    Task Remove(int userId);
}