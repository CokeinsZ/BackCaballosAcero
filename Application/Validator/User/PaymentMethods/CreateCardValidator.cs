using Core.DTOs;
using FluentValidation;

namespace Application.Validator.User.PaymentMethods;

public class CreateCardValidator : AbstractValidator<CreateCardDto>
{
    private static readonly string[] ValidTypes = { "Credit", "Debit" };
    private const string ExpirationPattern = @"^(0[1-9]|1[0-2])\/\d{2}$";

    public CreateCardValidator()
    {
        RuleFor(x => x.owner)
            .NotEmpty()
            .MaximumLength(32)
            .WithMessage("El owner es obligatorio y debe tener máximo 32 caracteres.");

        RuleFor(x => x.pan)
            .NotEmpty()
            .MaximumLength(16)
            .WithMessage("El pan es obligatorio.");

        RuleFor(x => x.cvv)
            .NotEmpty()
            .MaximumLength(6)
            .WithMessage("El cvv es obligatorio.");

        RuleFor(x => x.type)
            .NotEmpty()
            .Must(t => ValidTypes.Contains(t))
            .WithMessage($"El type debe ser uno de: {string.Join(", ", ValidTypes)}");

        RuleFor(x => x.expiration_date)
            .NotEmpty()
            .Matches(ExpirationPattern)
            .WithMessage("La expiration_date es obligatoria y debe tener el formato MM/AA.");
    }
}