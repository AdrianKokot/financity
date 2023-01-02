using System.Globalization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Common;
using Financity.Domain.Entities;

namespace Financity.Application.Categories.Queries;

public sealed class GetCategoriesQuery : FilteredEntitiesQuery<CategoryListItem>
{
    public GetCategoriesQuery(QuerySpecification<CategoryListItem> querySpecification) : base(querySpecification)
    {
    }

    public string? WalletCurrencyId { get; set; }
}

public sealed class
    GetCategoriesQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetCategoriesQuery, Category, CategoryListItem>
{
    public GetCategoriesQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    private static IQueryable<Category> ApplyAdditionalFilters(IQueryable<Category> q, GetCategoriesQuery query)
    {
        if (!string.IsNullOrEmpty(query.WalletCurrencyId))
            q = q.Where(x => x.Wallet.CurrencyId.Equals(query.WalletCurrencyId));

        return q;
    }

    public override Task<IEnumerable<CategoryListItem>> Handle(GetCategoriesQuery query,
                                                               CancellationToken cancellationToken)
    {
        return FilterAndMapAsync(
            query,
            q => ApplyAdditionalFilters(q, query),
            cancellationToken
        );
    }

    protected override IQueryable<Category> ExecuteSearch(IQueryable<Category> query, string search)
    {
        search = search.ToLower(CultureInfo.InvariantCulture);

        return query.Where(x =>
            x.Name.ToLower().Contains(search));
    }
}

public sealed record CategoryListItem(Guid Id, string Name, Guid WalletId, Appearance Appearance, string TransactionType, string WalletName) : IMapFrom<Category>;