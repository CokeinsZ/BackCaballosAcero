using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class ResetPasswordValidator: AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.verification_code).NotEmpty();
        RuleFor(x => x.password).NotEmpty();
    }
}