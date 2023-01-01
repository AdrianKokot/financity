using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Wallets.Commands;
using Financity.Domain.Enums;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class ResignWalletAccessValidator : AbstractValidator<ResignWalletAccessCommand>
{
    public ResignWalletAccessValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.WalletId)
            .NotEmpty()
            .HasUserAccessToWallet(dbContext, WalletAccessLevel.Shared);
    }
}