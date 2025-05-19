using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Post;

public class CreatePostValidator : AbstractValidator<CreatePostDto>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.branch_id)
            .GreaterThan(0)
            .WithMessage("El branch_id debe ser un entero mayor que 0.");

        RuleFor(x => x.price)
            .GreaterThan(0)
            .WithMessage("El price debe ser un número mayor que 0.");

        RuleFor(x => x.motoInventories)
            .NotEmpty()
            .WithMessage("La lista de motos no puede estar vacía.")
            .Must(list => list.All(id => id > 0))
            .WithMessage("Cada moto en la lista debe tener un id mayor que 0.");

        RuleFor(x => x.description)
            .MaximumLength(500)
            .WithMessage("La descripción debe tener máximo 500 caracteres.")
            .When(x => x.description is not null);

    }
}