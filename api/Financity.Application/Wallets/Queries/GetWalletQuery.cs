using Financity.Application.Abstractions.Data;
using Financity.Application.Common.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletQuery : EntityQuery<Wallet>
{
    public GetWalletQuery(string entityId) : base(entityId)
    {
    }
}

public sealed class GetWalletQueryHandler : EntityQueryHandler<GetWalletQuery, Wallet>
{
    public GetWalletQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}