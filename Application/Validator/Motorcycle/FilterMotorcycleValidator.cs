using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Motorcycle;

public class FilterMotorcycleValidator: AbstractValidator<FilterMotorcycleDto>
{
    public FilterMotorcycleValidator()
    {
        RuleFor(x => x.Brand)
            .MaximumLength(32)
            .When(x => x.Brand != null);

        RuleFor(x => x.Model)
            .MaximumLength(32)
            .When(x => x.Model != null);

        RuleFor(x => x.CC)
            .Matches(@"^\d+$")
            .WithMessage("CC must contain only numeric characters")
            .MaximumLength(3)
            .When(x => x.CC != null);

        RuleFor(x => x.Color)
            .MaximumLength(32)
            .When(x => x.Color != null);
    }
}