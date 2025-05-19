using System.Security.Claims;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

namespace Api.Controllers;

[ApiController]
[Route("api/bill")]
public class BillController : ControllerBase
{
    private readonly IBillService _service;
    private readonly IValidator<CreateBillDto> _createValidator;
    private readonly IValidator<UpdateBillDto> _updateValidator;

    public BillController(
        IBillService service,
        IValidator<CreateBillDto> createValidator,
        IValidator<UpdateBillDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private IActionResult? EnsureAdminOrBranch(int branchId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var claim = User.FindFirst("branchId")?.Value;
        if (role == null || claim == null) return Unauthorized();
        if (role == IUserRole.Branch && int.Parse(claim) != branchId)
            return Forbid();
        return null;
    }

    private IActionResult? EnsureAdminOrOwn(int userId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (role == null || claim == null) return Unauthorized();
        if (role == IUserRole.User && int.Parse(claim) != userId)
            return Forbid();
        return null;
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet("branch/{branchId:int}")]
    public async Task<IActionResult> GetByBranch(int branchId)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;

        var bills = await _service.GetByBranch(branchId);
        return Ok(bills);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var err = EnsureAdminOrOwn(userId);
        if (err != null) return err;

        var bills = await _service.GetByUser(userId);
        return Ok(bills);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet("motorcycle/{motoId:int}")]
    public async Task<IActionResult> GetByMotorcycle(int motoId)
    {
        var bills = await _service.GetByMotorcycle(motoId);
        return Ok(bills);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var bill = await _service.GetById(id);
        if (bill == null) return NotFound();

        var err = EnsureAdminOrOwn(bill.user_id.Value);
        if (err != null) return err;

        return Ok(bill);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBillDto dto)
    {
        var err = EnsureAdminOrOwn(dto.user_id);
        if (err != null) return err;

        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var bill = await _service.Create(dto);
        return Ok(bill);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBillDto dto)
    {
        var existing = await _service.GetById(id);
        if (existing == null) return NotFound();

        var err = EnsureAdminOrOwn(existing.user_id.Value);
        if (err != null) return err;

        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var updated = await _service.Update(dto, id);
        return Ok(updated);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetById(id);
        if (existing == null) return NotFound();

        var err = EnsureAdminOrOwn(existing.user_id.Value);
        if (err != null) return err;

        var ok = await _service.Delete(id);
        return Ok($"deleted: {ok}");
    }
}