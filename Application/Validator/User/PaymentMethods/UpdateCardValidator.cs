using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User.PaymentMethods;

public class UpdateCardValidator : AbstractValidator<UpdateCardDto>
{
    private static readonly string[] ValidTypes = { "Credit", "Debit" };
    private const string ExpirationPattern = @"^(0[1-9]|1[0-2])\/\d{2}$";

    public UpdateCardValidator()
    {
        RuleFor(x => x.owner)
            .NotEmpty()
            .MaximumLength(32)
            .When(x => x.owner is not null)
            .WithMessage("El owner debe tener máximo 32 caracteres.");

        RuleFor(x => x.pan)
            .NotEmpty()
            .MaximumLength(16)
            .When(x => x.pan is not null)
            .WithMessage("El pan debe tener máximo 16 caracteres.");

        RuleFor(x => x.cvv)
            .NotEmpty()
            .MaximumLength(6)
            .When(x => x.cvv is not null)
            .WithMessage("El cvv debe tener máximo 6 caracteres.");

        RuleFor(x => x.type)
            .NotEmpty()
            .Must(t => ValidTypes.Contains(t))
            .When(x => x.type is not null)
            .WithMessage($"El type debe ser uno de: {string.Join(", ", ValidTypes)}");

        RuleFor(x => x.expiration_date)
            .NotEmpty()
            .Matches(ExpirationPattern)
            .When(x => x.expiration_date is not null)
            .WithMessage("La expiration_date debe tener el formato MM/AA.");
    }
}