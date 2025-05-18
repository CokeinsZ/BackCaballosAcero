using System.Security.Claims;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/branch/{branchId:int}/inventory")]
public class MotoInventoryController : ControllerBase
{
    private readonly IMotoInventoryService _motoInventoryService;
    private readonly IValidator<CreateMotoInventoryDto> _createValidator;
    private readonly IValidator<UpdateMotoInventoryDto> _updateValidator;
    private readonly IValidator<ChangeMotoInventoryStatusDto> _statusValidator;

    public MotoInventoryController(
        IMotoInventoryService motoInventoryService,
        IValidator<CreateMotoInventoryDto> createValidator,
        IValidator<UpdateMotoInventoryDto> updateValidator,
        IValidator<ChangeMotoInventoryStatusDto> statusValidator)
    {
        _motoInventoryService = motoInventoryService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _statusValidator = statusValidator;
    }

    private IActionResult? EnsureAdminOrBranch(int branchId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var claim = User.FindFirst("branchId")?.Value;
        if (role == null || claim == null)
            return Unauthorized();
        if (role == IUserRole.Branch && int.Parse(claim) != branchId)
            return Forbid();
        return null;
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet]
    public async Task<IActionResult> GetByBranch(int branchId)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;

        var items = await _motoInventoryService.GetByBranch(branchId);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _motoInventoryService.GetById(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpPost]
    public async Task<IActionResult> Create(int branchId, [FromBody] CreateMotoInventoryDto dto)
    {
        dto.branch_id = branchId;
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var created = await _motoInventoryService.Create(dto);
        return Ok(created);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int branchId, int id, [FromBody] UpdateMotoInventoryDto dto)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;

        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var updated = await _motoInventoryService.Update(dto, id);
        if (updated == null) return NotFound();
        if (updated.branch_id != branchId) return Forbid();
        return Ok(updated);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int branchId, int id, [FromBody] ChangeMotoInventoryStatusDto dto)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;

        var vr = await _statusValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var ok = await _motoInventoryService.ChangeStatus(id, dto.status);
        if (!ok) return NotFound();
        return Ok(new { status_changed = ok });
        
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int branchId, int id)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;

        var ok = await _motoInventoryService.Delete(id);
        if (!ok) return NotFound();
        return Ok(new { Deleted = ok });
    }
}