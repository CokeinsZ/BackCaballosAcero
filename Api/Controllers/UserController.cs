using System.Security.Claims;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IValidator<ResetPasswordDto> _resetPwdValidator;
    private readonly IValidator<ChangeStatusDto> _changeStatusValidator;

    public UserController(IUserService userService, IValidator<CreateUserDto> createUserValidator, IValidator<UpdateUserDto> updateUserValidator, IValidator<VerifyUserDto> verifyUserValidator, IValidator<ResetPasswordDto> resetPwdValidator, IValidator<ChangeStatusDto> changeStatusValidator)
    {
        _userService = userService;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
        _verifyUserValidator = verifyUserValidator;
        _resetPwdValidator = resetPwdValidator;
        _changeStatusValidator = changeStatusValidator;
    }

    private IActionResult? EnsureAdminOrOwn(int id)
    {
        // 1) Extraer userId y userRole del JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userIdClaim == null || userRoleClaim == null)
            return Unauthorized();

        var userIdFromToken = int.Parse(userIdClaim);
        
        // 2) Si no es Admin y el id no coincide, denegar
        if (userRoleClaim == IUserRole.User && userIdFromToken != id)
            return Forbid();

        return null;
    }
    
    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)] 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _userService.GetAll();
        return Ok(list);
    }
    
    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.Branch+","+IUserRole.User)] 
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var error = EnsureAdminOrOwn(id);
        if (error != null) return error;
        
        var user = await _userService.GetById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.Branch+","+IUserRole.User)] 
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        var user = await _userService.GetByEmail(email);
        if (user == null) return NotFound();
        
        var error = EnsureAdminOrOwn(user.id);
        if (error != null) return error;
        
        return Ok(user);
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
    
    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.User)] 
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var error = EnsureAdminOrOwn(id);
        if (error != null) return error;
        
        var vr = await _updateUserValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var updated = await _userService.Update(dto, id);
        return Ok(updated);
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

    [HttpPatch("{id:int}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ResetPasswordDto dto)
    {
        var vr = await _resetPwdValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var ok = await _userService.ChangePassword(dto, id);
        return Ok(new { passwordChanged = ok });
    }

    [HttpGet("{id:int}/send-verification-code")]
    public async Task<IActionResult> SendNewVerificationCode(int id)
    {
        await _userService.SendVerificationCode(id);
        return NoContent();
    }

    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.Branch)] 
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusDto dto)
    {
        var vr = await _changeStatusValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);
        
        var ok = await _userService.ChangeStatus(id, dto.status);
        return Ok(new { statusChanged = ok });
    }

    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.Branch+","+IUserRole.User)] 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var error = EnsureAdminOrOwn(id);
        if (error != null) return error;
        
        await _userService.Delete(id);
        return NoContent();
    }

}