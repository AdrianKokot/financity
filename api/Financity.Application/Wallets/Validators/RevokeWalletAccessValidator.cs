using Financity.Application.Wallets.Commands;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class RevokeWalletAccessValidator : AbstractValidator<RevokeWalletAccessCommand>
{
    public RevokeWalletAccessValidator()
    {
        RuleFor(x => x.UserEmail).EmailAddress().NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
    }
}