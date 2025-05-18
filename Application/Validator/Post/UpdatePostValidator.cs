using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Post;

public class UpdatePostValidator : AbstractValidator<UpdatePostDto>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.price)
            .GreaterThan(0)
            .When(x => x.price.HasValue)
            .WithMessage("El price, si se proporciona, debe ser un número mayor que 0.");

        RuleFor(x => x.motoInventories)
            .NotNull()
            .When(x => x.motoInventories is not null)
            .WithMessage("La lista de motos no puede ser nula.")
            .Must(list => list != null && list.All(id => id > 0))
            .When(x => x.motoInventories is not null)
            .WithMessage("Cada moto en la lista debe tener un id mayor que 0.");
    }
}