using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class CreateUserValidator: AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.last_name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.identification_document).NotEmpty().MaximumLength(16).Matches(@"^\d+$").WithMessage("Identification document must contain only numeric characters");
        RuleFor(x => x.country).NotEmpty().MaximumLength(32);
        RuleFor(x => x.city).NotEmpty().MaximumLength(32);
        RuleFor(x => x.address).MaximumLength(100).When(x => x.address != null);
        RuleFor(x => x.password).NotEmpty();
        RuleFor(x => x.email).NotEmpty().EmailAddress();
        RuleFor(x => x.phone_number).MaximumLength(20).When(x => x.phone_number != null);
    }
    
}