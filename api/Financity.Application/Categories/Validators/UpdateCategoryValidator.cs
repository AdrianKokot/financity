using Financity.Application.Categories.Commands;
using FluentValidation;

namespace Financity.Application.Categories.Validators;

public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Color).MaximumLength(64);
        RuleFor(x => x.IconName).MaximumLength(64);
    }
}