using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Transactions.Commands;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Validators;

public sealed class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(512);
        RuleFor(x => x.WalletId).NotEmpty().HasUserAccessToWallet(dbContext);
        RuleFor(x => x.TransactionType).IsEnumName(typeof(TransactionType), false);
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.RecipientId).HasUserAccessOrNull<CreateTransactionCommand, Recipient>(dbContext);
        RuleFor(x => x.CategoryId).HasUserAccessOrNull<CreateTransactionCommand, Category>(dbContext);
        RuleFor(x => x.CurrencyId).NotEmpty().Exists<CreateTransactionCommand, Currency, string>(dbContext);
        RuleFor(x => x.LabelIds)
            .ForEach(y => y.NotEmpty())
            .HasUserAccess<CreateTransactionCommand, Label>(dbContext);

        WhenAsync(async (command, ct) => command.CurrencyId != await dbContext.GetDbSet<Wallet>()
                                                                              .AsNoTracking()
                                                                              .Where(x => x.Id == command.WalletId)
                                                                              .Select(x => x.CurrencyId)
                                                                              .FirstOrDefaultAsync(ct),
            () =>
                RuleFor(x => x.ExchangeRate)
                    .NotEmpty()
                    .WithMessage(
                        $"{nameof(CreateTransactionCommand.ExchangeRate)} should be specified when using other currency than wallet's default.")
                    .GreaterThan(0)
        ).Otherwise(() =>

            // When(x => x.ExchangeRate is not null, () => RuleFor())

            RuleFor(x => x.ExchangeRate).Equal(1).When(x => x.ExchangeRate is not null)
                                        .WithMessage(
                    $"{nameof(CreateTransactionCommand.ExchangeRate)} shouldn't be specified or equal 1 when using the same currency as the wallet.")
        );
    }
}