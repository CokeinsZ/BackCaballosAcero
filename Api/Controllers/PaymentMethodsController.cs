using System.Security.Claims;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/user/{userId:int}/cards")]
public class PaymentMethodsController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly IValidator<CreateCardDto> _createCardValidator;
    private readonly IValidator<UpdateCardDto> _updateCardValidator;
    private readonly IValidator<ChangeCardStatusDto> _changeCardStatusValidator;

    public PaymentMethodsController(
        ICardService cardService,
        IValidator<CreateCardDto> createCardValidator,
        IValidator<UpdateCardDto> updateCardValidator,
        IValidator<ChangeCardStatusDto> changeCardStatusValidator)
    {
        _cardService = cardService;
        _createCardValidator = createCardValidator;
        _updateCardValidator = updateCardValidator;
        _changeCardStatusValidator = changeCardStatusValidator;
    }

    private IActionResult? EnsureAdminOrOwn(int userId)
    {
        var idClaim   = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        if (idClaim == null || roleClaim == null)
            return Unauthorized();

        var tokenId = int.Parse(idClaim);
        if (roleClaim == IUserRole.User && tokenId != userId)
            return Forbid();

        return null;
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpGet]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var err = EnsureAdminOrOwn(userId);
        if (err != null) return err;

        var cards = await _cardService.GetByUserId(userId);
        return Ok(cards);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int userId, int id)
    {
        var card = await _cardService.GetById(id);
        if (card == null) return NotFound();
        
        var err = EnsureAdminOrOwn(card.user_id);
        if (err != null) return err;

        return Ok(card);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpPost]
    public async Task<IActionResult> Create(int userId, [FromBody] CreateCardDto dto)
    {
        dto.user_id = userId;
        var vr = await _createCardValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);
        
        var err = EnsureAdminOrOwn(userId);
        if (err != null) return err;

        var card = await _cardService.Create(dto);
        return Ok(card);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int userId, int id, [FromBody] UpdateCardDto dto)
    {
        var err = EnsureAdminOrOwn(userId);
        if (err != null) return err;

        var vr = await _updateCardValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        await _cardService.Update(dto, id);
        return Ok();
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int userId, int id, [FromBody] ChangeCardStatusDto dto)
    {
        var err = EnsureAdminOrOwn(userId);
        if (err != null) return err;

        var vr = await _changeCardStatusValidator.ValidateAsync(dto);
        if (!vr.IsValid) return BadRequest(vr.Errors);

        await _cardService.ChangeCardStatus(dto, id);
        return Ok();
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch + "," + IUserRole.User)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int userId, int id)
    {
        var err = EnsureAdminOrOwn(userId);
        if (err != null) return err;

        await _cardService.Delete(id);
        return NoContent();
    }
}