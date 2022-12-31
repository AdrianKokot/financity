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
}

public sealed class
    GetCategoriesQueryHandler : FilteredUserWalletEntitiesQueryHandler<GetCategoriesQuery, Category, CategoryListItem>
{
    public GetCategoriesQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    protected override IQueryable<Category> ExecuteSearch(IQueryable<Category> query, string search)
    {
        search = search.ToLower(CultureInfo.InvariantCulture);

        return query.Where(x =>
            x.Name.ToLower().Contains(search));
    }

    protected override IQueryable<CategoryListItem> Project(IQueryable<Category> q, GetCategoriesQuery query)
    {
        return query.QuerySpecification.Filters.Any(x =>
            x.Key.ToLower().Equals($"{nameof(Category.WalletId)}_eq".ToLower()))
            ? base.Project(q, query)
            : q.ProjectTo<CategoryListItem>(Mapper.ConfigurationProvider, x => x.WalletName);
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

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<Category, CategoryListItem>()
               .ForMember(x => x.WalletName, opt => opt.ExplicitExpansion());
    }
}