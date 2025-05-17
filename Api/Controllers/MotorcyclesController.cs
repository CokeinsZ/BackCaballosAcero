using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/motorcycles")]
public class MotorcyclesController : ControllerBase
{

    private readonly IMotorcycleService _motorcycleService;
    private readonly IValidator<CreateMotorcycleDto> _createMotorcycleValidator;
    private readonly IValidator<UpdateMotorcycleDto> _updateMotorcycleValidator;
    private readonly IValidator<FilterMotorcycleDto> _filterMotorcycleValidator;

    public MotorcyclesController(IMotorcycleService motorcycleService, IValidator<CreateMotorcycleDto> createMotorcycleValidator, IValidator<UpdateMotorcycleDto> updateMotorcycleValidator, IValidator<FilterMotorcycleDto> filterMotorcycleValidator)
    {
        _motorcycleService = motorcycleService;
        _createMotorcycleValidator = createMotorcycleValidator;
        _updateMotorcycleValidator = updateMotorcycleValidator;
        _filterMotorcycleValidator = filterMotorcycleValidator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var motorcycles = await _motorcycleService.GetAll();
        return Ok(motorcycles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var motorcycle = await _motorcycleService.GetById(id);
        return Ok(motorcycle);
    }

    [HttpPost("filter")]
    public async Task<IActionResult> GetByFilters([FromBody] FilterMotorcycleDto dto)
    {
        System.Console.WriteLine(dto);
        var validationResult = await _filterMotorcycleValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var motorcycles = await _motorcycleService.GetByFilters(dto);
        return Ok(motorcycles);
    }

    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.Branch)] 
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMotorcycleDto dto)
    {
        var validationResult = await _createMotorcycleValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var createdMotorcycle = await _motorcycleService.Create(dto);
        return Ok(createdMotorcycle);
    }

    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.Branch)] 
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMotorcycleDto dto)
    {
        var validationResult = await _updateMotorcycleValidator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var updated = await _motorcycleService.Update(dto, id);
        return Ok(updated);
    }

    [Authorize(Roles = ""+IUserRole.Admin+","+IUserRole.Branch)] 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _motorcycleService.Delete(id);
        return NoContent();
    }
}