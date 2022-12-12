using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.DetailsQuery;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletUsersWithSharedAccessQuery : FilteredEntitiesQuery<UserWithSharedAccessListItem>
{
    public Guid WalletId { get; init; }
    public GetWalletUsersWithSharedAccessQuery(Guid walletId, QuerySpecification<UserWithSharedAccessListItem> querySpecification) : base(querySpecification)
    {
        WalletId = walletId;
    }
}

public sealed class GetWalletUsersWithAccessQueryHandler : FilteredEntitiesQueryHandler<GetWalletUsersWithSharedAccessQuery, Wallet, UserWithSharedAccessListItem>
{
    public GetWalletUsersWithAccessQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override Task<IEnumerable<UserWithSharedAccessListItem>> Handle(GetWalletUsersWithSharedAccessQuery query, CancellationToken cancellationToken)
    {
        return FilterAndMapAsync(
            query,
            q => q.Where(x => x.Id == query.WalletId).SelectMany(x => x.UsersWithSharedAccess),
            cancellationToken
        );
    }
}

public sealed record UserWithSharedAccessListItem
    (Guid Id, string Name, string Email) : IMapFrom<User>;