using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Branch;

public class CreateBranchValidator : AbstractValidator<CreateBranchDto>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.name)
            .NotEmpty()
            .MaximumLength(32)
            .WithMessage("El name es obligatorio y debe tener máximo 32 caracteres.");

        RuleFor(x => x.country)
            .NotEmpty()
            .MaximumLength(32)
            .WithMessage("El country es obligatorio y debe tener máximo 32 caracteres.");

        RuleFor(x => x.city)
            .NotEmpty()
            .MaximumLength(32)
            .WithMessage("El city es obligatorio y debe tener máximo 32 caracteres.");

        RuleFor(x => x.address)
            .MaximumLength(32)
            .When(x => x.address is not null)
            .WithMessage("La address puede tener máximo 32 caracteres.");
    }
}