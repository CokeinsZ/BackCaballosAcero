using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User;

public class LoginUserValidator: AbstractValidator<LoginUserDto>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.email).NotEmpty().EmailAddress();
        RuleFor(x => x.password).NotEmpty();
    }
}