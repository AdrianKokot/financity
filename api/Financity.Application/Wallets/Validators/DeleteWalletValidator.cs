using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Wallets.Commands;
using Financity.Domain.Enums;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class DeleteWalletValidator : AbstractValidator<DeleteWalletCommand>
{
    public DeleteWalletValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Id).HasUserAccessToWallet(dbContext, WalletAccessLevel.Owner);
    }
}