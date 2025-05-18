using System.Security.Claims;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/branch")]
public class BranchController : ControllerBase
{
    private readonly IBranchService _service;
    private readonly IValidator<CreateBranchDto> _createValidator;
    private readonly IValidator<UpdateBranchDto> _updateValidator;

    public BranchController(
        IBranchService service,
        IValidator<CreateBranchDto> createValidator,
        IValidator<UpdateBranchDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private IActionResult? EnsureAdminOrOwn(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var branchClaim = User.FindFirst("branchId")?.Value;
        if (role == null) return Unauthorized();
        if (role == IUserRole.Branch && int.Parse(branchClaim ?? "-1") != id)
            return Forbid();
        return null;
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAll();
        return Ok(items);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var err = EnsureAdminOrOwn(id);
        if (err != null) return err;

        var item = await _service.GetById(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [Authorize(Roles = "" + IUserRole.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
    {
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var created = await _service.Create(dto);
        return Ok(created);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto dto)
    {
        var err = EnsureAdminOrOwn(id);
        if (err != null) return err;

        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var updated = await _service.Update(dto, id);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var err = EnsureAdminOrOwn(id);
        if (err != null) return err;

        var ok = await _service.Delete(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}