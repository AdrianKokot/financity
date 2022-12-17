using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletUsersWithSharedAccessQuery : FilteredEntitiesQuery<UserWithSharedAccessListItem>
{
    public GetWalletUsersWithSharedAccessQuery(Guid walletId,
                                               QuerySpecification<UserWithSharedAccessListItem> querySpecification) :
        base(querySpecification)
    {
        WalletId = walletId;
    }

    public Guid WalletId { get; init; }
}

public sealed class GetWalletUsersWithAccessQueryHandler : FilteredEntitiesQueryHandler<
    GetWalletUsersWithSharedAccessQuery, User, UserWithSharedAccessListItem>
{
    public GetWalletUsersWithAccessQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext,
        mapper)
    {
    }

    public override Task<IEnumerable<UserWithSharedAccessListItem>> Handle(
        GetWalletUsersWithSharedAccessQuery query, CancellationToken cancellationToken)
    {
        return FilterAndMapAsync(
            query,
            q => q.Where(x => x.SharedWallets.Any(y => y.Id == query.WalletId)),
            cancellationToken
        );
    }

    protected override IQueryable<User> ExecuteSearch(IQueryable<User> query, string search)
    {
        search = search.ToLower(CultureInfo.InvariantCulture));
        return query.Where(x =>
            x.Name.ToLower().Contains(search) || (x.Email ?? string.Empty).ToLower().Contains(search));
    }
}

public sealed record UserWithSharedAccessListItem
    (Guid Id, string Name, string Email) : IMapFrom<User>;