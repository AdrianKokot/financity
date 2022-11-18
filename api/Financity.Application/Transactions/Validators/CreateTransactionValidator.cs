using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Transactions.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(255);
        RuleFor(x => x.WalletId).NotEmpty().HasUserAccessToWallet(dbContext);
        RuleFor(x => x.TransactionType).NotNull().IsInEnum();
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.RecipientId).HasUserAccessOrNull<CreateTransactionCommand, Recipient>(dbContext);
        RuleFor(x => x.CategoryId).HasUserAccessOrNull<CreateTransactionCommand, Category>(dbContext);
        RuleFor(x => x.CurrencyId).NotEmpty().Exists<CreateTransactionCommand, Currency>(dbContext);
        RuleFor(x => x.LabelIds)
            .ForEach(y => y.NotEmpty())
            .HasUserAccess<CreateTransactionCommand, Label>(dbContext);
    }
}