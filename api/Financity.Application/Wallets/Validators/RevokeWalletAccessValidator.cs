using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Wallets.Commands;
using Financity.Domain.Enums;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public sealed class RevokeWalletAccessValidator : AbstractValidator<RevokeWalletAccessCommand>
{
    public RevokeWalletAccessValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.UserEmail).EmailAddress().NotEmpty()
                                 .Must(x => x.ToUpper() != dbContext.UserService.NormalizedUserEmail)
                                 .WithMessage("You cannot revoke wallet access from yourself.");

        RuleFor(x => x.WalletId).NotEmpty().HasUserAccessToWallet(dbContext, WalletAccessLevel.Owner);
    }
}