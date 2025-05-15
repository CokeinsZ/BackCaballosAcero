using Core.DTOs;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginUserDto> _loginUserValidator;


    public AuthController(IAuthService authService, IValidator<LoginUserDto> loginUserValidator)
    {
        _authService = authService;
        _loginUserValidator = loginUserValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
    {
        var validationResult = await _loginUserValidator.ValidateAsync(userDto);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var response = await _authService.Login(userDto);
        return Ok(new { response });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken) || dto.RefreshToken.Length == 0)
        {
            return BadRequest("Refresh token is required");
        }
        
        var newToken = await _authService.RefreshToken(dto.RefreshToken);
        return Ok(new { AccessToken = newToken });
    }
}