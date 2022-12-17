using Financity.Application.Abstractions.Data;
using Financity.Application.Budgets.Commands;
using Financity.Application.Common.Extensions;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Budgets.Validators;

public sealed class CreateBudgetValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .Must(x => x >= 0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.TrackedCategoriesId)
            .ForEach(x => x.NotEmpty())
            .HasUserAccess<CreateBudgetCommand, Category>(dbContext);

        RuleFor(x => x.CurrencyId)
            .NotEmpty()
            .Exists<CreateBudgetCommand, Currency, string>(dbContext);
    }
}