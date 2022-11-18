using Financity.Application.Abstractions.Data;
using Financity.Application.Budgets.Commands;
using Financity.Application.Common.Extensions;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Budgets.Validators;

public sealed class DeleteBudgetValidator : AbstractValidator<DeleteBudgetCommand>
{
    public DeleteBudgetValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Id).UserOwns<DeleteBudgetCommand, Budget>(dbContext);
    }
}