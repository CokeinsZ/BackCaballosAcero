using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Bill;

public class UpdateBillValidator : AbstractValidator<UpdateBillDto>
{
    private static readonly string[] ValidMethods = { "Cash", "Card" };

    public UpdateBillValidator()
    {
        RuleFor(x => x.discount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.discount.HasValue)
            .WithMessage("El discount, si se proporciona, debe ser un número mayor o igual a 0.");

        RuleFor(x => x.payment_method)
            .Must(m => m is null || ValidMethods.Contains(m))
            .WithMessage($"El payment_method, si se proporciona, debe ser uno de: {string.Join(", ", ValidMethods)}.");

        RuleFor(x => x.moto_inventories_ids)
            .Must(list => list == null || list.All(id => id > 0))
            .When(x => x.moto_inventories_ids is not null)
            .WithMessage("Cada moto_inventories_id debe ser un entero mayor que 0 cuando se proporciona la lista.");
    }
}