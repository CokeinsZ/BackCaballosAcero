using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services;

public interface IUserService
{ 
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(int id);
    Task<User?> GetByEmail(string email);
    Task<User> Register(CreateUserDto userDto);
    Task<User> Update(UpdateUserDto dto, int id);
    Task<bool> VerifyUser(VerifyUserDto userDto);
    Task<bool> ChangePassword(ResetPasswordDto userDto, int id);
    Task<bool> ChangeStatus(int id, string status);
    Task Delete(int id);
    Task SendVerificationCode(int id);
    Task SendVerificationCode(User user);
}