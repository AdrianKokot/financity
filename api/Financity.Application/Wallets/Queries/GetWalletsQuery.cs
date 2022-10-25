using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public class GetWalletsQuery : FilteredEntitiesQuery<Wallet>
{
    public GetWalletsQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class GetWalletsQueryHandler : FilteredEntitiesQueryHandler<GetWalletsQuery, Wallet>
{
    public GetWalletsQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}