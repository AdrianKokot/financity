using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Application.Common.Mappings;
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

public sealed class WalletListItem : IMapFrom<Wallet>
{
    public string Name { get; init; }

    public Guid CurrencyId { get; init; }
    public string CurrencyName { get; init; }
    public string CurrencyCode { get; init; }
}