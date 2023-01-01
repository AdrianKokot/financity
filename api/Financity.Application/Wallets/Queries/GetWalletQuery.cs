using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public sealed record GetWalletQuery(Guid EntityId) : IEntityQuery<WalletDetails>;

public sealed class GetWalletQueryHandler : EntityQueryHandler<GetWalletQuery, Wallet, WalletDetails>
{
    public GetWalletQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record WalletDetails
(Guid Id, string Name, string CurrencyId, string CurrencyName,
 decimal StartingAmount, Guid OwnerId) : IMapFrom<Wallet>;