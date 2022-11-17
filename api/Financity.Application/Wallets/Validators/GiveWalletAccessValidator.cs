using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Wallets.Commands;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class GiveWalletAccessValidator : AbstractValidator<GiveWalletAccessCommand>
{
    public GiveWalletAccessValidator(IApplicationDbContext dbContext, ICurrentUserService userService)
    {
        RuleFor(x => x.UserEmail).EmailAddress().NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty().HasAccessToWallet(dbContext, userService);
    }
}