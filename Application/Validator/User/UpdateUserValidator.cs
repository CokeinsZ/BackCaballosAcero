using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class UpdateUserValidator: AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.name).MaximumLength(32).When(x => x.name != null);
        RuleFor(x => x.last_name).MaximumLength(32).When(x => x.last_name != null);
        RuleFor(x => x.identification_document).MaximumLength(16).Matches(@"^\d+$").WithMessage("Identification document must contain only numeric characters").When(x => x.identification_document != null);
        RuleFor(x => x.country).MaximumLength(32).When(x => x.country != null);
        RuleFor(x => x.city).MaximumLength(32).When(x => x.city != null);
        RuleFor(x => x.address).MaximumLength(100).When(x => x.address != null);
        RuleFor(x => x.phone_number).MaximumLength(20).When(x => x.phone_number != null);
    }

}