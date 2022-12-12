using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Wallets.Queries;
using Financity.Domain.Enums;
using FluentValidation;

namespace Financity.Application.Wallets.Validators;

public class GetWalletUsersWithSharedAccessValidator : AbstractValidator<GetWalletUsersWithSharedAccessQuery>
{
    public GetWalletUsersWithSharedAccessValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.WalletId).HasUserAccessToWallet(dbContext, WalletAccessLevel.Owner);
    }
}