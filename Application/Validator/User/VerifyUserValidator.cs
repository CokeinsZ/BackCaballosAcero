using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class VerifyUserValidator: AbstractValidator<VerifyUserDto>
{
    public VerifyUserValidator()
    {
        RuleFor(x => x.verification_code).NotEmpty();
        RuleFor(x => x.email).NotEmpty().EmailAddress();
    }
}