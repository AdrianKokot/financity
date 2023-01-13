using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Exceptions;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public sealed record GetWalletQuery(Guid EntityId) : IEntityQuery<WalletDetails>;

public sealed class GetWalletQueryHandler : EntityQueryHandler<GetWalletQuery, Wallet, WalletDetails>
{
    public GetWalletQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override Task<WalletDetails> Handle(GetWalletQuery query, CancellationToken cancellationToken)
    {
        if (DbContext.UserService.UserWalletIds.Contains(query.EntityId)) return base.Handle(query, cancellationToken);

        throw new EntityNotFoundException(nameof(Wallet), query.EntityId);
    }
}

public sealed record WalletDetails
(Guid Id, string Name, string CurrencyId, string CurrencyName,
 decimal StartingAmount, Guid OwnerId, string OwnerName) : IMapFrom<Wallet>;