using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class ChangeStatusValidator: AbstractValidator<ChangeStatusDto>
{
    private static readonly string[] ValidStatuses =
        ["Active", "Not Verified", "Inactive", "Banned"];

    public ChangeStatusValidator()
    {
        RuleFor(x => x.status)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"El status debe ser uno de: {string.Join(", ", ValidStatuses)}");
    }
}