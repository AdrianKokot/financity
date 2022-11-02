using Financity.Application.Categories.Commands;
using FluentValidation;

namespace Financity.Application.Categories.Validators;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.WalletId).NotEmpty();
    }
}