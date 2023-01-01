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

    protected override IQueryable<CategoryListItem> Project(IQueryable<Category> q, GetCategoriesQuery query)
    {
        var shouldAddWalletName = query.QuerySpecification.Filters.All(x => !x.Key.Equals(nameof(Category.WalletId)));

        return shouldAddWalletName
            ? q.ProjectTo<CategoryListItem>(Mapper.ConfigurationProvider, x => x.WalletName)
            : base.Project(q, query);
    }
}

public class CategoryListItem : IMapFrom<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid WalletId { get; set; }
    public Appearance Appearance { get; set; }
    public string TransactionType { get; set; }
    public string? WalletName { get; set; }
    public static bool MapWalletName => false;

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<Category, CategoryListItem>()
               .ForMember(x => x.WalletName, opt =>
               {
                   if (!MapWalletName)
                   {
                       opt.ExplicitExpansion();
                   }
               });
    }
}