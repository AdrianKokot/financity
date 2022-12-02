using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Wallets.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.CurrencyId)
            .NotEmpty()
            .Exists<CreateWalletCommand, Currency, string>(dbContext);
    }
}