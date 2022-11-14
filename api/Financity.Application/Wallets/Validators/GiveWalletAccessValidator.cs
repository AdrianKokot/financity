using Financity.Application.Wallets.Commands;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class GiveWalletAccessValidator : AbstractValidator<GiveWalletAccessCommand>
{
    public GiveWalletAccessValidator()
    {
        RuleFor(x => x.UserEmail).EmailAddress().NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
    }
}