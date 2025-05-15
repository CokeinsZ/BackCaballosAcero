using Core.DTOs;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserDto> _createUserValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;
    private readonly IValidator<VerifyUserDto> _verifyUserValidator;

    public UserController(IUserService userService, IValidator<CreateUserDto> createUserValidator, IValidator<UpdateUserDto> updateUserValidator, IValidator<VerifyUserDto> verifyUserValidator)
    {
        _userService = userService;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
        _verifyUserValidator = verifyUserValidator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
    {
        var validationResult = await _createUserValidator.ValidateAsync(userDto);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var user = await _userService.Register(userDto);
        return Ok(user);
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyUser([FromBody] VerifyUserDto userDto)
    {
        var validationResult = await _verifyUserValidator.ValidateAsync(userDto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var response = await _userService.VerifyUser(userDto);
        return Ok(new { response });
    }

}