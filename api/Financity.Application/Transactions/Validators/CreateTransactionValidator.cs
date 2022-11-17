using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Transactions.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator(IApplicationDbContext dbContext, ICurrentUserService userService)
    {
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(255);
        RuleFor(x => x.WalletId).NotEmpty().HasAccessToWallet(dbContext, userService);
        RuleFor(x => x.TransactionType).NotNull().IsInEnum();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.LabelIds).ForEach(y => y.NotEmpty());
    }
}