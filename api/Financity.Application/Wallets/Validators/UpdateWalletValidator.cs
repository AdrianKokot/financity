using Financity.Application.Wallets.Commands;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class UpdateWalletValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
    }
}