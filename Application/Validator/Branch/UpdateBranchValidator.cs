using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Branch;

public class UpdateBranchValidator : AbstractValidator<UpdateBranchDto>
{
    public UpdateBranchValidator()
    {
        RuleFor(x => x.nit)
            .MaximumLength(10)
            .Matches(@"^\d+$")
            .When(x => x.nit is not null)
            .WithMessage("El nit es obligatorio y puede contener como maximo 10 digitos numericos.");
        
        RuleFor(x => x.name)
            .MaximumLength(32)
            .When(x => x.name is not null)
            .WithMessage("El name puede tener máximo 32 caracteres.");

        RuleFor(x => x.country)
            .MaximumLength(32)
            .When(x => x.country is not null)
            .WithMessage("El country puede tener máximo 32 caracteres.");

        RuleFor(x => x.city)
            .MaximumLength(32)
            .When(x => x.city is not null)
            .WithMessage("El city puede tener máximo 32 caracteres.");

        RuleFor(x => x.address)
            .MaximumLength(32)
            .When(x => x.address is not null)
            .WithMessage("La address puede tener máximo 32 caracteres.");
    }
}