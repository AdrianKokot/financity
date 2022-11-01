using Financity.Application.Abstractions.Data;
using Financity.Application.Common.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletQuery : IEntityQuery<Wallet>
{
    public Guid EntityId { get; set; }
}

public sealed class GetWalletQueryHandler : EntityQueryHandler<GetWalletQuery, Wallet>
{
    public GetWalletQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}