using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class ChangeRoleValidator : AbstractValidator<ChangeRoleDto>
{
    private static readonly string[] ValidStatuses =
        ["user", "admin", "branch"];

    public ChangeRoleValidator()
    {
        RuleFor(x => x.role)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"El rol debe ser uno de: {string.Join(", ", ValidStatuses)}");
    }
}