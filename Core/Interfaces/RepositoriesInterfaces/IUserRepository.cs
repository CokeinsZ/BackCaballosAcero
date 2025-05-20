using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(int id);
    Task<User?> GetByEmail(string email);
    
    Task<User> Add(CreateUserDto user);
    
    Task<User?> Update(UpdateUserDto user, int id);
    Task<bool> VerifyUser(int id);
    Task<bool> ChangePassword(int id, string password);
    Task<bool> ChangeStatus(int id, string status);
    Task<bool> ChangeRole(int id, string role);
    Task Delete(int id);
}