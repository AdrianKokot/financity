using Financity.Application.Transactions.Commands;
using Financity.Domain.Enums;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(255);
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.TransactionType).NotEmpty().IsInEnum();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.LabelIds).NotEmpty();
    }
}