using Financity.Application.Budgets.Commands;
using FluentValidation;

namespace Financity.Application.Budgets.Validators;

public sealed class UpdateBudgetValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetValidator()
    {
        RuleFor(x => x.Amount).NotEmpty().Must(x => x >= 0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}