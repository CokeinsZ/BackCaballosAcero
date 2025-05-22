using Core.DTOs;
using FluentValidation;

namespace Application.Validator.MotoInventory;

public class UpdateMotoInventoryValidator : AbstractValidator<UpdateMotoInventoryDto>
{
    private static readonly string LicensePattern = @"^.{0,7}$";
    private static readonly string KmPattern = @"^\d{1,7}$";

    public UpdateMotoInventoryValidator()
    {
        RuleFor(x => x.post_id)
            .GreaterThan(0)
            .When(x => x.post_id.HasValue)
            .WithMessage("El post_id debe ser un entero mayor que 0.");
        
        RuleFor(x => x.bill_id)
            .GreaterThan(0)
            .When(x => x.bill_id.HasValue)
            .WithMessage("El bill_id debe ser un entero mayor que 0.");
        
        RuleFor(x => x.is_new)
            .NotNull()
            .WithMessage("El campo is_new debe ser un valor booleano.")
            .When(x => x.is_new.HasValue);

        RuleFor(x => x.license_plate)
            .Matches(LicensePattern)
            .WithMessage("La license_plate debe tener máximo 7 caracteres.")
            .When(x => x.license_plate is not null && x.license_plate != string.Empty);

        RuleFor(x => x.km)
            .Matches(KmPattern)
            .WithMessage("El km debe ser un número de hasta 7 dígitos.")
            .When(x => x.km is not null && x.km != string.Empty);
    }
}