using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Motorcycle;

public class UpdateMotorcycleValidator: AbstractValidator<UpdateMotorcycleDto>
{
    public UpdateMotorcycleValidator()
    {
        RuleFor(x => x.Brand)
            .MaximumLength(32)
            .When(x => x.Brand != null);

        RuleFor(x => x.Model)
            .MaximumLength(32)
            .When(x => x.Model != null);

        RuleFor(x => x.CC)
            .MaximumLength(3)
            .Matches(@"^\d+$")
            .WithMessage("CC must contain only numeric characters")
            .When(x => x.CC != null);

        RuleFor(x => x.Color)
            .MaximumLength(32)
            .When(x => x.Color != null);

    }
}