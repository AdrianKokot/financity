using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Transactions.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class DeleteTransactionValidator : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Id).ExistsForCurrentUser<DeleteTransactionCommand, Transaction>(dbContext);
    }
}