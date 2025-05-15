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
            
        var code = RandomCodeGenerator.GenerateRandomCode(6);
        await _verificationCodesRepo.Add(code, newUser.id);
        await _emailService.SendVerificationEmail(newUser, code);
        
        return newUser;
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
}