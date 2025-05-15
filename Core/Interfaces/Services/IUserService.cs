using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<User> Register(CreateUserDto userDto);
    Task<bool> VerifyUser(VerifyUserDto userDto);
}