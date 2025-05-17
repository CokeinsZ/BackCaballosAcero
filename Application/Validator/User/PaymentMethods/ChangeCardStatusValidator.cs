using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User.PaymentMethods;

public class ChangeCardStatusValidator : AbstractValidator<ChangeCardStatusDto>
{
    private static readonly string[] ValidStatuses = { "Active", "Inactive" };

    public ChangeCardStatusValidator()
    {
        RuleFor(x => x.status)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"El status debe ser uno de: {string.Join(", ", ValidStatuses)}");
    }
}