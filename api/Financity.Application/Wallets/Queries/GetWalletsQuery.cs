using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public class GetWalletsQuery : FilteredEntitiesQuery<WalletListItem>
{
    public GetWalletsQuery(QuerySpecification<WalletListItem> querySpecification) : base(querySpecification)
    {
    }
}

public sealed class GetWalletsQueryHandler : FilteredEntitiesQueryHandler<GetWalletsQuery, Wallet, WalletListItem>
{
    public GetWalletsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override Task<IEnumerable<WalletListItem>> Handle(GetWalletsQuery query, CancellationToken cancellationToken)
    {
        return FilterAndMapAsync(
            query,
            q => q.Where(x => DbContext.UserService.UserWalletIds.Contains(x.Id)).Include(x => x.Transactions),
            cancellationToken
        );
    }
}

public sealed record WalletListItem
(Guid Id, string Name, string CurrencyId, string CurrencyName, decimal CurrentState, decimal StartingAmount,
 Guid OwnerId) : IMapFrom<Wallet>;