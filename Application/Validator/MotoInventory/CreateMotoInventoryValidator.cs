using Core.DTOs;
using FluentValidation;

namespace Application.Validator.MotoInventory;

public class CreateMotoInventoryValidator : AbstractValidator<CreateMotoInventoryDto>
{
    private static readonly string LicensePattern = @"^.{0,7}$";
    private static readonly string KmPattern = @"^\d{1,7}$";

    public CreateMotoInventoryValidator()
    {
        RuleFor(x => x.moto_id)
            .GreaterThan(0)
            .WithMessage("El moto_id debe ser un entero mayor que 0.");

        RuleFor(x => x.branch_id)
            .GreaterThan(0)
            .WithMessage("El branch_id debe ser un entero mayor que 0.");

        RuleFor(x => x.post_id)
            .GreaterThan(0)
            .When(x => x.post_id.HasValue)
            .WithMessage("El post_id, si se proporciona, debe ser un entero mayor que 0.");

        RuleFor(x => x.license_plate)
            .Matches(LicensePattern)
            .WithMessage("La license_plate debe tener máximo 7 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.license_plate));

        RuleFor(x => x.km)
            .Matches(KmPattern)
            .WithMessage("El km debe ser un número de hasta 7 dígitos.")
            .When(x => !string.IsNullOrEmpty(x.km));
    }
}