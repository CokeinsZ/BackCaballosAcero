using Core.DTOs;
using FluentValidation;

namespace Application.Validator.Post;

public class ChangePostStatusValidator : AbstractValidator<ChangePostStatusDto>
{
    private static readonly string[] ValidStatuses = { "Available", "SoldOut" };

    public ChangePostStatusValidator()
    {
        RuleFor(x => x.status)
            .NotEmpty()
            .WithMessage("El status es obligatorio.")
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"El status debe ser uno de: {string.Join(", ", ValidStatuses)}");
    }
}
