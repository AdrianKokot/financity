using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Transactions.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(255);
        RuleFor(x => x.LabelIds)
            .ForEach(y => y.NotEmpty())
            .HasUserAccess<UpdateTransactionCommand, Label>(dbContext);

        RuleFor(x => x.Id).HasUserAccess<UpdateTransactionCommand, Transaction>(dbContext);
    }
}