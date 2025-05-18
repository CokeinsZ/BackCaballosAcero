using Core.DTOs;
using FluentValidation;

namespace Application.Validator.MotoInventory;

public class ChangeMotoInventoryStatusValidator : AbstractValidator<ChangeMotoInventoryStatusDto>
{
    private static readonly string[] ValidStatuses = {
        "Available", "Sold", "Under Customization", "Ready", "Delivered"
    };

    public ChangeMotoInventoryStatusValidator()
    {
        RuleFor(x => x.status)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"El status debe ser uno de: {string.Join(", ", ValidStatuses)}");
    }
}