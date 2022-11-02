using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public sealed record GetWalletQuery(Guid EntityId) : IEntityQuery<WalletDetails>;

public sealed class GetWalletQueryHandler : EntityQueryHandler<GetWalletQuery, Wallet, WalletDetails>
{
    public GetWalletQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record WalletDetails(Guid Id, string Name, Guid CurrencyId, string CurrencyName, string CurrencyCode);