using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public class GetWalletsQuery : FilteredEntitiesQuery<WalletListItem>
{
    public GetWalletsQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class GetWalletsQueryHandler : FilteredEntitiesQueryHandler<GetWalletsQuery, Wallet, WalletListItem>
{
    public GetWalletsQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record WalletListItem(Guid Id, string Name, Guid CurrencyId, string CurrencyName, string CurrencyCode);