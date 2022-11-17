using Financity.Application.Budgets.Commands;
using FluentValidation;

namespace Financity.Application.Budgets.Validators;

public sealed class CreateBudgetValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetValidator()
    {
        RuleFor(x => x.Amount).NotEmpty().Must(x => x >= 0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TrackedCategoriesId).ForEach(x => x.NotEmpty());
    }
}