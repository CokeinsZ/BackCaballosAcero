using Application.Tools;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Email;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService: IUserService
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepo;
    private readonly IVerificationCodesRepository _verificationCodesRepo;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IEmailService emailService, IUserRepository userRepo, IVerificationCodesRepository verificationCodesRepo, IPasswordHasher<User> passwordHasher)
    {
        _emailService = emailService;
        _userRepo = userRepo;
        _verificationCodesRepo = verificationCodesRepo;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepo.GetAll();
    }

    public async Task<User?> GetById(int id)
    {
        return await _userRepo.GetById(id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _userRepo.GetByEmail(email);
    }

    public async Task<User> Register(CreateUserDto userDto)
    {
        var userExists = await _userRepo.GetByEmail(userDto.email);
        if (userExists != null) throw new Exception("Email already registered");
        
        if (userDto.password.Length < 4) throw new Exception("Password must be at least 4 characters");
        
        var userObj = new User()
        {
            name = userDto.name,
            last_name = userDto.last_name,
            identification_document = userDto.identification_document,
            country = userDto.country,
            city = userDto.city,
            email = userDto.email,
            password = userDto.password
        };
        var hashedPassword = _passwordHasher.HashPassword(userObj, userDto.password);
        userDto.password = hashedPassword;

        var newUser = await _userRepo.Add(userDto);

        await SendVerificationCode(newUser);
        
        return newUser;
    }

    public async Task<User> Update(UpdateUserDto dto, int id)
    {
        var updated = await _userRepo.Update(dto, id);
        if (updated == null)
            throw new KeyNotFoundException($"User with id {id} not found.");
        return updated;
    }

    public async Task<bool> VerifyUser(VerifyUserDto userDto)
    {
        var user = await _userRepo.GetByEmail(userDto.email);
        if (user == null) throw new Exception("Invalid Email");
        
        if (!user.status.Equals("Not Verified")) throw new Exception("Unable to verify user");
        
        var storedCode = await _verificationCodesRepo.GetCode(user.id);
        if (string.IsNullOrWhiteSpace(storedCode)) throw new Exception("User have no security codes");
        
        if (storedCode != userDto.verification_code) throw new Exception("Invalid verification code");

        await _verificationCodesRepo.Remove(user.id);
        return await _userRepo.VerifyUser(user.id);
    }

    public async Task<bool> ChangePassword(ResetPasswordDto userDto, int id)
    {
        var user = await _userRepo.GetById(id);
        if (user == null) throw new Exception("User not found");
        
        if (!user.status!.Equals("Active"))
            throw new Exception("User not active");
        
        var storedCode = await _verificationCodesRepo.GetCode(user.id);
        if (string.IsNullOrWhiteSpace(storedCode)) throw new Exception("User have no security codes");
        
        if (storedCode != userDto.verification_code) throw new Exception("Invalid verification code");

        var hashedPassword = _passwordHasher.HashPassword(user, userDto.password);
        userDto.password = hashedPassword;
        
        await _verificationCodesRepo.Remove(user.id);
        return await _userRepo.ChangePassword(id, userDto.password);
    }

    public async Task<bool> ChangeStatus(int id, string status)
    {
        return await _userRepo.ChangeStatus(id, status);
    }

    public async Task<bool> ChangeRole(int id, string role)
    {
        return await _userRepo.ChangeRole(id, role);
    }

    public async Task Delete(int id)
    {
        await _userRepo.Delete(id);
    }

    public async Task SendVerificationCode(int id)
    {
        var user = await _userRepo.GetById(id);
        if (user == null) throw new Exception("User not found");
        
        var code = RandomCodeGenerator.GenerateRandomCode(6);
        await _verificationCodesRepo.Add(code, user.id);
        await _emailService.SendVerificationEmail(user, code);
    }
    
    public async Task SendVerificationCode(User user)
    {
        var code = RandomCodeGenerator.GenerateRandomCode(6);
        await _verificationCodesRepo.Add(code, user.id);
        await _emailService.SendVerificationEmail(user, code);
    }
}