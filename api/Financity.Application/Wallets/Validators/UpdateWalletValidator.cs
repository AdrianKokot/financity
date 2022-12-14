using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Wallets.Commands;
using Financity.Domain.Enums;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class UpdateWalletValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.Id).HasUserAccessToWallet(dbContext, WalletAccessLevel.Owner);

        RuleFor(x => x.StartingAmount).NotNull();
    }
}