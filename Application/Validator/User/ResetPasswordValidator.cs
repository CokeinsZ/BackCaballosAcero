using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class ResetPasswordValidator: AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.verification_code).NotEmpty();
        RuleFor(x => x.password).NotEmpty().MinimumLength(4).WithMessage("Password must be at least 4 characters long");
    }
}