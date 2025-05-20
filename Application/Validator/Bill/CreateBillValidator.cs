using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Bill;

public class CreateBillValidator : AbstractValidator<CreateBillDto>
{
    private static readonly string[] ValidMethods = { "Cash", "Card" };

    public CreateBillValidator()
    {
        RuleFor(x => x.numberOfMotos)
            .GreaterThan(0)
            .WithMessage("El numero de motos a comprar debe ser mayor que 0.");
        
        RuleFor(x => x.post_id)
            .GreaterThan(0)
            .WithMessage("El post_id debe ser un entero mayor que 0.");
        
        RuleFor(x => x.user_id)
            .GreaterThan(0)
            .WithMessage("El user_id debe ser un entero mayor que 0.");

        RuleFor(x => x.discount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.discount.HasValue)
            .WithMessage("El discount, si se proporciona, debe ser un número mayor o igual a 0.");

        RuleFor(x => x.payment_method)
            .NotEmpty()
            .WithMessage("El payment_method es obligatorio.")
            //.Must(m => ValidMethods.Contains(m))
            .Must(m => m.ToLower().Equals("cash"))
            //.WithMessage($"El payment_method debe ser uno de: {string.Join(", ", ValidMethods)}.");
            .WithMessage($"Solo está disponible el pago en efectivo.");
        
    }
}