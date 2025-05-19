using System.Security.Claims;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/branch/{branchId:int}/posts")]
public class PostController : ControllerBase
{
    private readonly IPostService _service;
    private readonly IValidator<CreatePostDto> _createValidator;
    private readonly IValidator<UpdatePostDto> _updateValidator;
    private readonly IValidator<ChangePostStatusDto> _statusValidator;

    public PostController(
        IPostService service,
        IValidator<CreatePostDto> createValidator,
        IValidator<UpdatePostDto> updateValidator,
        IValidator<ChangePostStatusDto> statusValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _statusValidator = statusValidator;
    }

    private IActionResult? EnsureAdminOrBranch(int branchId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var claim = User.FindFirst("branchId")?.Value;
        if (role == null )
            return Unauthorized();
        if (role == IUserRole.Branch && (claim == null || int.Parse(claim) != branchId))
            return Forbid();
        return null;
    }

    [HttpGet]
    public async Task<IActionResult> GetByBranch(int branchId)
    {
        var posts = await _service.GetByBranch(branchId);
        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int branchId, int id)
    {
        var post = await _service.GetById(id);
        if (post == null) return NotFound();
        if (post.branch_id != branchId) return Forbid();
        return Ok(post);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpPost]
    public async Task<IActionResult> Create(int branchId, [FromBody] CreatePostDto dto)
    {
        dto.branch_id = branchId;
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var created = await _service.Create(dto);
        return Ok(created);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int branchId, int id, [FromBody] UpdatePostDto dto)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;
        
        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var updated = await _service.Update(dto, id);
        if (updated == null) return NotFound();
        if (updated.branch_id != branchId) return Forbid();
        return Ok(updated);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int branchId, int id, [FromBody] ChangePostStatusDto dto)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;

        var vr = await _statusValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        var ok = await _service.ChangeStatus(id, dto.status);
        return Ok(new { statusChanged = ok });
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int branchId, int id)
    {
        var err = EnsureAdminOrBranch(branchId);
        if (err != null) return err;

        var ok = await _service.Delete(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}