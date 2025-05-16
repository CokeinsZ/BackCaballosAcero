using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Motorcycle;

public class CreateMotorcycleValidator: AbstractValidator<CreateMotorcycleDto>
{
    public CreateMotorcycleValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.CC)
            .NotEmpty()
            .Matches(@"^\d+$")
            .WithMessage("CC must contain only numeric characters")
            .MaximumLength(3);

        RuleFor(x => x.Color)
            .MaximumLength(32)
            .When(x => x.Color != null);

    }
}